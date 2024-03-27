using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Laba5
{
    public partial class MainWindow : Window
    {
        private class DatabaseManager
        {
            private string connectionString;

            public DatabaseManager(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public void LoadDataToDataGrid(string tableName, DataGrid dataGrid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"SELECT * FROM {tableName}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        dataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }

            public void InsertData(string tableName, Dictionary<string, string> data)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    StringBuilder columns = new StringBuilder();
                    StringBuilder values = new StringBuilder();

                    foreach (KeyValuePair<string, string> entry in data)
                    {
                        columns.Append(entry.Key + ",");
                        values.Append("'" + entry.Value + "',");
                    }

                    // удалить последнюю запятую
                    columns.Remove(columns.Length - 1, 1);
                    values.Remove(values.Length - 1, 1);

                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            public void UpdateData(string tableName, Dictionary<string, string> data, string condition)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    StringBuilder updateValues = new StringBuilder();

                    foreach (KeyValuePair<string, string> entry in data)
                    {
                        updateValues.Append(entry.Key + "='" + entry.Value + "',");
                    }

                    // удалить последнюю запятую
                    updateValues.Remove(updateValues.Length - 1, 1);

                    string query = $"UPDATE {tableName} SET {updateValues} WHERE {condition}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            public void DeleteData(string tableName, string condition)
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"DELETE FROM {tableName} WHERE {condition}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            internal void LoadDataToDataGrid(string tableName, object dataGrid)
            {
                throw new NotImplementedException();
            }
        }

        private DatabaseManager databaseManager;

        private string connectionString = "Ваша строка подключения";

        public MainWindow()
        {
            InitializeComponent();

            databaseManager = new DatabaseManager(connectionString);

            // Загрузить данные таблицы "Employees" в dataGrid
            databaseManager.LoadDataToDataGrid("Employees", dataGrid);

            // Добавить новую запись в таблицу "Employees"
            Dictionary<string, string> employeeData = new Dictionary<string, string>
            {
                {"Name", "John Doe"},
                {"Position", "Manager"},
                {"Salary", "5000"}
            };
            databaseManager.InsertData("Employees", employeeData);

            // Обновить данные в таблице "Employees"
            Dictionary<string, string> updatedData = new Dictionary<string, string>
            {
                {"Name", "Jane Smith"},
                {"Position", "Supervisor"},
                {"Salary", "6000"}
            };
            string condition = "ID = 1";
            databaseManager.UpdateData("Employees", updatedData, condition);

            // Удалить запись из таблицы "Employees"
            string deleteCondition = "ID = 2";
            databaseManager.DeleteData("Employees", deleteCondition);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            // Ваш код здесь
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            string tableName = "Ваша таблица";
            databaseManager.LoadDataToDataGrid(tableName, dataGrid);
        }
    }
}
