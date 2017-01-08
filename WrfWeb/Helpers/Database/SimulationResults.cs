using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Helpers.Database
{
    public class SimulationResults
    {
        public DateTime SimulationStartDate { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<float[]> PrecipRecords { get; set; }
        public List<float[]> TempRecords { get; set; }
        public List<float[]> SnowDepths { get; set; }
        public List<float[]> WindSpeeds { get; set; }
        public List<float[]> SurfacePressure { get; set; }
        public List<string> RunIds { get; set; }
    }
}
