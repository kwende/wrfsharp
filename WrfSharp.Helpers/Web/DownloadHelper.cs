using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Helpers.Web
{
    public static class DownloadHelper
    {
        public static string DownloadString(string url)
        {
            WebClient wc = new WebClient();
            return wc.DownloadString(url); 
        }
    }
}
