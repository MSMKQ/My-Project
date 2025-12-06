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
    public class ClsUserDataAccess
    {
        private static string _LogName = "Application";


        public static DataTable GetUsers(string tableName)
        {
            DataTable Users = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = "SELECT U.UserID, U.PersonID, CONCAT_WS(' ', P.FirstName, P.SecondName, P.ThirdName, P.LastName) As FullName, U.Username, U.IsActive FROM Users U INNER JOIN People P ON P.PersonID = U.PersonID";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Users.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{tableName}.{nameof(GetUsers)}";

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
                    Console.WriteLine($"Error push message to events: {ex.Message}.");
                }
            }

            return Users;
        }
    }
}
