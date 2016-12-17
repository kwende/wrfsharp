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
            ucar.ma2.Array array1 = variable.read();

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

        public float[][][] Read3DFloatArray(string name)
        {
            Variable var = _file.findVariable(name);
            ucar.ma2.Array arr = var.read();

            int[] shape = arr.getShape();

            float[][][] ret = new float[shape[0]][][];
            
            for(int t=0,i=0;t< shape[0];t++)
            {
                ret[t] = new float[shape[1]][];
                for (int y = 0; y < shape[1]; y++)
                {
                    ret[t][y] = new float[shape[2]];
                    for (int x = 0; x < shape[2]; x++, i++)
                    {
                        ret[t][y][x] = arr.getFloat(i);
                    }
                }
            }

            return ret;
        }

        public DateTime[] ReadDateArray(string name)
        {
            string[] dateStrings = ReadStringArray(name);

            DateTime[] ret = new DateTime[dateStrings.Length]; 
            for(int c=0;c<ret.Length;c++)
            {
                ret[c] = DateTime.ParseExact(dateStrings[c], "yyy-MM-dd_hh:mm:ss",
                CultureInfo.InvariantCulture);
            }

            return ret; 
        }
    }
}
