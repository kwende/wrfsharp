using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class Namelist
    {
        public List<NamelistSection> Sections { get; set; }
        
        public NamelistSection this[string name]
        {
            get
            {
                return Sections.Where(n => n.Name == name).First(); 
            }
        }
    }
}
