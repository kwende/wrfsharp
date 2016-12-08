using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface INetCDFReader
    {
        int ReadIntAttribute(string attributeName);
        string[] ReadStringArray(string arrayVariableName);
        string ReadStringAttribute(string v);
    }
}
