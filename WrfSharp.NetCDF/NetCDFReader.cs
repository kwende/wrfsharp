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

        public int ReadIntAttribute(string attributeName)
        {
            ucar.nc2.Attribute attrib = _file.findGlobalAttribute(attributeName);
            if (!attrib.getDataType().isIntegral())
            {
                throw new BadNetCDFTypeException();
            }
            else
            {
                return (attrib.getValue(0) as java.lang.Number).intValue();
            }
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

    }
}
