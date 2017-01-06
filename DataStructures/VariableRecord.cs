using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class VariableRecord
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
        public float PrecipInMM { get; set; }
        public float TempInF { get; set; }
        public float SnowDepthInM { get; set; }
        public float SurfacePressure { get; set; }
        public float SurfaceSkinTemperature { get; set; }
        public float UWind { get; set; }
        public float VWind { get; set; }
        public float CloudFraction { get; set; }
        public DateTime DateTime { get; set; }
    }
}
