using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParameterReader
{
    class Program
    {
        static void HandleType(HtmlNode node, string name)
        {
            if(name.Contains("_"))
            {
                Console.WriteLine(name);

                HtmlNode tableNode = node.ParentNode.ParentNode; 
            }
        }

        static void Main(string[] args)
        {
            //http://www2.mmm.ucar.edu/wrf/users/phys_references.html

            WebClient wc = new WebClient();
            string page = wc.DownloadString("http://www2.mmm.ucar.edu/wrf/users/phys_references.html");

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            // first set. 
            HtmlNodeCollection nodes = 
                doc.DocumentNode.SelectNodes("//table/caption/p[@class='caption']");

            foreach(HtmlNode node in nodes)
            {
                string innerText = node.InnerText;

                int numberOfParen = innerText.Count(n => n == '('); 

                if(numberOfParen == 1)
                {
                    int firstParenIndex = innerText.IndexOf("(");
                    int secondParenIndex = innerText.IndexOf(")");

                    string variableType = innerText.Substring(
                        firstParenIndex + 1, secondParenIndex - 1 - firstParenIndex);

                    HandleType(node, variableType); 
                }
                else if(numberOfParen == 2)
                {
                    int firstParenIndex = innerText.IndexOf("(");
                    int secondParenIndex = innerText.IndexOf(")", firstParenIndex);

                    string variableType = innerText.Substring(
                        firstParenIndex + 1, secondParenIndex - 1 - firstParenIndex);

                    HandleType(node, variableType);

                    firstParenIndex = innerText.IndexOf("(", secondParenIndex);
                    secondParenIndex = innerText.IndexOf(")", firstParenIndex);

                    variableType = innerText.Substring(
                        firstParenIndex + 1, secondParenIndex - 1 - firstParenIndex);

                    HandleType(node, variableType);
                }
            }

            Console.ReadLine(); 
        }
    }
}
