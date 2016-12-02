using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class PhysicsConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }


        [ConfigurationProperty("mp_physics")]
        public string MpPhysics
        {
            get
            {
                return (string)this["mp_physics"]; 
            }
            set
            {
                this["mp_physics"] = value; 
            }
        }

        [ConfigurationProperty("ra_lw_physics")]
        public string RaLwPhysics
        {
            get
            {
                return (string)this["ra_lw_physics"];
            }
            set
            {
                this["ra_lw_physics"] = value;
            }
        }

        [ConfigurationProperty("ra_sw_physics")]
        public string RaSwPhysics
        {
            get
            {
                return (string)this["ra_sw_physics"];
            }
            set
            {
                this["ra_sw_physics"] = value;
            }
        }

        [ConfigurationProperty("radt")]
        public string Radt
        {
            get
            {
                return (string)this["radt"];
            }
            set
            {
                this["radt"] = value;
            }
        }

        [ConfigurationProperty("sf_sfclay_physics")]
        public string SfSfClayPhysics
        {
            get
            {
                return (string)this["sf_sfclay_physics"];
            }
            set
            {
                this["sf_sfclay_physics"] = value;
            }
        }

        [ConfigurationProperty("sf_surface_physics")]
        public string SfSurfacePhysics
        {
            get
            {
                return (string)this["sf_surface_physics"];
            }
            set
            {
                this["sf_surface_physics"] = value;
            }
        }

        [ConfigurationProperty("bl_pbl_physics")]
        public string BlPblPhysics
        {
            get
            {
                return (string)this["bl_pbl_physics"];
            }
            set
            {
                this["bl_pbl_physics"] = value;
            }
        }

        [ConfigurationProperty("bldt")]
        public string Bldt
        {
            get
            {
                return (string)this["bldt"];
            }
            set
            {
                this["bldt"] = value;
            }
        }

        [ConfigurationProperty("cu_physics")]
        public string CuPhysics
        {
            get
            {
                return (string)this["cu_physics"];
            }
            set
            {
                this["cu_physics"] = value;
            }
        }

        [ConfigurationProperty("cudt")]
        public string Cudt
        {
            get
            {
                return (string)this["cudt"];
            }
            set
            {
                this["cudt"] = value;
            }
        }

        [ConfigurationProperty("isfflx")]
        public string IsFflx
        {
            get
            {
                return (string)this["isfflx"];
            }
            set
            {
                this["isfflx"] = value;
            }
        }

        [ConfigurationProperty("ifsnow")]
        public string IfSnow
        {
            get
            {
                return (string)this["ifsnow"];
            }
            set
            {
                this["ifsnow"] = value;
            }
        }

        [ConfigurationProperty("icloud")]
        public string ICloud
        {
            get
            {
                return (string)this["icloud"];
            }
            set
            {
                this["icloud"] = value;
            }
        }

        [ConfigurationProperty("surface_input_source")]
        public string SurfaceInputSource
        {
            get
            {
                return (string)this["surface_input_source"];
            }
            set
            {
                this["surface_input_source"] = value;
            }
        }

        [ConfigurationProperty("num_soil_layers")]
        public string NumSoilLayers
        {
            get
            {
                return (string)this["num_soil_layers"];
            }
            set
            {
                this["num_soil_layers"] = value;
            }
        }

        [ConfigurationProperty("sf_urban_physics")]
        public string SfUrbanPhysics
        {
            get
            {
                return (string)this["sf_urban_physics"];
            }
            set
            {
                this["sf_urban_physics"] = value;
            }
        }
    }
}
