using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WrfSharp.Helpers.Web
{
    public static class PageParsingHelper
    {
        private const string GFSStartString = ">gfs.";
        private const string GFSEndString = "/"; 

        public static string FindDirectoryNameForSecondToLastGFSEntry(string pageContent)
        {
            int lastOccurenceIndex = pageContent.LastIndexOf(GFSStartString) + 1;
            int secondToLastOccurenceIndex = pageContent.LastIndexOf(GFSStartString, lastOccurenceIndex - 1) + 1; 

            int endIndex = pageContent.IndexOf(GFSEndString, secondToLastOccurenceIndex);

            return pageContent.Substring(secondToLastOccurenceIndex, endIndex - secondToLastOccurenceIndex); 
        }

        public static List<string> FindAllGFSOneDegreePGRB2Files(string pageContent)
        {
            MatchCollection matches = 
                Regex.Matches(pageContent, ">gfs\\.t[0-9].z\\.pgrb2\\.1p00\\.f[0-9][0-9][0-9]<");

            List<string> ret = new List<string>(); 
            foreach(Match match in matches)
            {
                ret.Add(match.Value.Replace("<","").Replace(">", "")); 
            }

            return ret; 
        }
    }
}
