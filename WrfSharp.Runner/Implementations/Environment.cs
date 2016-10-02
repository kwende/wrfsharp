using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace WrfSharp.Runner.Implementations
{
    public class Environment : IEnvironment
    {
        public void SetEnvironmentVariable(string name, string value)
        {
            System.Environment.SetEnvironmentVariable(name, value, 
                EnvironmentVariableTarget.Machine); 
        }
    }
}
