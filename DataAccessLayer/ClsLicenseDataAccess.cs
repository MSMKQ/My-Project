using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClsLicenseDataAccess
    {
        private static string _LogName = "Application";

        public static bool IsThereLicense(int? PersonID, int? LicenseClassID)
        {
            bool IsFound = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT 1 FROM Licenses L INNER JOIN Drivers D ON L.DriverID = D.DriverID WHERE D.PersonID = @PersonID AND L.LicenseClassID = @LicenseClassID";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch ( MySqlException e)
            {
                string source = "Licenses.IsThereLicense";

                try
                {
                    if (!EventLog.SourceExists(source))
                    {
                        EventLog.CreateEventSource(source, _LogName);
                    }

                    EventLog.WriteEntry(source, e.Message, EventLogEntryType.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writting Message to events: {ex.Message}.");
                }
            }

            return IsFound;
        }
    }
}
