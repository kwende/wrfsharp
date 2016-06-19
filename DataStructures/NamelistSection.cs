using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class NamelistSection
    {
        public string Name { get; set; }
        public List<NamelistItem> Items { get; set; }

        public NamelistItem this[string name]
        {
            get
            {
                return Items.Where(n => n.Name == name).First(); 
            }
        }
    }
}
