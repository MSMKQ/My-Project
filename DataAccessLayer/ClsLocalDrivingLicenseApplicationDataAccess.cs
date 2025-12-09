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

                    string Query = "SELECT L.LocalDrivingLicenseApplicationID, LC.Title, Concat_ws(' ', P.FirstName, P.SecondName, P.ThirdName, P.LastName) as FullName, P.NationID, A.ApplicationDate, CASE WHEN A.ApplicationStatus = 1 THEN 'New' WHEN A.ApplicationStatus = 2 THEN 'Cancelled' WHEN A.ApplicationStatus = 3 THEN 'Compeleted' ELSE 'Unknown' END AS ApplicationStatus, A.LastStatusDate, U.Username FROM LocalDrivingLicenseApplications L INNER JOIN Applications A ON A.ApplicationID = L.ApplicationID INNER JOIN People P ON A.ApplicationPersonID = P.PersonID INNER JOIN LicenseClasses LC ON L.LicenseClassID = LC.LicenseClassID INNER JOIN Users U ON A.CreatedByUserID = U.UserID";

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
    }
}
