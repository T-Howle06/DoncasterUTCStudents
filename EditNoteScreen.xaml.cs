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
    /// Interaction logic for EditNoteScreen.xaml
    /// </summary>
    public partial class EditNoteScreen : Window
    {
        private int selectedNoteID;

        public EditNoteScreen(int noteID)
        {
            InitializeComponent();
            selectedNoteID = noteID;
            populateNote();
        }

        private void populateNote()
        {
            string currentUser = SharedFunctions.getCurrentUser();
            string query = "SELECT name, contents FROM notes WHERE username = @username";

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
                            string noteName = reader.GetString(reader.GetOrdinal("name"));
                            string noteContent = reader.GetString(reader.GetOrdinal("contents"));

                            noteNameTextBox.Text = noteName;

                            FlowDocument flowDocument = new FlowDocument();
                            Paragraph paragraph = new Paragraph(new Run(noteContent));
                            flowDocument.Blocks.Add(paragraph);
                            noteContentsRichTextBox.Document = flowDocument;
                        }
                    }
                }
            }
        }

        private bool editNoteDatabase(string noteName, string noteContents)
        {
            string query = "UPDATE notes SET name = @newName, contents = @newContents WHERE id = @noteID";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newName", noteName);
                        command.Parameters.AddWithValue("@newContents", noteContents);
                        command.Parameters.AddWithValue("@noteID", selectedNoteID);

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Note Edit Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void editNoteButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = noteNameTextBox.Text;
            TextRange textRange = new TextRange(noteContentsRichTextBox.Document.ContentStart, noteContentsRichTextBox.Document.ContentEnd);
            string newContents = textRange.Text;

            if (newContents == null)
            {
                MessageBox.Show("Please enter new contents.", "Note Edit Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (editNoteDatabase(newName, newContents))
            {
                MessageBox.Show("Note successfuly updated.", "Note Edit Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Hide();
            }
        }
    }
}
