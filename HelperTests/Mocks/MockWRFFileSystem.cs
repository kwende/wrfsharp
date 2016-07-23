using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockWRFFileSystem : IFileSystem
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
            //{ "wrfout", "wrfrst", "met_em" };
            //{ ".mp4", ".png", };
            return new string[]
            {
                "wrfout_ereere",
                "wrf",
                "wrfrst",
                "wrfsrt",
                "met_em",
                "metmetmet",
                "test.mp4",
                "testmp4",
                "test.png",
                "testpng"
            }; 
        }
    }
}
