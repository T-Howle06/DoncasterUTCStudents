using System;
using System.Collections.Generic;
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
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void tasksButton_Click(object sender, RoutedEventArgs e)
        {
            TasksScreen tasksScreen = new TasksScreen();
            tasksScreen.Show();
            this.Close();
        }

        private void notesButton_Click(object sender, RoutedEventArgs e)
        {
            NotesScreen notesScreen = new NotesScreen();
            notesScreen.Show();
            this.Close();
        }

        private void timerButton_Click(object sender, RoutedEventArgs e)
        {
            TimerScreen timerScreen = new TimerScreen();
            timerScreen.Show();
            this.Close();
        }

        private void timetableButton_Click(object sender, RoutedEventArgs e)
        {
            TimetableScreen timetableScreen = new TimetableScreen();
            timetableScreen.Show();
            this.Close();
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
