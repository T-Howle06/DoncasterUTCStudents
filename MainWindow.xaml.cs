using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;

namespace DoncasterUTCStudents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string connectionString = "Data Source=DESKTOP-RVQ5B1D\\SQLEXPRESS;Initial Catalog=DoncasterUTC;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private string hashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        private bool validLogin(string username, string password)
        {
            if (username == string.Empty)
            {
                MessageBox.Show("Please enter a username.", "Invalid Login.", MessageBoxButton.OK, MessageBoxImage.Error);
                usernameTextBox.Focus();
                return false;
            }
            else if (password == string.Empty)
            {
                MessageBox.Show("Please enter a password.", "Invalid Login.", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordPasswordBox.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool validateLogin(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM logins WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = hashPassword(passwordPasswordBox.Password);

            if (validLogin(username, passwordPasswordBox.Password) && validateLogin(username, password))
            {
                MessageBox.Show("Valid login.", "Valid Login.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!validateLogin(username, password))
            {
                MessageBox.Show("Please enter a valid login or create a new login.", "Invalid Login.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            usernameTextBox.Text = string.Empty;
            passwordPasswordBox.Password = string.Empty;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Hand);
        
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}