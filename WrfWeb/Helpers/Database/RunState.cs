using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Helpers.Database
{
    public class RunState
    {
        public string StateText { get; set; }
        public DateTime LastCheckinDate { get; set; }
    }
}
