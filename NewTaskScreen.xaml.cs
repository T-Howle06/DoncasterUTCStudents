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
    /// Interaction logic for NewTaskScreen.xaml
    /// </summary>
    public partial class NewTaskScreen : Window
    {
        public NewTaskScreen()
        {
            InitializeComponent();
        }

        private bool addToDatabase(string username, string taskName, DateTime? taskDueDate)
        {
            string query = "INSERT INTO tasks (username, name, due) VALUES (@username, @name, @due)";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@name", taskName);
                        command.Parameters.AddWithValue("@due", taskDueDate);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Task Creation Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void createTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string username = SharedFunctions.getCurrentUser();
            string taskName = taskNameTextBox.Text;
            DateTime? dateTime = taskDueDateDatePicker.SelectedDate;

            if (taskName == null || dateTime == null)
            {
                MessageBox.Show("Invalid details.", "Task Creation Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (addToDatabase(username, taskName, dateTime))
            {
                MessageBoxResult result = MessageBox.Show("Would you like to add another task?", "Task Creation Successful.", MessageBoxButton.YesNo, MessageBoxImage.Information);
                
                if (result == MessageBoxResult.Yes)
                {
                    taskNameTextBox.Text = string.Empty;
                    taskDueDateDatePicker.SelectedDate = null;
                }
                else
                {
                    createTaskButton.IsEnabled = false;
                    DialogResult = true;
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Task Creation Unsucessful.", "Task Creation Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }
    }
}
