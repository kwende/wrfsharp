using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockWPSFileSystemWithMetEmFiles : IFileSystem
    {
        public List<string> SymLinksCreated = new List<string>(); 

        public void ChangeCurrentDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public void CreateSymLink(string source, string link)
        {
            SymLinksCreated.Add(link); 
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
            return File.ReadAllLines("TestFiles/ls_WPS.txt"); 
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
