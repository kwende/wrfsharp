using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class PhysicsConfigurationProcessed
    {
        [ConfigurationProperty("mp_physics")]
        public int MpPhysics { get; set; }

        [ConfigurationProperty("ra_lw_physics")]
        public int RaLwPhysics { get; set; }

        [ConfigurationProperty("ra_sw_physics")]
        public int RaSwPhysics { get; set; }

        [ConfigurationProperty("radt")]
        public int Radt { get; set; }

        [ConfigurationProperty("sf_sfclay_physics")]
        public int SfSfClayPhysics { get; set; }

        [ConfigurationProperty("sf_surface_physics")]
        public int SfSurfacePhysics { get; set; }

        [ConfigurationProperty("bl_pbl_physics")]
        public int BlPblPhysics { get; set; }

        [ConfigurationProperty("bldt")]
        public int Bldt { get; set; }

        [ConfigurationProperty("cu_physics")]
        public int CuPhysics { get; set; }

        [ConfigurationProperty("cudt")]
        public int Cudt { get; set; }

        [ConfigurationProperty("isfflx")]
        public int IsFflx { get; set; }

        [ConfigurationProperty("ifsnow")]
        public int IfSnow { get; set; }

        [ConfigurationProperty("icloud")]
        public int ICloud { get; set; }

        [ConfigurationProperty("surface_input_source")]
        public int SurfaceInputSource { get; set; }

        [ConfigurationProperty("num_soil_layers")]
        public int NumSoilLayers { get; set; }

        [ConfigurationProperty("sf_urban_physics")]
        public int SfUrbanPhysics { get; set; }
    }
}
