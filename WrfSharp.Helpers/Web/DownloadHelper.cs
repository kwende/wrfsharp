using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
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
            string fullPath = Path.Combine(
                destinationDirectory, url.Substring(lastSlashIndex + 1)).Replace("\\", "/"); 

            downloader.DownloadFile(url, fullPath);

            return fullPath; 
        }

        public static List<string> DownloadGFSProductsToDataDirectory(string productDirectoryUrl, 
            List<string> products, WrfConfiguration config, IDownloader iDownloader, ILogger logger)
        {
            List<string> localPaths = new List<string>(); 

            foreach(string product in products)
            {
                string productUrl = UrlHelper.Join(productDirectoryUrl, product);
                string localPath = 
                    DownloadHelper.DownloadFile(productUrl, config.DataDirectory, iDownloader);
                if(logger!=null)
                {
                    logger.LogLine($"\t...downloaded {localPath}");
                }
                localPaths.Add(localPath); 
            }

            return localPaths; 
        }
    }
}
