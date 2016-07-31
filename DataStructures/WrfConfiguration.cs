using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class WrfConfiguration
    {
        public string DataDirectory { get; set; }
        public string WPSDirectory { get; set; }
        public string WRFDirectory { get; set; }
        public string WGRIB2FilePath { get; set; }
        public string GFSProductUrl { get; set; }
        public string WRFNamelist { get; set; }
        public string WPSNamelist { get; set; }
    }
}
