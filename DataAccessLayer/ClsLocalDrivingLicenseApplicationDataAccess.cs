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
    public class ClsLocalDrivingLicenseApplicationDataAccess
    {

        private static string _LogName = "Applications";


        public static int? IsThereAnActiveApplication(int? ApplicationPersonID, int? LicenseClassID)
        {
            int? ActiveApplicationID = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT L.ApplicationID As ActiveApplicationID FROM localdrivinglicenseapplications L INNER JOIN applications A ON L.ApplicationID = A.ApplicationID WHERE L.LicenseClassID = @LicenseClassID AND A.ApplicationPersonID = @ApplicationPersonID AND A.ApplicationStatus = 1";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@ApplicationPersonID", ApplicationPersonID);

                        object result = command.ExecuteScalar();
                        ActiveApplicationID = int.TryParse(result?.ToString(), out int Output) ? Output : (int?)null;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"LocalDrivingLicenseApplications.IsThereAnActiveApplication";

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
                    Console.WriteLine($"Error writting to events: {ex.Message}.");
                }
            }

            return ActiveApplicationID;
        }

        public static DataTable GetApplications<T>()
        {
            var AttributeTable = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));

            string table = AttributeTable.Name;

            DataTable _Applications = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT L.LocalDrivingLicenseApplicationID, LC.Title, Concat_ws(' ', P.FirstName, P.SecondName, P.ThirdName, P.LastName) as FullName, P.NationID, A.ApplicationDate, CASE WHEN A.ApplicationStatus = 1 THEN 'New' WHEN A.ApplicationStatus = 2 THEN 'Cancelled' WHEN A.ApplicationStatus = 3 THEN 'Compeleted' ELSE 'Unknown' END AS ApplicationStatus, A.LastStatusDate, U.Username FROM LocalDrivingLicenseApplications L INNER JOIN Applications A ON A.ApplicationID = L.ApplicationID INNER JOIN People P ON A.ApplicationPersonID = P.PersonID INNER JOIN LicenseClasses LC ON L.LicenseClassID = LC.LicenseClassID INNER JOIN Users U ON A.CreatedByUserID = U.UserID ORDER BY L.LocalDrivingLicenseApplicationID ASC ";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                _Applications.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.{nameof(GetApplications)}";

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
                    Console.WriteLine($"Error Writting Message to Events: {ex.Message}.");
                }
            }

            return _Applications;
        }

        public static bool DoesPassedTest(int? LocalDrivingLicenseApplicationID, byte? TestTypeID)
        {
            bool TestResult = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT T.TestResult FROM localdrivinglicenseapplications L INNER JOIN testappointments TA ON L.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID INNER JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID WHERE L.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TA.TestTypeID = @TestTypeID ORDER BY T.TestID DESC LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        object result = command.ExecuteScalar();
                        TestResult = bool.TryParse(result?.ToString(), out bool Output) ? Output : false;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = "LocalDrivingLicenseApplication.DoesPassedTest";

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

            return TestResult;
        }
    }
}
