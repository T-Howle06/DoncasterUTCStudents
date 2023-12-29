using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TasksScreen.xaml
    /// </summary>
    public partial class TasksScreen : Window
    {

        private ObservableCollection<SharedFunctions.TaskItem> tasks;

        public TasksScreen()
        {
            InitializeComponent();
            tasks = new ObservableCollection<SharedFunctions.TaskItem>();
            tasksListView.ItemsSource = tasks;
            populateTasks();
        }

        private void populateTasks()
        {
            string currentUser = SharedFunctions.getCurrentUser();
            string query = "SELECT name, due FROM tasks WHERE username = @username";

            using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", currentUser);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string taskName = reader.GetString(reader.GetOrdinal("name"));
                            DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("due"));

                            tasks.Add(new SharedFunctions.TaskItem { TaskName = taskName, DueDate = dueDate });
                        }
                    }
                }
            }
        }

        private int getCurrentTaskID(SharedFunctions.TaskItem task)
        {
            string query = "SELECT id FROM tasks WHERE username = @username AND name = @name";

            string currentUser = SharedFunctions.getCurrentUser();

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", currentUser);
                        command.Parameters.AddWithValue("@name", task.TaskName);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            throw new Exception("Task not found for the given user and name.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Task ID Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private bool deleteTask(int taskID)
        {
            string query = "DELETE FROM tasks WHERE id = @taskId";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@taskId", taskID);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void addNewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskScreen newTaskScreen = new NewTaskScreen();
            
            bool? result = newTaskScreen.ShowDialog();

            if (result == true)
            {
                tasks.Clear();
                tasksListView.ItemsSource = tasks;
                populateTasks();
            }
        }

        private void editTaskButton_Click(object sender, RoutedEventArgs e)
        {
            SharedFunctions.TaskItem? selectedTask = tasksListView.SelectedItem as SharedFunctions.TaskItem;

            if (selectedTask != null)
            {
                int taskID = getCurrentTaskID(selectedTask);

                EditTaskScreen editTaskScreen = new EditTaskScreen(taskID);

                bool? result = editTaskScreen.ShowDialog();

                if (result == true)
                {
                    tasks.Clear();
                    tasksListView.ItemsSource = tasks;
                    populateTasks();
                }
            }
            else
            {
                MessageBox.Show("No task selected.", "Task Edit Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            SharedFunctions.TaskItem? selectedTask = tasksListView.SelectedItem as SharedFunctions.TaskItem;

            if (selectedTask != null )
            {
                int taskID = getCurrentTaskID(selectedTask);
                if (deleteTask(taskID))
                {
                    MessageBox.Show("Task deleted.", "Task Deletion Successful.", MessageBoxButton.OK, MessageBoxImage.Information);

                    tasks.Clear();
                    tasksListView.ItemsSource = tasks;
                    populateTasks();
                }
                else
                {
                    MessageBox.Show("Task not deleted.", "Task Deletion Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No task selected.", "Task Deletion Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen homeScreen = new HomeScreen();
            homeScreen.Show();
            this.Close();
        }
    }
}
