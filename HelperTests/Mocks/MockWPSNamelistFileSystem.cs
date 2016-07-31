using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockWPSNamelistFileSystem : IFileSystem
    {
        private string _result; 

        public void CreateDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string file)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string directory)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilesInDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public string ReadFileContent(string path)
        {
            if(path == "getresult")
            {
                return _result; 
            }
            else
            {
                return File.ReadAllText("TestFiles/wrfnamelist.input");
            }
        }

        public void WriteFileContent(string path, string content)
        {
            _result = content; 
        }
    }
}
