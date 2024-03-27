using System.Data.SqlClient;
using System.Windows;

namespace Laba5
{
    public partial class AuthWindow : Window
    {
        private string connectionString = "revision-pc\\sqlexpress.Laba5.dbo";

        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (Authenticate(username, password))
            {

            }
            else
            {
                MessageBox.Show("Неверные учетные данные");
            }
        }

        private bool Authenticate(string username, string password)
        {
            string query = $"SELECT COUNT(*) FROM Departments WHERE Lg = '{username}' AND Pswd = '{password}';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                int count = (int)command.ExecuteScalar();

                return (count > 0);
            }
        }
    }
}
