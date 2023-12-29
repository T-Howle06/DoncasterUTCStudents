using Microsoft.VisualBasic;
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
using System.Windows.Shell;

namespace DoncasterUTCStudents
{
    /// <summary>
    /// Interaction logic for EditTaskScreen.xaml
    /// </summary>
    public partial class EditTaskScreen : Window
    {
        private int selectedTaskID;

        public EditTaskScreen(int taskID)
        {
            InitializeComponent();
            selectedTaskID = taskID;
        }

        private bool editTaskDatabase(string taskName, DateTime taskDue)
        {
            string query = "UPDATE tasks SET name = @newName, due = @newDue WHERE id = @taskID";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newName", taskName);
                        command.Parameters.AddWithValue("@newDue", taskDue);
                        command.Parameters.AddWithValue("@taskID", selectedTaskID);

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Task Edit Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void editTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = taskNameTextBox.Text;
            DateTime? newDateTime = taskDueDateDatePicker.SelectedDate;

            if (newDateTime == null)
            {
                MessageBox.Show("Please enter new due date.", "Task Edit Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (editTaskDatabase(newName, newDateTime.Value))
            {
                MessageBoxResult result = MessageBox.Show("Task successfuly updated.", "Task Edit Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Hide();
            }
        }
    }
}
