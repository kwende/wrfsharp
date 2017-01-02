using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Helpers.Database
{
    public class PrecipSimulationResults
    {
        public DateTime SimulationStartDate { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<float[]> RunRecords { get; set; }
        public List<string> RunIds { get; set; }
    }
}
