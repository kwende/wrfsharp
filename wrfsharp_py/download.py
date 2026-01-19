from __future__ import annotations

import re
import urllib.request
from dataclasses import dataclass
from pathlib import Path
from typing import Iterable, List, Tuple


@dataclass(frozen=True)
class GfsListing:
    cycle_dir: str
    files: List[str]


def _read_url(url: str) -> str:
    with urllib.request.urlopen(url) as response:
        return response.read().decode("utf-8", errors="replace")


def _find_dirs(page: str) -> List[str]:
    return sorted(set(re.findall(r"gfs\.\d{8}/\d{2}/", page)))


def _find_files(page: str, pattern: str) -> List[str]:
    regex = re.compile(pattern)
    return sorted(set(match.group(0) for match in regex.finditer(page)))


def list_latest_gfs(
    base_url: str,
    file_pattern: str,
    cycle_subdir: str,
    prefer_latest: bool = True,
) -> GfsListing:
    page = _read_url(base_url)
    dirs = _find_dirs(page)
    if not dirs:
        raise RuntimeError(f"No GFS directories found at {base_url}")

    cycle_dir = dirs[-1] if prefer_latest else dirs[0]
    cycle_url = f"{base_url.rstrip('/')}/{cycle_dir}{cycle_subdir.strip('/')}/"
    cycle_page = _read_url(cycle_url)
    files = _find_files(cycle_page, file_pattern)
    if not files:
        raise RuntimeError(f"No files matching {file_pattern} found at {cycle_url}")
    return GfsListing(cycle_dir=cycle_dir, files=files)


def download_files(
    base_url: str,
    cycle_dir: str,
    cycle_subdir: str,
    files: Iterable[str],
    target_dir: Path,
) -> List[Path]:
    target_dir.mkdir(parents=True, exist_ok=True)
    downloaded: List[Path] = []
    for filename in files:
        url = f"{base_url.rstrip('/')}/{cycle_dir}{cycle_subdir.strip('/')}/{filename}"
        destination = target_dir / filename
        if destination.exists():
            downloaded.append(destination)
            continue
        with urllib.request.urlopen(url) as response:
            destination.write_bytes(response.read())
        downloaded.append(destination)
    return downloaded


def pick_files_for_latest_cycle(
    base_url: str,
    file_pattern: str,
    cycle_subdir: str,
    prefer_latest: bool,
    required_count: int | None,
) -> Tuple[GfsListing, bool]:
    listing = list_latest_gfs(base_url, file_pattern, cycle_subdir, prefer_latest=True)
    is_complete = required_count is None or len(listing.files) >= required_count

    if is_complete or prefer_latest:
        return listing, is_complete

    fallback = list_latest_gfs(base_url, file_pattern, cycle_subdir, prefer_latest=False)
    fallback_complete = required_count is None or len(fallback.files) >= required_count
    return fallback, fallback_complete
