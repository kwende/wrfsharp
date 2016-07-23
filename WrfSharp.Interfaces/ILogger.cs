using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface ILogger
    {
        void Log(string content);
        void LogLine(string content);
    }
}
