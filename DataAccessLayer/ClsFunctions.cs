using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ClsFunctions
    {
        private static string _LogName = "Application";
        



        public static DataTable GetDataTable<T>()
        {
            var tableAttribute = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));

            if (tableAttribute == null)
                throw new InvalidOperationException("Table attribute missing.");

            string table = tableAttribute.Name;

            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT * FROM {table}";

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
                string source = $"{table}.{nameof(GetDataTable)}";

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
                    Console.WriteLine($"Error Push Message to events: {ex.Message}.");
                }
            }

            return dataTable;
        }

        public static T GetInfoByID<T>(int? Id) where T : new()
        {
            var tableAttribute = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
            var KeyProp = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            if (tableAttribute == null || KeyProp == null)
                throw new InvalidOperationException("Table or Key attribute is missing.");

            string table = tableAttribute.Name;
            string IdColumn = KeyProp.Name;
            
            bool IsFound = false;
            T obj = new T();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT * FROM {table} WHERE {IdColumn} = @Id LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue($"@Id", Id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foreach (var prop in typeof(T).GetProperties())
                                {
                                    if (!reader.IsExistsColumn(prop.Name) || !prop.CanWrite)
                                        continue;

                                    int Ordinal = reader.GetOrdinal(prop.Name);

                                    if (Ordinal < 0)
                                        continue;

                                    object dpValue = reader[prop.Name];

                                    if (dpValue == null || dpValue == DBNull.Value)
                                    {
                                        prop.SetValue(obj, null);
                                    }
                                    else if (prop.PropertyType.IsEnum)
                                    {
                                        prop.SetValue(obj, Enum.ToObject(prop.PropertyType, dpValue));
                                    }
                                    else if (Nullable.GetUnderlyingType(prop.PropertyType) is Type UnderlyingType && UnderlyingType.IsEnum)
                                    {
                                        prop.SetValue(obj, Enum.ToObject(UnderlyingType, Convert.ToInt16(dpValue)));
                                    }
                                    else if (Nullable.GetUnderlyingType(prop.PropertyType) is Type UnderlyingTypeOne)
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(dpValue, UnderlyingTypeOne));
                                    }
                                    else
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(dpValue, prop.PropertyType));
                                    }
                                }

                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.{nameof(GetInfoByID)}";

                try
                {
                    if (EventLog.SourceExists(source))
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

            return IsFound ? obj : default;
        }

        public static int? Create<T>(T obj)
        {
            if (obj == null) throw new InvalidOperationException("Object is missing");

            var tableAttri = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
            var KeyProp = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            if (tableAttri == null || KeyProp == null) throw new InvalidOperationException("Table is missiong.");

            string table = tableAttri.Name;
            string IdColumn = KeyProp.Name;

            int? Id = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var ColumnNames = typeof(T).GetProperties().Where(p => p.Name != IdColumn);

                    string Paramertes = string.Join(", ", ColumnNames.Select(p => p.Name));
                    string Values = string.Join(", ", ColumnNames.Select(v => $"@{v.Name}"));
                    string Query = $"INSERT INTO {table}({Paramertes}) VALUES({Values}); SELECT LAST_INSERT_ID();";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var prop in ColumnNames)
                        {
                            object result = prop.GetValue(obj) ?? DBNull.Value;
                            command.Parameters.AddWithValue($"@{prop.Name}", result);
                        }

                        object IdResult = command.ExecuteScalar();
                        Id = int.TryParse(IdResult?.ToString(), out int OutPut) ? OutPut : (int?)null;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.Create";

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

            return Id;
        }

        public static bool Update<T>(T obj) where T :new()
        {
            var tableAttri = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
            var KeyProp = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            if (tableAttri == null || KeyProp == null) throw new InvalidOperationException("table or Key is missing.");

            string table = tableAttri.Name;
            string IdColumn = KeyProp.Name;

            int RowsEffector = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var props = typeof(T).GetProperties();

                    string Clauses = string.Join(", ", props.Where(p => p.Name != IdColumn).Select(p => $"{p.Name} = @{p.Name}"));

                    string Query = $"UPDATE {table} SET {Clauses} WHERE {IdColumn} = @{IdColumn}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var prop in props)
                        {
                            object result = prop.GetValue(obj) ?? DBNull.Value;
                            command.Parameters.AddWithValue($"@{prop.Name}", result);
                        }

                        RowsEffector = command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.Update";

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

            return (RowsEffector > 0);
        }

        public static bool Update<T>(Dictionary<string, object> parameters) where T : new()
        {
            var tableAttri = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
            var KeyProp = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            if (tableAttri == null || KeyProp == null) throw new InvalidOperationException("table or Key is missing.");

            string table = tableAttri.Name;
            string IdColumn = KeyProp.Name;

            int RowsEffector = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var Cluses = parameters.Keys.Select(p => $"{p} = @{p}");
                    string Query = $"UPDATE {table} SET {string.Join(", ", Cluses)} WHERE {IdColumn} = @{IdColumn}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value ?? DBNull.Value);
                        }

                        RowsEffector = command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.Update";

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

            return (RowsEffector > 0);
        }

        public static bool DeleteById<T>(int? Id) where T : new()
        {
            var tableAttr = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
            var KeyProp = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            if (tableAttr == null || KeyProp == null)
                throw new InvalidOperationException("Table or key attribute missing.");

            string table = tableAttr.Name;
            string IdColumn = KeyProp.Name;

            int RowsEffected = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"DELETE FROM {table} WHERE {IdColumn} = @Id";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);

                        RowsEffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.Delete";

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

            return (RowsEffected > 0);
        }

        public static bool IsExists<T>(Dictionary<string, object> parameters) where T : new()
        {
            var tableAttr = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));

            if (tableAttr == null)
                throw new InvalidOperationException("Table is missing.");

            string table = tableAttr.Name;
            bool IsFound = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var wherClauses = parameters.Keys.Select(K => $"{K} = @{K}");
                    string Query = $"SELECT 1 FROM {table} WHERE {string.Join(" AND ", wherClauses)}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value ?? DBNull.Value);
                        }

                        object result = command.ExecuteScalar();

                        IsFound = (result != null);
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.IsExists";

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

            return IsFound;
        }

        public static bool IsExists<T>(int? Id) where T : new()
        {
            if (!Id.HasValue) return false;
            var tableAttribute = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
            var KeyProp = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            if (tableAttribute == null || KeyProp == null)
                throw new InvalidOperationException("Table name or key is missing.");

            string table = tableAttribute.Name;
            string IdColumn = KeyProp.Name;

            bool IsFound = false;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT 1 FROM {table} WHERE {IdColumn} = @Id";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);

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
            catch (MySqlException e)
            {
                string source = $"{table}.IsExists";

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

            return IsFound;
        }





        public static T GetInfoByString<T>(string Id, string IdColumn, string IdOne = null, string IdColumnOne = null) where T : new()
        {
            var tableAttribute = (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));

            if (tableAttribute == null)
                throw new InvalidOperationException("Table Attribute is missing");

            string table = tableAttribute.Name;

            bool IsFound = false;
            T obj = new T();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT * FROM {table} WHERE {IdColumn} = @{IdColumn} ";

                    if (IdOne != null && IdColumnOne != null)
                    {
                        Query += $"AND {IdColumnOne} = @{IdColumnOne}";
                    }

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue($"@{IdColumn}", Id);

                        if (IdOne != null && IdColumnOne != null)
                        {
                            command.Parameters.AddWithValue($"@{IdColumnOne}", IdOne);
                        }

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                foreach (var prop in typeof(T).GetProperties())
                                {
                                    if (!reader.IsExistsColumn(prop.Name) || !prop.CanWrite)
                                        continue;

                                    object dpValue = reader[prop.Name];

                                    if (dpValue == null || dpValue == DBNull.Value)
                                    {
                                        prop.SetValue(obj, null);
                                    }
                                    else if (prop.PropertyType.IsEnum)
                                    {
                                        prop.SetValue(obj, Enum.ToObject(prop.PropertyType, dpValue));
                                    }
                                    else if (Nullable.GetUnderlyingType(prop.PropertyType) is Type Underlyingtype && Underlyingtype.IsEnum)
                                    {
                                        prop.SetValue(obj, Enum.ToObject(Underlyingtype, Convert.ToInt32(dpValue)));
                                    }
                                    else if (Nullable.GetUnderlyingType(prop.PropertyType) is Type UnderlyingType)
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(dpValue, UnderlyingType));
                                    }
                                    else
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(dpValue, prop.PropertyType));
                                    }
                                }

                                IsFound = true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{table}.{nameof(GetInfoByString)}";

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

            return IsFound ? obj : default;
        }

        public static int? Create<T>(string tableName, string IdColumn, T obj)
        {
            int? Id = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var props = typeof(T).GetProperties().Where(p => p.Name != IdColumn);

                    string columnList = string.Join(", ", props.Select(p => p.Name));

                    string parametersList = string.Join(", ", props.Select(p => $"@{p.Name}"));

                    string Query = $"INSERT INTO {tableName}({columnList}) VALUES({parametersList}); SELECT LAST_INSERT_ID();";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var prop in props.Where(p => p.Name != IdColumn))
                        {
                            object value = prop.GetValue(obj) ?? DBNull.Value;
                            command.Parameters.AddWithValue($"@{prop.Name}", value);
                        }

                        object result = command.ExecuteScalar();
                        Id = int.TryParse(result?.ToString(), out int id) ? id : (int?)null;
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{tableName}.{nameof(Create)}";

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
                    Console.WriteLine($"Error push messages to events: {ex.Message}");
                }
            }

            return Id;
        }

        public static bool Update<T>(string tableName, string IdColumn, T obj)
        {
            int RowsEffector = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var props = typeof(T).GetProperties().Where(p => !Attribute.IsDefined(p, typeof(ClsIgnoreAttribute)));

                    string ParaList = string.Join(", ", props.Where(p => p.Name != IdColumn).Select(p => $"{p.Name} = @{p.Name}"));

                    string Query = $"UPDATE {tableName} SET {ParaList} WHERE {IdColumn} = @{IdColumn}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var prop in props)
                        {
                            object result = prop.GetValue(obj) ?? DBNull.Value;
                            command.Parameters.AddWithValue($"@{prop.Name}", result);
                        }

                        RowsEffector = command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{tableName}.{nameof(Update)}";

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
                    Console.WriteLine($"Error push Messages events: {ex.Message}.");
                }
            }

            return (RowsEffector > 0);
        }

        public static bool Delete(string tableName, int? Id, string IdColumn)
        {
            int RowsEffector = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"DELETE FROM {tableName} WHERE {IdColumn} = @{IdColumn}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue($"@{IdColumn}", Id);

                        RowsEffector = command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                string source = $"{tableName}.{nameof(Delete)}";

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
                    Console.WriteLine($"Error to push message to events: {ex.Message}.");
                }
            }

            return (RowsEffector > 0);
        }

    }
}
