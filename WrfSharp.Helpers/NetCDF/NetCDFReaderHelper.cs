﻿using System;
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

            ret.Bldt = reader.ReadIntAttribute("BLDT");
            ret.BlPblPhysics = reader.ReadIntAttribute("BL_PBL_PHYSICS");
            ret.Cudt = reader.ReadIntAttribute("CUDT");
            ret.CuPhysics = reader.ReadIntAttribute("CU_PHYSICS");
            ret.ICloud = 0; // ?
            ret.IfSnow = 0; // ?
            ret.IsFflx = reader.ReadIntAttribute("ISFFLX");
            ret.MpPhysics = reader.ReadIntAttribute("MP_PHYSICS");
            ret.NumSoilLayers = 0; // ?
            ret.Radt = reader.ReadIntAttribute("RADT");
            ret.RaLwPhysics = reader.ReadIntAttribute("RA_LW_PHYSICS");
            ret.RaSwPhysics = reader.ReadIntAttribute("RA_SW_PHYSICS");
            ret.SfSfClayPhysics = reader.ReadIntAttribute("SF_SFCLAY_PHYSICS");
            ret.SfSurfacePhysics = reader.ReadIntAttribute("SF_SURFACE_PHYSICS");
            ret.SfUrbanPhysics = reader.ReadIntAttribute("SF_URBAN_PHYSICS");
            ret.SurfaceInputSource = reader.ReadIntAttribute("SURFACE_INPUT_SOURCE"); 

            return ret; 
        }

        public static void ReadGridDimensions(INetCDFReader reader, out int westEastGridDimension, 
            out int southNorthGridDimension, out int bottomTopGridDimension)
        {
            westEastGridDimension = reader.ReadIntAttribute("WEST-EAST_GRID_DIMENSION");
            southNorthGridDimension = reader.ReadIntAttribute("SOUTH-NORTH_GRID_DIMENSION");
            bottomTopGridDimension = reader.ReadIntAttribute("BOTTOM-TOP_GRID_DIMENSION"); 
        }

        public static DateTime GetSimulationDate(INetCDFReader reader)
        {
            //Example: 2016-11-12_12:00:00
            CultureInfo provider = CultureInfo.InvariantCulture;
            string dateTimeAsString = reader.ReadStringAttribute("SIMULATION_START_DATE");
            return DateTime.ParseExact(dateTimeAsString, "yyy-MM-dd_hh:mm:ss", provider); 
        }
    }
}