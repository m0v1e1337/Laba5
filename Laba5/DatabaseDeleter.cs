using System;
using System.Data;
using System.Data.SqlClient;

namespace NamespaceToDatabaseDeleter
{
    public class DatabaseDeleter
    {
        public bool DeleteData(string connectionString, string tableName, string condition)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"DELETE FROM {tableName} WHERE {condition}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
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
