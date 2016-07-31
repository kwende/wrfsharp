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
        public static string ParseToString(Namelist namelist)
        {
            StringBuilder sb = new StringBuilder(1024 * 2); 

            foreach(NamelistSection section in namelist.Sections)
            {
                sb.AppendLine($"&{section.Name}"); 

                foreach(NamelistItem item in section.Items)
                {
                    if(item.Values[0].GetType() == typeof(bool))
                    {
                        sb.Append($"{item.Name} = ");

                        foreach(bool val in item.Values)
                        {
                            sb.Append($".{val.ToString().ToLower()}.,"); 
                        }

                        sb.AppendLine(); 
                    }
                    else if(item.Values[0].GetType() ==typeof(string))
                    {
                        if(!item.Values[0].ToString().StartsWith("."))
                        {
                            for(int c=0;c<item.Values.Count;c++)
                            {
                                item.Values[c] = $"'{item.Values[c]}'"; 
                            }
                        }
                        sb.AppendLine($"{item.Name} = {string.Join(",", item.Values)}");
                    }
                    else
                    {
                        foreach(object val in item.Values)
                        {
                            sb.AppendLine($"{item.Name} = {string.Join(",", item.Values)}");
                        }
                    }
                }

                sb.AppendLine("/" + Environment.NewLine); 
            }

            return sb.ToString(); 
        }

        public static Namelist ParseFromString(string namelistContent)
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
                    double dResult = 0; 
                    if(int.TryParse(stringValues[0], out result))
                    {
                        foreach(string stringValue in stringValues)
                        {
                            values.Add(int.Parse(stringValue)); 
                        }
                    }
                    else if(double.TryParse(stringValues[0], out dResult))
                    {
                        foreach (string stringValue in stringValues)
                        {
                            values.Add(double.Parse(stringValue));
                        }
                    }
                    else if(stringValues[0] == ".true." ||
                        stringValues[0] == ".false.")
                    {
                        foreach (string stringValue in stringValues)
                        {
                            if(stringValue == ".true.")
                            {
                                values.Add(true);
                            }
                            else if (stringValue == ".false.")
                            {
                                values.Add(false);
                            }
                        }
                    }
                    else if(stringValues[0].StartsWith("\'"))
                    {
                        foreach (string stringValue in stringValues)
                        {
                            values.Add(stringValue.Replace("\'", "")); 
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
