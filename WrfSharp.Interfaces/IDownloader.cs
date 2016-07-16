using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface IDownloader
    {
        string DownloadString(string url);
        void DownloadFile(string url, string destinationFile); 
    }
}
