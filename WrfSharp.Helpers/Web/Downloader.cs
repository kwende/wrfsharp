using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.Web
{
    public class Downloader : IDownloader
    {
        public void DownloadFile(string url, string destinationFile)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(url, destinationFile); 
        }

        public string DownloadString(string url)
        {
            WebClient wc = new WebClient();
            return wc.DownloadString(url); 
        }
    }
}
