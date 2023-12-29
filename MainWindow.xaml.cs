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
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool validateLogin(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM logins WHERE username = @username AND password = @password";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
        }

        private bool addToLog(string username)
        {
            DateTime dateTime = DateTime.Now;
            string query = "INSERT INTO log (username, time) VALUES (@username, @time)";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@time", dateTime);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void createNewLoginLink_Click(object sender, EventArgs e)
        {
            LoginCreationScreen loginCreationScreen = new LoginCreationScreen();
            loginCreationScreen.Show();
            this.Close();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = SharedFunctions.hashPassword(passwordPasswordBox.Password);

            if (SharedFunctions.validLogin(usernameTextBox, passwordPasswordBox, username, password) && validateLogin(username, password))
            {
                MessageBox.Show("Valid login.", "Valid Login.", MessageBoxButton.OK, MessageBoxImage.Information);

                addToLog(username);

                HomeScreen homeScreen = new HomeScreen();
                homeScreen.Show();
                this.Close();
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