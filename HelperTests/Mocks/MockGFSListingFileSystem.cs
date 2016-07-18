using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockGFSListingFileSystem : IFileSystem
    {
        public void DeleteDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string file)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilesInDirectory(string directory)
        {
            return File.ReadAllText("TestFiles/GfsFileListings.txt").Split('\n').ToArray();
        }
    }
}
