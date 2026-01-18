from __future__ import annotations

import argparse
from datetime import datetime
import re
from pathlib import Path
from typing import List

from wrfsharp_py.config import load_config
from wrfsharp_py.download import download_files, pick_files_for_latest_cycle
from wrfsharp_py.namelist import update_dates, update_physics
from wrfsharp_py.physics import expand_all
from wrfsharp_py.process import run_command, run_command_output


def _parse_wgrib2_date(output: str) -> datetime:
    match = re.search(r"d=(\d{10})", output)
    if not match:
        raise RuntimeError("Unable to parse date from wgrib2 output.")
    return datetime.strptime(match.group(1), "%Y%m%d%H")


def _find_grib_dates(grib_files: List[Path], wgrib2_path: Path) -> tuple[datetime, datetime]:
    if not grib_files:
        raise RuntimeError("No GRIB files were downloaded to inspect dates.")
    sorted_files = sorted(grib_files)
    first_output = run_command_output([str(wgrib2_path), "-s", str(sorted_files[0])])
    last_output = run_command_output([str(wgrib2_path), "-s", str(sorted_files[-1])])
    return _parse_wgrib2_date(first_output), _parse_wgrib2_date(last_output)


def prep_stage(config_path: Path) -> None:
    config = load_config(config_path)
    listing, is_complete = pick_files_for_latest_cycle(
        config.gfs.base_url,
        config.gfs.file_regex,
        config.gfs.cycle_subdir,
        prefer_latest=config.gfs.prefer_latest,
        required_count=config.gfs.require_complete_file_count,
    )
    if not is_complete and config.run.force_latest_gfs:
        raise RuntimeError("Latest GFS data is incomplete, and force_latest_gfs is enabled.")

    downloaded = download_files(
        config.gfs.base_url,
        listing.cycle_dir,
        config.gfs.cycle_subdir,
        listing.files,
        config.paths.data_dir,
    )

    start_date, end_date = _find_grib_dates(downloaded, config.paths.wgrib2_exe)
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
            pattern = str(config.paths.output_dir / f"{script.stem}_*.png")
            output_mp4 = config.paths.output_dir / f"{script.stem}_{physics.name}.mp4"
            run_command(
                [
                    str(config.paths.ffmpeg_path),
                    "-y",
                    "-framerate",
                    "10",
                    "-pattern_type",
                    "glob",
                    "-i",
                    pattern,
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
