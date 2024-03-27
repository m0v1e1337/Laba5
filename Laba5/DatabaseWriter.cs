using System;
using System.Data;
using System.Data.SqlClient;

namespace NamespaceToDatabaseWriter
{
    public class DatabaseWriter
    {
        public bool InsertData(string connectionString, string tableName, SqlParameter[] parameters)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"INSERT INTO {tableName} VALUES (";

                for (int i = 0; i < parameters.Length; i++)
                {
                    query += $"@param{i},";
                }

                query = query.TrimEnd(',') + ")";

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
