using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.NetCDF
{
    public static class NetCDFReaderHelper
    {
        public static PhysicsConfigurationProcessed ReadPhysicsSettings(INetCDFReader reader)
        {
            PhysicsConfigurationProcessed ret = new PhysicsConfigurationProcessed();

            ret.Bldt = reader.ReadFloatAttribute("BLDT");
            ret.BlPblPhysics = reader.ReadFloatAttribute("BL_PBL_PHYSICS");
            ret.Cudt = reader.ReadFloatAttribute("CUDT");
            ret.CuPhysics = reader.ReadFloatAttribute("CU_PHYSICS");
            ret.ICloud = 0; // ?
            ret.IfSnow = 0; // ?
            ret.IsFflx = reader.ReadFloatAttribute("ISFFLX");
            ret.MpPhysics = reader.ReadFloatAttribute("MP_PHYSICS");
            ret.NumSoilLayers = 0; // ?
            ret.Radt = reader.ReadFloatAttribute("RADT");
            ret.RaLwPhysics = reader.ReadFloatAttribute("RA_LW_PHYSICS");
            ret.RaSwPhysics = reader.ReadFloatAttribute("RA_SW_PHYSICS");
            ret.SfSfClayPhysics = reader.ReadFloatAttribute("SF_SFCLAY_PHYSICS");
            ret.SfSurfacePhysics = reader.ReadFloatAttribute("SF_SURFACE_PHYSICS");
            ret.SfUrbanPhysics = reader.ReadFloatAttribute("SF_URBAN_PHYSICS");
            ret.SurfaceInputSource = reader.ReadFloatAttribute("SURFACE_INPUT_SOURCE");

            return ret;
        }

        public static void ReadGridDimensions(INetCDFReader reader, out float westEastGridDimension,
            out float southNorthGridDimension, out float bottomTopGridDimension)
        {
            westEastGridDimension = reader.ReadFloatAttribute("WEST-EAST_GRID_DIMENSION");
            southNorthGridDimension = reader.ReadFloatAttribute("SOUTH-NORTH_GRID_DIMENSION");
            bottomTopGridDimension = reader.ReadFloatAttribute("BOTTOM-TOP_GRID_DIMENSION");
        }

        public static DateTime GetSimulationDate(INetCDFReader reader)
        {
            //Example: 2016-11-12_12:00:00
            CultureInfo provider = CultureInfo.InvariantCulture;
            string dateTimeAsString = reader.ReadStringAttribute("SIMULATION_START_DATE");
            return DateTime.ParseExact(dateTimeAsString, "yyyy-MM-dd_HH:mm:ss", provider);
        }


        public static VariableRecord[] GetVariableRecords(INetCDFReader reader, float minLat,
            float maxLat, float minLon, float maxLon)
        {
            float[][][] lats = reader.Read3DFloatArray("XLAT");
            float[][][] lons = reader.Read3DFloatArray("XLONG");
            float[][][] rainc = reader.Read3DFloatArray("RAINC");
            float[][][] rainnc = reader.Read3DFloatArray("RAINNC");
            float[][][] tc2 = reader.Read3DFloatArray("T2");
            float[][][] snowh = reader.Read3DFloatArray("SNOWH");
            float[][][] surfacePressure = reader.Read3DFloatArray("PSFC");
            float[][][] surfaceSkinTemp = reader.Read3DFloatArray("TSK");
            float[][][] u10 = reader.Read3DFloatArray("U10");
            float[][][] v10 = reader.Read3DFloatArray("V10");
            float[][][] cloudFraction = reader.Read3DFloatArray("CLDFRA"); 

            DateTime[] dates = reader.ReadDateArray("Times");

            //tc2 = tc2-273.16                  ; T2 in C
            //tf2 = 1.8*tc2+32.                    ; Turn temperature into Fahrenheit

            List<VariableRecord> ret = new List<VariableRecord>();

            for (int t = 0; t < dates.Length; t++)
            {
                DateTime date = dates[t];
                for (int y = 0; y < lats.Length; y++)
                {
                    for (int x = 0; x < lons.Length; x++)
                    {
                        float lat = lats[t][y][x];
                        float lon = lons[t][y][x];

                        if (lat > minLat && lat < maxLat &&
                            lon < maxLon && lon > minLon)
                        {
                            float tempInK = tc2[t][y][x];
                            float tempInC = tempInK - 273.16f;
                            float tempInF = 1.8f * tempInC + 32;

                            float skinTempInK = surfaceSkinTemp[t][y][x];
                            float skinTempInC = skinTempInK - 273.16f;
                            float skinTempInF = 1.8f * skinTempInC + 32;

                            VariableRecord rec = new VariableRecord
                            {
                                DateTime = date,
                                Lat = lat,
                                Lon = lon,
                                PrecipInMM = rainc[t][y][x] + rainnc[t][y][x],
                                TempInF = tempInF,
                                SnowDepthInM = snowh[t][y][x],
                                SurfacePressure = surfacePressure[t][y][x],
                                SurfaceSkinTemperature = skinTempInF,
                                UWind = u10[t][y][x],
                                VWind = v10[t][y][x],
                                //CloudFraction = cloudFraction[t][y][x],
                            };

                            ret.Add(rec);
                        }
                    }
                }
            }

            return ret.ToArray();
        }
    }
}
