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
    /// Interaction logic for NotesScreen.xaml
    /// </summary>
    public partial class NotesScreen : Window
    {
        private ObservableCollection<SharedFunctions.NoteItem> notes;

        public NotesScreen()
        {
            InitializeComponent();
            notes = new ObservableCollection<SharedFunctions.NoteItem>();
            notesListBox.ItemsSource = notes;
            populateNotes();
        }

        private void populateNotes()
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

                            notes.Add(new SharedFunctions.NoteItem { NoteName = noteName, Content = noteContent });
                        }
                    }
                }
            }
        }

        private int getCurrentNoteID(SharedFunctions.NoteItem note)
        {
            string query = "SELECT id FROM notes WHERE username = @username AND name = @name";

            string currentUser = SharedFunctions.getCurrentUser();

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", currentUser);
                        command.Parameters.AddWithValue("@name", note.NoteName);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            throw new Exception("Note not found for the given user and name.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Note ID Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private bool deleteNote(int noteID)
        {
            string query = "DELETE FROM notes WHERE id = @noteId";

            try
            {
                using (SqlConnection connection = new SqlConnection(SharedFunctions.connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@noteId", noteID);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Note Deletion Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void addNewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            NewNoteScreen newNoteScreen = new NewNoteScreen();

            bool? result = newNoteScreen.ShowDialog();

            if (result == true)
            {
                notes.Clear();
                notesListBox.ItemsSource = notes;
                populateNotes();
            }
        }

        private void editNoteButton_Click(object sender, RoutedEventArgs e)
        {
            SharedFunctions.NoteItem? selectedNote = notesListBox.SelectedItem as SharedFunctions.NoteItem;

            if (selectedNote != null)
            {
                int noteID = getCurrentNoteID(selectedNote);

                EditNoteScreen editNoteScreen = new EditNoteScreen(noteID);

                bool? result = editNoteScreen.ShowDialog();

                if (result == true)
                {
                    notes.Clear();
                    notesListBox.ItemsSource = notes;
                    populateNotes();
                }
            }
            else
            {
                MessageBox.Show("No note selected.", "Note Edit Error.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            SharedFunctions.NoteItem? selectedNote = notesListBox.SelectedItem as SharedFunctions.NoteItem;

            if (selectedNote != null)
            {
                int noteID = getCurrentNoteID(selectedNote);
                if (deleteNote(noteID))
                {
                    MessageBox.Show("Note deleted.", "Note Deletion Successful.", MessageBoxButton.OK, MessageBoxImage.Information);

                    notes.Clear();
                    notesListBox.ItemsSource = notes;
                    populateNotes();
                }
                else
                {
                    MessageBox.Show("Note not deleted.", "Note Deletion Error.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No note selected.", "Note Deletion Error.", MessageBoxButton.OK, MessageBoxImage.Error);
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
