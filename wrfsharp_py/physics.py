from __future__ import annotations

from dataclasses import dataclass
from itertools import product
from typing import Any, Dict, Iterable, List, Tuple


@dataclass(frozen=True)
class PhysicsRun:
    name: str
    parameters: Dict[str, int]


def _expand_value(value: Any) -> List[int]:
    if isinstance(value, int):
        return [value]
    if isinstance(value, str):
        parts = [part.strip() for part in value.split(",") if part.strip()]
        return [int(part) for part in parts]
    if isinstance(value, Iterable):
        return [int(part) for part in value]
    raise ValueError(f"Unsupported physics parameter value: {value}")


def expand_physics_config(name: str, parameters: Dict[str, Any]) -> List[PhysicsRun]:
    keys = list(parameters.keys())
    values: List[List[int]] = [_expand_value(parameters[key]) for key in keys]
    runs: List[PhysicsRun] = []
    for combo in product(*values):
        run_params = {key: int(value) for key, value in zip(keys, combo)}
        runs.append(PhysicsRun(name=name, parameters=run_params))
    return runs


def expand_all(configs: Iterable[Tuple[str, Dict[str, Any]]]) -> List[PhysicsRun]:
    expanded: List[PhysicsRun] = []
    for name, params in configs:
        expanded.extend(expand_physics_config(name, params))
    return expanded
