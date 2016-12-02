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
        public int MpPhysics { get; set; }

        public int RaLwPhysics { get; set; }

        public int RaSwPhysics { get; set; }

        public int Radt { get; set; }

        public int SfSfClayPhysics { get; set; }

        public int SfSurfacePhysics { get; set; }

        public int BlPblPhysics { get; set; }

        public int Bldt { get; set; }

        public int CuPhysics { get; set; }

        public int Cudt { get; set; }

        public int IsFflx { get; set; }

        public int IfSnow { get; set; }

        public int ICloud { get; set; }

        public int SurfaceInputSource { get; set; }

        public int NumSoilLayers { get; set; }

        public int SfUrbanPhysics { get; set; }
    }
}
