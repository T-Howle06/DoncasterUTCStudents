using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoncasterUTCStudents
{
    /// <summary>
    /// Interaction logic for LoginCreationScreen.xaml
    /// </summary>
    public partial class LoginCreationScreen : Window
    {
        public LoginCreationScreen()
        {
            InitializeComponent();
        }

        private bool addToDatabase(string username, string password)
        {
            string query = "INSERT INTO logins (username, password) VALUES (@username, @password)";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Login Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = SharedFunctions.hashPassword(passwordPasswordBox.Password);

            if (addToDatabase(username, password))
            {
                MessageBoxResult result = MessageBox.Show("Would you like to login in?", "Login Creation Successful", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
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
