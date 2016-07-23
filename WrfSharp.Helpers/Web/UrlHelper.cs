using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Helpers.Web
{
    public static class UrlHelper
    {
        public static string Join(string firstHalf, string secondHalf)
        {
            firstHalf = firstHalf.Replace("\\", "/");
            secondHalf = secondHalf.Replace("\\", "/");

            if (!firstHalf.EndsWith("/") && !secondHalf.StartsWith("/"))
                firstHalf += "/";
            else if (firstHalf.EndsWith("/") && secondHalf.StartsWith("/"))
                firstHalf = firstHalf.Substring(0, firstHalf.Length - 1); 

            return firstHalf + secondHalf; 
        }
    }
}
