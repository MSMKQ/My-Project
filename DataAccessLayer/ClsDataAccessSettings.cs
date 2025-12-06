using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccessLayer
{
    public class ClsDataAccessSettings
    {

        public static string ConnectionString { get; set; }

        static ClsDataAccessSettings()
        {
            var connection = ConfigurationManager.ConnectionStrings["MyData"].ConnectionString;

            if (string.IsNullOrWhiteSpace(connection) || connection == null)
            {
                throw new ConfigurationErrorsException("Connection string 'MyData' is missing or empty in the configuration file.");
            }

            ConnectionString = connection;
        }
    }
}
