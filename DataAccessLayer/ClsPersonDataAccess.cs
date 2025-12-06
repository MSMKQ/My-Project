using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClsPersonDataAccess
    {
        private static string _LogName = "Application";

        public static DataTable GetPeople(string tableName)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT P.PersonID, CONCAT_WS(' ', P.FirstName, P.SecondName, P.ThirdName, P.LastName) As FullName, P.NationID, P.DateOfBirth, CASE WHEN P.Gender = 1 THEN 'Male' WHEN P.Gender = 0 THEN 'Female' ELSE 'Unknown' END As Gender, P.Phone, P.Email, C.CountryName, P.Address, P.ImagePath FROM People P INNER JOIN Countries C ON P.CountryID = C.CountryID";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dataTable.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{tableName}.{nameof(GetPeople)}";

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
                    Console.WriteLine($"Error push message events: {ex.Message}.");
                }
            }

            return dataTable;
        }

        public static string GetLastNationID(string tableName)
        {
            string LastNationID = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT NationID As LastNationID FROM People ORDER BY NationID DESC LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        LastNationID = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{tableName}.{nameof(LastNationID)}";

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
                    Console.WriteLine($"Error: {ex.Message}.");
                }
            }

            return LastNationID;
        }
    }
}
