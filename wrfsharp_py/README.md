# WrfSharp Python Driver (prototype)

This folder contains a Python-first rewrite of the WRF pipeline so you can drive runs via JSON.
It mirrors the older C# flow but skips database output. The current focus is:

- Download latest GFS assets.
- Update WPS/WRF namelists.
- Run WPS and WRF executables.
- Execute NCL scripts and render PNG sequences into MP4s with ffmpeg.

## Quick start

```bash
python -m wrfsharp_py.driver configs/sample.json --prep --compute
```

## Notes

- `download.py` targets the NOAA NOMADS GFS directory. Adjust `gfs.base_url`, `gfs.cycle_subdir`,
  and `gfs.file_regex` to match the resolution/cycle you want.
- The namelist helpers perform string substitution. If your namelist formatting differs, you may
  need to tweak `namelist.py`.
- `driver.py` uses `wgrib2 -s` to parse start/end dates from the first and last GRIB files.
- For web visualization alternatives, consider generating NetCDF or GeoTIFF layers and serving
  them via raster tiles (e.g., `xarray` + `rasterio` + `rio-tiler`) instead of MP4.
