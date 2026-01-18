from __future__ import annotations

import json
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Dict, List, Mapping, Optional


@dataclass
class GfsDownloadConfig:
    base_url: str
    file_glob: str
    require_complete_file_count: Optional[int] = None
    prefer_latest: bool = True


@dataclass
class PathsConfig:
    data_dir: Path
    wps_dir: Path
    wrf_dir: Path
    wps_namelist: Path
    wrf_namelist: Path
    scripts_dir: Path
    output_dir: Path
    geogrid_exe: Path
    link_grib_script: Path
    ungrib_exe: Path
    metgrid_exe: Path
    wgrib2_exe: Path
    mpirun_path: Path
    real_exe: Path
    wrf_exe: Path
    ncl_path: Path
    ffmpeg_path: Path


@dataclass
class RegionConfig:
    upper_left_latlon: str
    lower_right_latlon: str


@dataclass
class RunConfig:
    max_runs: int
    force_latest_gfs: bool


@dataclass
class PhysicsConfig:
    name: str
    parameters: Dict[str, Any]


@dataclass
class WrfConfig:
    paths: PathsConfig
    gfs: GfsDownloadConfig
    region: RegionConfig
    run: RunConfig
    physics: List[PhysicsConfig]


def _require(mapping: Mapping[str, Any], key: str) -> Any:
    if key not in mapping:
        raise KeyError(f"Missing required config key: {key}")
    return mapping[key]


def _as_path(value: str) -> Path:
    return Path(value).expanduser()


def load_config(config_path: Path) -> WrfConfig:
    payload = json.loads(config_path.read_text())

    gfs_payload = _require(payload, "gfs")
    paths_payload = _require(payload, "paths")
    region_payload = _require(payload, "region")
    run_payload = _require(payload, "run")

    gfs = GfsDownloadConfig(
        base_url=_require(gfs_payload, "base_url"),
        file_glob=_require(gfs_payload, "file_glob"),
        require_complete_file_count=gfs_payload.get("require_complete_file_count"),
        prefer_latest=gfs_payload.get("prefer_latest", True),
    )

    paths = PathsConfig(
        data_dir=_as_path(_require(paths_payload, "data_dir")),
        wps_dir=_as_path(_require(paths_payload, "wps_dir")),
        wrf_dir=_as_path(_require(paths_payload, "wrf_dir")),
        wps_namelist=_as_path(_require(paths_payload, "wps_namelist")),
        wrf_namelist=_as_path(_require(paths_payload, "wrf_namelist")),
        scripts_dir=_as_path(_require(paths_payload, "scripts_dir")),
        output_dir=_as_path(_require(paths_payload, "output_dir")),
        geogrid_exe=_as_path(_require(paths_payload, "geogrid_exe")),
        link_grib_script=_as_path(_require(paths_payload, "link_grib_script")),
        ungrib_exe=_as_path(_require(paths_payload, "ungrib_exe")),
        metgrid_exe=_as_path(_require(paths_payload, "metgrid_exe")),
        wgrib2_exe=_as_path(_require(paths_payload, "wgrib2_exe")),
        mpirun_path=_as_path(_require(paths_payload, "mpirun_path")),
        real_exe=_as_path(_require(paths_payload, "real_exe")),
        wrf_exe=_as_path(_require(paths_payload, "wrf_exe")),
        ncl_path=_as_path(_require(paths_payload, "ncl_path")),
        ffmpeg_path=_as_path(_require(paths_payload, "ffmpeg_path")),
    )

    region = RegionConfig(
        upper_left_latlon=_require(region_payload, "upper_left_latlon"),
        lower_right_latlon=_require(region_payload, "lower_right_latlon"),
    )

    run = RunConfig(
        max_runs=int(_require(run_payload, "max_runs")),
        force_latest_gfs=bool(run_payload.get("force_latest_gfs", True)),
    )

    physics_payload = payload.get("physics", [])
    physics = [
        PhysicsConfig(name=entry.get("name", f"physics_{idx}"), parameters=entry["parameters"])
        for idx, entry in enumerate(physics_payload)
    ]

    return WrfConfig(paths=paths, gfs=gfs, region=region, run=run, physics=physics)
