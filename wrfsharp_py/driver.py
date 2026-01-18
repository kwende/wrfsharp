from __future__ import annotations

import argparse
from datetime import datetime
from pathlib import Path
from typing import List

from wrfsharp_py.config import load_config
from wrfsharp_py.download import download_files, pick_files_for_latest_cycle
from wrfsharp_py.namelist import update_dates, update_physics
from wrfsharp_py.physics import expand_all
from wrfsharp_py.process import run_command


def _find_grib_dates(grib_files: List[Path]) -> tuple[datetime, datetime]:
    if not grib_files:
        raise RuntimeError("No GRIB files were downloaded to inspect dates.")
    # Placeholder: use file timestamps until wgrib2 integration.
    times = [datetime.fromtimestamp(path.stat().st_mtime) for path in grib_files]
    return min(times), max(times)


def prep_stage(config_path: Path) -> None:
    config = load_config(config_path)
    listing, is_complete = pick_files_for_latest_cycle(
        config.gfs.base_url,
        config.gfs.file_glob,
        prefer_latest=config.gfs.prefer_latest,
        required_count=config.gfs.require_complete_file_count,
    )
    if not is_complete and config.run.force_latest_gfs:
        raise RuntimeError("Latest GFS data is incomplete, and force_latest_gfs is enabled.")

    downloaded = download_files(
        config.gfs.base_url,
        listing.cycle_dir,
        listing.files,
        config.paths.data_dir,
    )

    start_date, end_date = _find_grib_dates(downloaded)
    update_dates(config.paths.wps_namelist, start_date, end_date)
    update_dates(config.paths.wrf_namelist, start_date, end_date)

    run_command([str(config.paths.geogrid_exe)], cwd=config.paths.wps_dir)
    run_command([str(config.paths.link_grib_script)], cwd=config.paths.wps_dir)
    run_command([str(config.paths.ungrib_exe)], cwd=config.paths.wps_dir)
    run_command([str(config.paths.metgrid_exe)], cwd=config.paths.wps_dir)


def compute_stage(config_path: Path) -> None:
    config = load_config(config_path)
    physics_runs = expand_all([(entry.name, entry.parameters) for entry in config.physics])
    runs = physics_runs[: config.run.max_runs]

    for physics in runs:
        update_physics(config.paths.wrf_namelist, physics.parameters)
        run_command([str(config.paths.mpirun_path), str(config.paths.real_exe)], cwd=config.paths.wrf_dir)
        run_command([str(config.paths.mpirun_path), str(config.paths.wrf_exe)], cwd=config.paths.wrf_dir)

        for script in sorted(config.paths.scripts_dir.glob("*.ncl")):
            run_command([str(config.paths.ncl_path), str(script)], cwd=config.paths.wrf_dir)

        for image_dir in sorted(config.paths.output_dir.glob("*.png")):
            output_mp4 = config.paths.output_dir / f"{physics.name}.mp4"
            run_command(
                [
                    str(config.paths.ffmpeg_path),
                    "-y",
                    "-framerate",
                    "10",
                    "-pattern_type",
                    "glob",
                    "-i",
                    str(image_dir / "*.png"),
                    str(output_mp4),
                ],
                cwd=config.paths.output_dir,
            )


def main() -> None:
    parser = argparse.ArgumentParser(description="Run WRF processing steps.")
    parser.add_argument("config", type=Path, help="Path to JSON config.")
    parser.add_argument("--prep", action="store_true", help="Run the data prep stage.")
    parser.add_argument("--compute", action="store_true", help="Run the compute stage.")
    args = parser.parse_args()

    if not args.prep and not args.compute:
        raise SystemExit("Specify --prep and/or --compute")

    if args.prep:
        prep_stage(args.config)
    if args.compute:
        compute_stage(args.config)


if __name__ == "__main__":
    main()
