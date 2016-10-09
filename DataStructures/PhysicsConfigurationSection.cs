using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures
{
    public class PhysicsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("physicsConfigurations")]
        public PhysicsConfigurationCollection PhysicsConfigurations
        {
            get
            {
                return (PhysicsConfigurationCollection)this["physicsConfigurations"]; 
            }
            set
            {
                this["physicsConfigurations"] = value; 
            }
        }
    }
}
