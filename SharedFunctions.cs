using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoncasterUTCStudents
{
    public static class SharedFunctions
    {
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
    }
}
