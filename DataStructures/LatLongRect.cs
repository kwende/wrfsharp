using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class LatLongRect
    {
        public float UpperLeftLat { get; set; }
        public float UpperLeftLong { get; set; }
        public float LowerRightLat { get; set; }
        public float LowerRightLong { get; set; }
    }
}
