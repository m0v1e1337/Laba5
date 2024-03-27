using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using NamespaceToAddRecordWindow;
using NamespaceToUpdateRecordWindow;


namespace Laba5
{
    public partial class ManagerWindow : Window
    {
        private string connectionString = "revision-pc\\sqlexpress.Laba5.dbo";

        public ManagerWindow()
        {
            InitializeComponent();

            LoadTableNames();
        }

        private void LoadTableNames()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Products FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cbTables.Items.Add(reader["Products"].ToString());
                }

                reader.Close();
            }
        }

        private void cbTables_SelectionChanged(object sender, SelectionChangedEventArgs e, object cbTables)
        {
            string selectedTable = cbTables.ToString();
            
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM {tableName};";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dgData.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string selectedTable = cbTables.SelectedItem.ToString();

            AddRecordWindow addRecordWindow = new AddRecordWindow(connectionString, selectedTable);
            addRecordWindow.ShowDialog();

            // Обновление данных после добавления записи

            LoadTableData(selectedTable);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            string selectedTable = cbTables.SelectedValue.ToString();

            LoadTableData(selectedTable);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgData.SelectedItem != null)
            {
                string selectedTable = cbTables.SelectedValue.ToString();
                DataRowView row = (DataRowView)dgData.SelectedItem;

                UpdateRecordWindow updateRecordWindow = new UpdateRecordWindow(connectionString, selectedTable, row);
                updateRecordWindow.ShowDialog();

                // Обновление данных после изменения записи
                LoadTableData(selectedTable);
            }
            else
            {
                MessageBox.Show("Выберите запись для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgData.SelectedItem != null)
            {
                string selectedTable = cbTables.SelectedValue.ToString();
                DataRowView row = (DataRowView)dgData.SelectedItem;

                // Получаем значение первичного ключа для удаления записи
                object primaryKeyValue = row[selectedTable + "_ID"];

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteRecord(selectedTable, primaryKeyValue);

                    // Обновление данных после удаления записи
                    LoadTableData(selectedTable);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }

        private void DeleteRecord(string tableName, object primaryKeyValue)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"DELETE FROM {tableName} WHERE {tableName}_ID = @PrimaryKeyValue;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
