using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClsTestDataAccess
    {
        private static string _LogName = "Application";

        public static byte TotalTestsPassed(int? LocalDrivingLicenseApplicationID)
        {
            byte TotalTestsPassed = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT Count(TA.TestTypeID) As TotalCountPassed FROM localdrivinglicenseapplications L INNER JOIN testappointments TA ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID INNER JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID WHERE L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND  T.TestResult = 1";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                        object result = command.ExecuteScalar();
                        TotalTestsPassed = byte.TryParse(result.ToString(), out byte Output) ? Output : (byte)0;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"Tests.TotalTestPassed";

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
                    Console.WriteLine($"Error writting message to events: {ex.Message}.");
                }
            }

            return TotalTestsPassed;
        }
    }
}
