using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;

namespace WrfSharp.Helpers.Namelists
{
    public static class NamelistParser
    {
        public static Namelist Parse(string namelistContent)
        {
            Namelist ret = new Namelist();
            ret.Sections = new List<NamelistSection>(); 

            StringReader reader = new StringReader(namelistContent);

            NamelistSection currentSection = null; 
            string line = null; 
            while((line = reader.ReadLine()) != null)
            {
                if(line.StartsWith("&"))
                {
                    // new section. 
                    currentSection = new NamelistSection();
                    currentSection.Name = line.Replace("&", "").Trim();
                    currentSection.Items = new List<NamelistItem>(); 

                    ret.Sections.Add(currentSection); 
                }
                else if(!line.StartsWith("/") && line.Length > 0 && currentSection != null)
                {
                    string[] bits = line.Split('=');
                    string name = bits[0].Trim();
                    string[] stringValues = bits[1].Trim().Split(',').Where(
                        n=>!string.IsNullOrEmpty(n)).ToArray();

                    List<object> values = new List<object>();

                    int result = 0; 
                    if(int.TryParse(stringValues[0], out result))
                    {
                        foreach(string stringValue in stringValues)
                        {
                            values.Add(int.Parse(stringValue)); 
                        }
                    }

                    currentSection.Items.Add(new NamelistItem
                    {
                        Name = name,
                        Values = values
                    });  
                }
            }

            return ret; 
        }
    }
}
