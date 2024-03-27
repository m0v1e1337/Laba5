using System;
using System.Data;
using System.Data.SqlClient;

namespace NamespaceToDatabaseUpdater
{
    public class DatabaseUpdater
    {
        public bool UpdateData(string connectionString, string tableName, SqlParameter[] parameters, string condition)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"UPDATE {tableName} SET ";

                for (int i = 0; i < parameters.Length; i++)
                {
                    query += $"{parameters[i].ParameterName} = @{parameters[i].ParameterName},";
                }

                query = query.TrimEnd(',') + $" WHERE {condition}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }

            return success;
        }
    }
}
