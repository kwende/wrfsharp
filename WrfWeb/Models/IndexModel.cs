using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Models
{
    public class IndexModel
    {
        public string CurrentRunState { get; set; }
        public DateTime LastCheckinDate { get; set; }
        public List<string> RunIds { get; set; }
        public List<List<object>> PrecipSummary { get; set; }
        public List<List<object>> TempSummary { get; set; }
        public List<List<object>> SnowDepths { get; set; }
        public List<List<object>> WindSpeeds { get; set; }
        public List<List<object>> SurfacePressures { get; set; }
        public DateTime SimulationStartDate { get; set; }
    }
}
