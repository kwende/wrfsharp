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
    }
}
