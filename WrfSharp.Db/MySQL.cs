using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Db
{
    public class MySQL : IDatabase
    {
        private MySqlConnection _connection; 
        public string Server { get; private set; }
        public string Database { get; private set; }

        public static MySQL OpenConnection(string server, string userName, 
            string password, string database)
        {
            MySQL ret = new MySQL(server, database);
            ret.OpenConnection(userName, password);

            return ret; 
        }

        private MySQL(string server, string database)
        {
            Server = server;
            Database = database; 
        }

        private void OpenConnection(string userName, string password)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Server;
            builder.Database = Database;
            builder.UserID = userName;
            builder.Password = password; 

            _connection = new MySqlConnection();
            _connection.ConnectionString = builder.ToString();
            _connection.Open(); 
        }

        public void SaveRun(DateTime startDate, DateTime simulationStartDate, int westEastDimension, 
            int southNorthDimension, int bottomTopDimension, PhysicsConfigurationProcessed physicsConfiguration, string runId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRun(DateTime simulationEndDate, string runId)
        {
            throw new NotImplementedException();
        }
    }
}
