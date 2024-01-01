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
using System.Windows.Threading;

namespace DoncasterUTCStudents
{
    /// <summary>
    /// Interaction logic for TimerScreen.xaml
    /// </summary>
    public partial class TimerScreen : Window
    {
        private DispatcherTimer timer;
        private int pomodoroDuration = 25;
        private int breakDuration = 5;
        private int secondsRemaining;

        public TimerScreen()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (secondsRemaining > 0)
            {
                secondsRemaining--;
                updateTimerText();
            }
            else
            {
                timer.Stop();

                if (pomodoroDuration > 0)
                {
                    MessageBox.Show("Pomodoro completed! Take a break.");
                    secondsRemaining = breakDuration * 60;
                    pomodoroDuration = 0;
                }
                else
                {
                    MessageBox.Show("Break completed! Start Pomodoro.");
                    secondsRemaining = pomodoroDuration * 60;
                    pomodoroDuration = 25; // Reset to Pomodoro duration
                }

                updateTimerText();
                timer.Start();
            }
        }

        private void updateTimerText()
        {
            int minutes = secondsRemaining / 60;
            int seconds = secondsRemaining % 60;
            timerText.Text = $"{minutes:D2}:{seconds:D2}";
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen homeScreen = new HomeScreen();
            homeScreen.Show();
            this.Hide();
        }

        private void startTimerButton_Click(object sender, RoutedEventArgs e)
        {
            secondsRemaining = pomodoroDuration * 60;
            updateTimerText();
            timer.Start();
        }

        private void pauseTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void resetTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            secondsRemaining = 0;
            updateTimerText();
        }
    }
}
