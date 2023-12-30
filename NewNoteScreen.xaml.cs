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
    /// Interaction logic for NewNoteScreen.xaml
    /// </summary>
    public partial class NewNoteScreen : Window
    {
        public NewNoteScreen()
        {
            InitializeComponent();
        }

        private bool addToDatabase(string username, string noteName, string noteContents)
        {
            string query = "INSERT INTO notes (username, name, contents) VALUES (@username, @name, @contents)";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@name", noteName);
                        command.Parameters.AddWithValue("@contents", noteContents);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Note Creation Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void createNoteButton_Click(object sender, RoutedEventArgs e)
        {
            string username = SharedFunctions.getCurrentUser();
            string noteName = noteNameTextBox.Text;
            TextRange textRange = new TextRange(noteContentsRichTextBox.Document.ContentStart, noteContentsRichTextBox.Document.ContentEnd);
            string noteContents = textRange.Text;

            if (noteName == null || noteContents == null)
            {
                MessageBox.Show("Invalid details.", "Note Creation Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (addToDatabase(username, noteName, noteContents))
            {
                MessageBoxResult result = MessageBox.Show("Would you like to add another note?", "Note Creation Successful.", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    noteNameTextBox.Text = string.Empty;
                    noteContentsRichTextBox.Document = null;
                }
                else
                {
                    createNoteButton.IsEnabled = false;
                    DialogResult = true;
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Note Creation Unsucessful.", "Note Creation Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
