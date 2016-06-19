using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class NamelistItem
    {
        public string Name { get; set; }
        public List<object> Values { get; set; }
    }
}
