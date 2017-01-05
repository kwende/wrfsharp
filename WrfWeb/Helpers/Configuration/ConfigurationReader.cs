using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WrfWeb.Helpers.Configuration
{
    public static class ConfigurationReader
    {
        public static string ReadDatabaseConnectionString()
        {
            IConfigurationBuilder builder = 
                new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("dbsettings.json");
            IConfigurationRoot config = builder.Build();

            return config["ConnectionString"]; 
        }
    }
}
