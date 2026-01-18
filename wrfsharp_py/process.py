from __future__ import annotations

import subprocess
from pathlib import Path
from typing import Iterable, Optional


def run_command(command: Iterable[str], cwd: Optional[Path] = None) -> None:
    completed = subprocess.run(list(command), cwd=str(cwd) if cwd else None, check=False)
    if completed.returncode != 0:
        raise RuntimeError(f"Command failed: {' '.join(command)}")
