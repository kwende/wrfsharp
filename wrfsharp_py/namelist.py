from __future__ import annotations

import re
from datetime import datetime
from pathlib import Path
from typing import Dict


DATE_FIELDS = ("start_date", "end_date")


def _format_date(value: datetime) -> str:
    return value.strftime("%Y-%m-%d_%H:%M:%S")


def update_dates(namelist_path: Path, start_date: datetime, end_date: datetime) -> None:
    content = namelist_path.read_text()
    date_values = {
        "start_date": _format_date(start_date),
        "end_date": _format_date(end_date),
    }
    for key, value in date_values.items():
        pattern = rf"({key}\s*=\s*)([^\n,]+)"
        content = re.sub(pattern, rf"\1'{value}'", content)
    namelist_path.write_text(content)


def update_physics(namelist_path: Path, parameters: Dict[str, int]) -> None:
    content = namelist_path.read_text()
    for key, value in parameters.items():
        pattern = rf"({re.escape(key)}\s*=\s*)([^\n,]+)"
        content = re.sub(pattern, rf"\1{value}", content)
    namelist_path.write_text(content)
