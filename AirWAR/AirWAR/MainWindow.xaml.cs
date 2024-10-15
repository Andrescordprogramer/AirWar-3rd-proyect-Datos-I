using System;
using System.Windows;
using System.Windows.Threading;

namespace AirWAR
{
    public partial class MainWindow : Window
    {
        private int score;
        private DispatcherTimer timer;
        private int timeLeft;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            score = 0;
            timeLeft = 30; // Duración del juego en segundos
            ScoreText.Text = "Puntuación: 0";
            TimerText.Text = "Tiempo: 30";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerText.Text = $"Tiempo: {timeLeft}";

            if (timeLeft <= 0)
            {
                timer.Stop();
                GameButton.IsEnabled = false;
                MessageBox.Show($"Juego terminado! Puntuación final: {score}");
            }
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            score++;
            ScoreText.Text = $"Puntuación: {score}";

            Random rand = new Random();
            double newX = rand.NextDouble() * (this.ActualWidth - GameButton.Width);
            double newY = rand.NextDouble() * (this.ActualHeight - GameButton.Height);

            GameButton.Margin = new Thickness(newX, newY, 0, 0);
        }
    }
}
