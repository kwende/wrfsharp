﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Models
{
    public class IndexModel
    {
        public List<List<object>> PrecipSummary { get; set; }
        public List<List<object>> TempSummary { get; set; }
        public DateTime SimulationStartDate { get; set; }
    }
}
