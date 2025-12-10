using DataAccessLayer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class GenericMapper
    {
        private const string _LogName = "Application";

        private static bool HasColumn(this MySqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).EndsWith(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private static void _LogError(string source, string message)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, _LogName);
                }

                EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }
            catch
            {
                Console.WriteLine($"Error writing to event log: {message}.");
            }
        }


        //Functions

        private static ClsTableAttribute _GetAttributeTable<T>()
        {
            return (ClsTableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ClsTableAttribute));
        }

        private static Type _GetAttributeKey<T>()
        {
            var prop = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(ClsKeyAttribute)));

            return prop?.PropertyType;
        }

        private static IEnumerable<PropertyInfo> _GetMapperProperties<T>(string excludeColumn = null)
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(p => p.CanRead && p.Name != excludeColumn && (p.PropertyType.IsPrimitive || p.PropertyType.IsEnum || p.PropertyType == typeof(string) || Nullable.GetUnderlyingType(p.PropertyType) != null));
        }

        public static int? Create<T>(T obj)
        {
            var tableAttribute = _GetAttributeTable<T>();
            var KeyProp = _GetAttributeKey<T>();

            if (tableAttribute == null || KeyProp == null)
                throw new InvalidOperationException("Table or Key is missing.");

            string table = tableAttribute.Name;
            string IdColumn = KeyProp.Name;

            int? Id = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var props = _GetMapperProperties<T>(IdColumn);
                    string ColNames = string.Join(", ", props.Select(p => p.Name));
                    string ColValues = string.Join(", ", props.Select(p => $"@{p.Name}"));
                    string Query = $"INSERT INTO {table}({ColNames}) VALUES({ColValues});";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var prop in props)
                        {
                            object value = prop.GetValue(obj) ?? DBNull.Value;
                            command.Parameters.AddWithValue($"@{prop.Name}", value);
                        }

                        command.ExecuteNonQuery();

                        command.CommandText = "SELECT LAST_INSERT_ID();";
                        object result = command.ExecuteScalar();
                        Id = int.TryParse(result?.ToString(), out int Output) ? Output : (int?)null;
                    }
                }
            }
            catch (MySqlException e)
            {
                _LogError($"{table}.Create", e.Message);
            }

            return Id;
        }

        public static bool Update<T>(T obj)
        {
            var tableAttribute= _GetAttributeTable<T>();
            var KeyProp = _GetAttributeKey<T>();

            if (tableAttribute == null || KeyProp == null)
                throw new InvalidOperationException("Table or Key is missing.");

            string table = tableAttribute.Name;
            string IdColumn = KeyProp.Name;

            int RowsEffected = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    var props = _GetMapperProperties<T>();
                    string SetClause = string.Join(", ", props.Where(p => p.Name != IdColumn).Select(p => $"{p} = @{p}"));
                    string Query = $"UPDATE {table} SET {SetClause} WHERE {IdColumn} = @{IdColumn}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var prop in props)
                        {
                            object value = prop.GetValue(obj) ?? DBNull.Value;
                            command.Parameters.AddWithValue($"@{prop.Name}", value);
                        }

                        RowsEffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                _LogError($"{table}.Update", e.Message);
            }

            return (RowsEffected > 0);
        }

        public static T FindBy<T>(int Id) where T : new()
        {
            var tableAttribute = _GetAttributeTable<T>();
            var KeyProp = _GetAttributeKey<T>();

            if (tableAttribute == null || KeyProp == null)
                throw new InvalidOperationException("Table or Key is missing.");

            string table = tableAttribute.Name;
            string IdColumn = KeyProp.Name;

            T obj = default;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string Query = $"SELECT * FROM {table} WHERE {IdColumn} = @{IdColumn}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue($"@{IdColumn}", Id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                obj = new T();

                                foreach (var prop in typeof(T).GetProperties())
                                {
                                    if (!reader.HasColumn(prop.Name)) continue;
                                    object value = reader[prop.Name];
                                    if (value == DBNull.Value) value = null;
                                    prop.SetValue(obj, value);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                _LogError($"{table}.FindBy", e.Message);
            }

            return obj;
        }

        public static List<T> FindBy<T>(Dictionary<string, object> conditions) where T : new()
        {
            var tableAttribute = _GetAttributeTable<T>();

            if (tableAttribute == null ) throw new InvalidOperationException("Table is missing.");

            string table = tableAttribute.Name;
            var list = new List<T>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ClsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string wherClause = string.Join(" AND ", conditions.Keys.Select(k => $"{k} = @{k}"));
                    string Query = $"SELECT * FROM {table} WHERE {wherClause}";

                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        foreach (var condition in conditions)
                        {
                            command.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
                        }

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var obj = new T();

                                foreach (var prop in typeof(T).GetProperties())
                                {
                                    if (!reader.HasColumn(prop.Name)) continue;
                                    object val = reader[prop.Name];
                                    if (val == DBNull.Value) val = null;
                                    prop.SetValue(obj, val);
                                }

                                list.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                _LogError($"{table}.FindBy", e.Message);
            }

            return list;
        }
    }
}
