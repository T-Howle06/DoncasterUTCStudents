using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Security.Policy;
using System.Data.SqlClient;

namespace DoncasterUTCStudents
{
    public static class SharedFunctions
    {
        public class TaskItem
        {
            public string? TaskName { get; set; }
            public DateTime? DueDate { get; set; }
        }

        public class NoteItem
        {
            public string? NoteName { get; set; }
            public string? Content { get; set; }
        }

        public static string connectionString = "Data Source=DESKTOP-RVQ5B1D\\SQLEXPRESS;Initial Catalog=DoncasterUTC;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public static string hashPassword(string password)
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

        public static bool validLogin(TextBox usernameTextBox, PasswordBox passwordPasswordBox, string username, string password)
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

        public static string getCurrentUser()
        {
            string? currentUser = null;

            string query = "SELECT TOP 1 username FROM log ORDER BY time DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check if the 'username' column exists in your table
                            int usernameColumnIndex = reader.GetOrdinal("username");
                            if (!reader.IsDBNull(usernameColumnIndex))
                            {
                                currentUser = reader.GetString(usernameColumnIndex);
                            }
                        }
                    }
                }
            }

            return currentUser;
        }
    }
}
