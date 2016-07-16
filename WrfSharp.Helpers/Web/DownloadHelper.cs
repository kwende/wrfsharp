using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.Web
{
    public static class DownloadHelper
    {
        public static string DownloadString(string url, IDownloader downloader)
        {
            return downloader.DownloadString(url);  
        }

        public static string DownloadFile(string url, string destinationDirectory, IDownloader downloader)
        {
            url = url.Replace("\\", "/"); 
            int lastSlashIndex = url.LastIndexOf("/");
            string fullPath = Path.Combine(destinationDirectory, url.Substring(lastSlashIndex + 1)); 

            downloader.DownloadFile(url, fullPath);

            return fullPath; 
        }
    }
}
