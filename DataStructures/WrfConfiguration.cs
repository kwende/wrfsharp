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
        public string GeogridFilePath { get; set; }
        public string LinkGribFilePath { get; set; }
        public string UngribFilePath { get; set; }
        public string MetgridFilePath { get; set; }
        public string CSHFilePath { get; set; }
        public string LinkGribCsh { get; set; }
        public string MpiRunPath { get; set; }
        public string WrfExecutablePath { get; set; }
        public string RealExecutablePath { get; set; }
        public string ScriptsDirectory { get; set; }
        public string NCLPath { get; set; }
    }
}
