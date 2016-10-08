using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockDownloader : INetwork
    {
        public void DownloadFile(string url, string destinationFile)
        {
            return; // do nothing. 
        }

        public string DownloadString(string url)
        {
            return "Fake"; 
        }
    }
}
