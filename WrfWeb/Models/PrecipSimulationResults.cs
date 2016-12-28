using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Models
{
    public class PrecipSimulationResults
    {
        public DateTime SimulationStartDate { get; set; }
        List<DateTime> Dates { get; set; }
        List<float[]> RunRecords { get; set; }
        List<string> RunIds { get; set; }
    }
}
