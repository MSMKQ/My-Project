using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClsTestAppointmentDataAccess
    {
        private static string _LogName = "Application";


        public static DataTable GetAppointments(int? LocalDrivingLicenseApplicationID, int? TestTypeID)
        {
            DataTable TestAppointments = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT TA.TestAppointmentID, TA.AppointmentDate, TA.PaidFees, U.UserName, TA.IsLocked FROM localdrivinglicenseapplications L INNER JOIN TestAppointments TA ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID INNER JOIN Users U ON U.UserID = TA.CreatedByUserID WHERE L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TA.TestTypeID = @TestTypeID ORDER BY TestAppointmentID DESC";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                TestAppointments.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = "TestAppointments.GetTestAppointments";

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
                    Console.WriteLine($"Error Writting Message to events: {ex.Message}.");
                }
            }

            return TestAppointments;
        }

        public static bool IsThereAnActiveAppointment(int? LocalDrivingLicenseApplicationID, int? TestAppointmentID)
        {
            bool IsThereAnActiveAppointment = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT 1 FROM LocalDrivingLicenseApplications L INNER JOIN TestAppointments TA ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID WHERE L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TA.TestAppointmentID = @TestAppointmentID AND (TA.IsLocked = 0 or TA.IsLocked is NULL)";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                IsThereAnActiveAppointment = true;
                            }
                        }
                    }
                }
            }
            catch ( MySqlException e)
            {
                string source = "TestAppointments.IsThereAnActiveAppointment";

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

            return IsThereAnActiveAppointment;
        }

        public static byte TotalTrails(int? LocalDrivingLicenseApplicationID, int? TestTypeID)
        {
            byte TotalTrails = 9;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT Count(TestTypeID) As TotalTrails FROM LocalDrivingLicenseApplications L INNER JOIN TestAppointments TA ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID WHERE L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TA.TestTypeID = @TestTypeID";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        object result = command.ExecuteScalar();
                        TotalTrails = byte.TryParse(result?.ToString(), out byte Output) ? Output : (byte)0;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = "TestAppointments.TotalTrails";

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

            return TotalTrails;
        }
    }
}
