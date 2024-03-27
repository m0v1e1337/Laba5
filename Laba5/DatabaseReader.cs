using System;
using System.Data;
using System.Data.SqlClient;

namespace NamespaceToDatabaseReader
{
    public class DatabaseReader
    {
        public DataTable ReadTable(string connectionString, string tableName)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM {tableName}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }
    }
}
