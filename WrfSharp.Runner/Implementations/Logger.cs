using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace WrfSharp.Runner.Implementations
{
    public class Logger : ILogger
    {
        private string _logFilePath; 

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath; 
        }

        private void LogInner(string content)
        {
            if (!string.IsNullOrEmpty(_logFilePath))
            {
                File.AppendAllText(_logFilePath, content);
            }
            Console.Write(content);
        }

        public void LogLine(string content)
        {
            LogInner(DateTime.Now.ToString() + ": " + content + System.Environment.NewLine); 
        }

        public void Log(string content)
        {
            LogInner(content); 
        }
    }
}
