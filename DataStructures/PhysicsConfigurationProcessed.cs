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
        public float MpPhysics { get; set; }

        [ConfigurationProperty("ra_lw_physics")]
        public float RaLwPhysics { get; set; }

        [ConfigurationProperty("ra_sw_physics")]
        public float RaSwPhysics { get; set; }

        [ConfigurationProperty("radt")]
        public float Radt { get; set; }

        [ConfigurationProperty("sf_sfclay_physics")]
        public float SfSfClayPhysics { get; set; }

        [ConfigurationProperty("sf_surface_physics")]
        public float SfSurfacePhysics { get; set; }

        [ConfigurationProperty("bl_pbl_physics")]
        public float BlPblPhysics { get; set; }

        [ConfigurationProperty("bldt")]
        public float Bldt { get; set; }

        [ConfigurationProperty("cu_physics")]
        public float CuPhysics { get; set; }

        [ConfigurationProperty("cudt")]
        public float Cudt { get; set; }

        [ConfigurationProperty("isfflx")]
        public float IsFflx { get; set; }

        [ConfigurationProperty("ifsnow")]
        public float IfSnow { get; set; }

        [ConfigurationProperty("icloud")]
        public float ICloud { get; set; }

        [ConfigurationProperty("surface_input_source")]
        public float SurfaceInputSource { get; set; }

        [ConfigurationProperty("num_soil_layers")]
        public float NumSoilLayers { get; set; }

        [ConfigurationProperty("sf_urban_physics")]
        public float SfUrbanPhysics { get; set; }
    }
}
