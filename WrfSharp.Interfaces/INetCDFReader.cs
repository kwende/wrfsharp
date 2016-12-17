using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface INetCDFReader
    {
        float ReadFloatAttribute(string attributeName);
        string[] ReadStringArray(string arrayVariableName);
        string ReadStringAttribute(string attributeName);
        DateTime ReadDateAttribute(string attributeName);
        float[][][] Read3DFloatArray(string name);
        DateTime[] ReadDateArray(string name); 
    }
}
