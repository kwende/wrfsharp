using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    [ConfigurationCollection(typeof(PhysicsConfiguration), AddItemName="physicsConfiguration")]
    public class PhysicsConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PhysicsConfiguration(); 
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PhysicsConfiguration)element).Name; 
        }
    }
}
