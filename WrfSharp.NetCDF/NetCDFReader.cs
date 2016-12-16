using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ucar.nc2;
using WrfSharp.DataStructures.Exceptions;
using WrfSharp.Interfaces;

namespace WrfSharp.NetCDF
{
    public class NetCDFReader : INetCDFReader
    {
        private NetcdfFile _file;

        public NetCDFReader(string fileName)
        {
            _file = NetcdfFile.open(fileName);
        }

        public float ReadFloatAttribute(string attributeName)
        {
            float ret = 0.0f; 
            ucar.nc2.Attribute attrib = _file.findGlobalAttribute(attributeName);
            if (attrib.getDataType().isIntegral())
            {
                ret = (attrib.getValue(0) as java.lang.Number).intValue();
            }
            else if(attrib.getDataType().isFloatingPoint())
            {
                ret = (attrib.getValue(0) as java.lang.Float).floatValue(); 
            }
            return ret; 
        }


        public DateTime ReadDateAttribute(string attributeName)
        {
            string dateAsString = ReadStringAttribute(attributeName);

            //2016-11-12_12:00:00
            return DateTime.ParseExact(dateAsString, "yyy-MM-dd_hh:mm:ss", 
                CultureInfo.InvariantCulture);
        }

        public string ReadStringAttribute(string attributeName)
        {
            ucar.nc2.Attribute attribute =
                _file.findGlobalAttribute("SIMULATION_START_DATE");
            return attribute.getValue(0) as string;
        }

        public string[] ReadStringArray(string variableName)
        {
            Variable variable = _file.findVariable(variableName);
             = variable.read();

            int[] shape = array1.getShape();
            string[] ret = new string[shape[0]];

            for (int c = 0; c < ret.Length; c++)
            {
                StringBuilder sb = new StringBuilder(ret[1]);
                for (int d = 0; d < shape[1]; d++)
                {
                    sb.Append(array1.getChar(d));
                }
                ret[c] = sb.ToString(); 
            }

            return ret; 
        }

        public void ReadVariable()
        {
            NetcdfFile file = NetcdfFile.open("wrfout.nc");
            Variable longVar = file.findVariable("XLONG");
            Variable latVar = file.findVariable("XLAT");
            Variable rainc = file.findVariable("RAINC");
            Variable rainnc = file.findVariable("RAINNC");


            ucar.ma2.Array longArray = longVar.read();
            ucar.ma2.Array latArray = latVar.read();
            ucar.ma2.Array raincArray = rainc.read();
            ucar.ma2.Array rainncArray = rainnc.read();

            int[] shape = longArray.getShape();

            int count = 0;
            for (int t = 0, i = 0; t < shape[0]; t++)
            {
                Console.WriteLine("========================");

                for (int y = 0; y < shape[1]; y++)
                {
                    for (int x = 0; x < shape[2]; x++, i++)
                    {
                        float lon = longArray.getFloat(i);
                        float lat = latArray.getFloat(i);

                        if (Math.Abs(lat) > 39.9f && Math.Abs(lat) < 41.7 &&
                            Math.Abs(lon) > 95.3 && Math.Abs(lon) < 97.8)
                        {
                            float raincAmount = raincArray.getFloat(i);
                            float rainncAmount = rainncArray.getFloat(i);
                            float total = raincAmount + rainncAmount;

                            //if(total > 0)
                            {
                                //Console.WriteLine($"{t}x{lat}x{lon}: {total}");
                                count++;
                            }
                        }
                    }
                }
            }
        }

    }
}
