using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.DataStructures.Exceptions
{
    public class SaveVariableRecordException : Exception
    {
        public SaveVariableRecordException(string message, Exception e) : base(message, e)
        {

        }
    }
}
