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
        public float Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}
