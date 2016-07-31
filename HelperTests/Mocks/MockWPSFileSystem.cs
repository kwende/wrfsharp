using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockWPSFileSystem : IFileSystem
    {
        public void CreateDirectory(string dataDirectory)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string directory)
        {
            return; 
        }

        public void DeleteFile(string file)
        {
            return; 
        }

        public bool DirectoryExists(string dataDirectory)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilesInDirectory(string directory)
        {
            //"FILE:", "PFILE:", "GRIBFILE", "met_em"
            return new string[]
            {
                "FILE:test",
                "Googliebah",
                "PFILE:adfadfs",
                "POOP",
                "GRIBFILE_",
                "GRRR",
                "met_em",
                "fart_met_em"
            }; 
        }

        public string ReadFileContent(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteFileContent(string path, string content)
        {
            throw new NotImplementedException();
        }
    }
}
