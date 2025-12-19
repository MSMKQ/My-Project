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

        public static bool DoesAttendedTest(int? LocalDrivingLicenseApplicationID, int? TestTypeID)
        {
            bool IsAttended = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT 1 FROM LocalDrivingLicenseApplications L INNER JOIN TestAppointments TA ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID INNER JOIN Tests T ON TA.TestAppointmentID = T.TestAppointmentID WHERE L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TA.TestTypeID = @TestTypeID LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        using ( MySqlDataReader reader = command.ExecuteReader())
                        {
                            if ( reader.HasRows )
                            {
                                IsAttended = true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = "Tests.DoesAttendedTest";

                try
                {
                    if ( !EventLog.SourceExists(source) )
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

            return IsAttended;
        }

        public static int? GetTestID(int? TestAppointmentID)
        {
            int? TestID = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT TestID FROM Tests T INNER JOIN TestAppointments TA ON TA.TestAppointmentID = T.TestAppointmentID WHERE TA.TestAppointmentID = @TestAppointmentID";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                        object result = command.ExecuteScalar();
                        TestID = int.TryParse(result?.ToString(), out int Output) ? Output : (int?)null;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = "Tests.GetTestID";

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

            return TestID;
        }
    }
}
