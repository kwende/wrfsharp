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
        [ConfigurationProperty("Name")]
        public string Name
        {
            get
            {
                return (string)this["Name"]; 
            }
            set
            {
                this["Name"] = value; 
            }
        }

        [ConfigurationProperty("mp_physics")]
        public int MpPhysics
        {
            get
            {
                return (int)this["mp_physics"]; 
            }
            set
            {
                this["mp_physics"] = value; 
            }
        }

        [ConfigurationProperty("ra_lw_physics")]
        public int RaLwPhysics
        {
            get
            {
                return (int)this["ra_lw_physics"];
            }
            set
            {
                this["ra_lw_physics"] = value;
            }
        }

        [ConfigurationProperty("ra_sw_physics")]
        public int RaSwPhysics
        {
            get
            {
                return (int)this["ra_sw_physics"];
            }
            set
            {
                this["ra_sw_physics"] = value;
            }
        }

        [ConfigurationProperty("radt")]
        public int Radt
        {
            get
            {
                return (int)this["radt"];
            }
            set
            {
                this["radt"] = value;
            }
        }

        [ConfigurationProperty("sf_sfclay_physics")]
        public int SfSfClayPhysics
        {
            get
            {
                return (int)this["sf_sfclay_physics"];
            }
            set
            {
                this["sf_sfclay_physics"] = value;
            }
        }

        [ConfigurationProperty("sf_surface_physics")]
        public int SfSurfacePhysics
        {
            get
            {
                return (int)this["sf_surface_physics"];
            }
            set
            {
                this["sf_surface_physics"] = value;
            }
        }

        [ConfigurationProperty("bl_pbl_physics")]
        public int BlPblPhysics
        {
            get
            {
                return (int)this["bl_pbl_physics"];
            }
            set
            {
                this["bl_pbl_physics"] = value;
            }
        }

        [ConfigurationProperty("bldt")]
        public int Bldt
        {
            get
            {
                return (int)this["bldt"];
            }
            set
            {
                this["bldt"] = value;
            }
        }

        [ConfigurationProperty("cu_physics")]
        public int CuPhysics
        {
            get
            {
                return (int)this["cu_physics"];
            }
            set
            {
                this["cu_physics"] = value;
            }
        }

        [ConfigurationProperty("cudt")]
        public int Cudt
        {
            get
            {
                return (int)this["cudt"];
            }
            set
            {
                this["cudt"] = value;
            }
        }

        [ConfigurationProperty("isfflx")]
        public int IsFflx
        {
            get
            {
                return (int)this["isfflx"];
            }
            set
            {
                this["isfflx"] = value;
            }
        }

        [ConfigurationProperty("ifsnow")]
        public int IfSnow
        {
            get
            {
                return (int)this["ifsnow"];
            }
            set
            {
                this["ifsnow"] = value;
            }
        }

        [ConfigurationProperty("icloud")]
        public int ICloud
        {
            get
            {
                return (int)this["icloud"];
            }
            set
            {
                this["icloud"] = value;
            }
        }

        [ConfigurationProperty("surface_input_source")]
        public int SurfaceInputSource
        {
            get
            {
                return (int)this["surface_input_source"];
            }
            set
            {
                this["surface_input_source"] = value;
            }
        }

        [ConfigurationProperty("num_soil_layers")]
        public int NumSoilLayers
        {
            get
            {
                return (int)this["num_soil_layers"];
            }
            set
            {
                this["num_soil_layers"] = value;
            }
        }

        [ConfigurationProperty("sf_urban_physics")]
        public int SfUrbanPhysics
        {
            get
            {
                return (int)this["sf_urban_physics"];
            }
            set
            {
                this["sf_urban_physics"] = value;
            }
        }
    }
}
