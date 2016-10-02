using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface IEnvironment
    {
        void SetEnvironmentVariable(string name, string value); 
    }
}
