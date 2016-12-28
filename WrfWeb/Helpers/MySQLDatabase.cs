using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Helpers
{
    public class MySQLDatabase
    {
        private string _connectionString; 

        public MySQLDatabase(string connectionString)
        {
            _connectionString = connectionString; 
        }

        public void Connect()
        {

        }
    }
}
