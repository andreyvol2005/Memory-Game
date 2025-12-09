using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace Game
{
    public partial class MainWindow : Window
    {
        public DispatcherTimer GameTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
        public bool isStart = false;
        public int pusition;
        public int max;
        public int count;
        public int score;
        List<int> memory = new List<int>();
        Random random = new Random();
        DropShadowEffect LightYellow = new DropShadowEffect
        {
            BlurRadius = 50,
            ShadowDepth = 0,
            Color = Colors.LightYellow
        };
        DropShadowEffect LightGray = new DropShadowEffect
        {
            BlurRadius = 0,
            ShadowDepth = 0,
            Color = Colors.LightGray
        };
        DropShadowEffect LightGreen = new DropShadowEffect
        {
            BlurRadius = 50,
            ShadowDepth = 0,
            Color = Colors.LightGreen
        };
        DropShadowEffect Red = new DropShadowEffect
        {
            BlurRadius = 50,
            ShadowDepth = 0,
            Color = Colors.Red
        };
        private MediaPlayer Bg = new MediaPlayer();
        private MediaPlayer Fg = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            GameTimer.Tick += GameTimer_Tick;
            ComboBox.Text = "3";
            LeftUpBotton.IsEnabled = false;
            UpBotton.IsEnabled = false;
            RightUpBotton.IsEnabled = false;
            LeftDownBotton.IsEnabled = false;
            DownBotton.IsEnabled = false;
            RightDownBotton.IsEnabled = false;
            Bg.Open(new Uri("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\NotStart.mp3"));
            Bg.MediaEnded += Bg_MediaEnded;
            Bg.Volume = 0.2;
            Bg.Play();
        }

        private void Bg_MediaEnded(object sender, EventArgs e)
        {
            Bg.Position = TimeSpan.Zero;
            Bg.Play();
        }

        private void Fg_MediaEnded(object sender, EventArgs e)
        {
            Fg.Position = TimeSpan.Zero;
            Fg.Play();
        }

        public void LeftUpBotton_Click(object sender, RoutedEventArgs e) { Tuping(1); }

        public void UpBotton_Click(object sender, RoutedEventArgs e) { Tuping(2); }

        public void RightUpBotton_Click(object sender, RoutedEventArgs e) { Tuping(3); }

        public void LeftDownBotton_Click(object sender, RoutedEventArgs e) { Tuping(4); }

        public void DownBotton_Click(object sender, RoutedEventArgs e) { Tuping(5); }

        public void RightDownBotton_Click(object sender, RoutedEventArgs e) { Tuping(6); }

        public void Tuping(int button)
        {
            if (button == memory[pusition] && isStart)
            {
                pusition++;
                PlaySound("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\Green.mp3");
                ChangeColor(Colors.LightGreen, LightGreen, button);
                if (pusition == max)
                {
                    max++;
                    score++;
                    count = 0;
                    pusition = 0;
                    Score.Content = "Счёт: " + score;
                    if (max == memory.Count + 1)
                    {
                        RestartButton.Content = "Начать";
                        isStart = false;
                        PlaySound("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\Win.mp3");
                        Bg.Open(new Uri("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\NotStart.mp3"));
                        Bg.MediaEnded += Bg_MediaEnded;
                        Bg.Volume = 0.2;
                        Fg.Stop();
                        Bg.Play();
                        MessageBox.Show("Вы победили!\nСо счётом " + score + ", поздравляем!");
                    }
                    else
                    {
                        GameTimer.Start();
                        LeftUpBotton.IsEnabled = false;
                        UpBotton.IsEnabled = false;
                        RightUpBotton.IsEnabled = false;
                        LeftDownBotton.IsEnabled = false;
                        DownBotton.IsEnabled = false;
                        RightDownBotton.IsEnabled = false;
                    }
                }
            }
            else
            {
                score = 0;
                Score.Content = "Счёт: " + score;
                if (isStart)
                {
                    PlaySound("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\Red.mp3");
                    ChangeColor(Colors.Red, Red, button);
                }
                isStart = false;
                RestartButton.Content = "Начать";
            }
        }

        public void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isStart)
            {
                max = 1;
                count = 0;
                score = 0;
                pusition = 0;
                Score.Content = "Счёт: " + score;
                memory.Clear();
                try
                {
                    for (int i = 0; i < int.Parse(ComboBox.Text); i++) { memory.Add(random.Next(1, 7)); }
                    RestartButton.Content = "Стоп";
                    isStart = true;
                    GameTimer.Start();
                    Fg.Open(new Uri("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\Start.mp3"));
                    Fg.MediaEnded += Fg_MediaEnded;
                    Fg.Volume = 0.2;
                    Bg.Stop();
                    Fg.Play();
                    LeftUpBotton.IsEnabled = false;
                    UpBotton.IsEnabled = false;
                    RightUpBotton.IsEnabled = false;
                    LeftDownBotton.IsEnabled = false;
                    DownBotton.IsEnabled = false;
                    RightDownBotton.IsEnabled = false;
                    ComboBox.IsEnabled = false;
                }
                catch { MessageBox.Show("Вы не выбрали уровень сложности!", "Ошибка!"); }
            }
            else
            {
                RestartButton.Content = "Начать";
                isStart = false;
                GameTimer.Stop();
                Bg.Open(new Uri("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\NotStart.mp3"));
                Bg.MediaEnded += Bg_MediaEnded;
                Bg.Volume = 0.2;
                Fg.Stop();
                Bg.Play();
                LeftUpBotton.IsEnabled = true;
                UpBotton.IsEnabled = true;
                RightUpBotton.IsEnabled = true;
                LeftDownBotton.IsEnabled = true;
                DownBotton.IsEnabled = true;
                RightDownBotton.IsEnabled = true;
                ComboBox.IsEnabled = true;
            }
        }

        public async void GameTimer_Tick(object sender, EventArgs e)
        {
            ChangeColor(Colors.Yellow, LightYellow, memory[count]);
            PlaySound("C:\\Users\\Andrey\\source\\repos\\Game\\Game\\Sounds\\Yellow.mp3");
            await Task.Delay(200);
            ChangeColor(Colors.LightGray, LightGray);
            if (count + 1 != max) count++;
            else
            {
                GameTimer.Stop();
                LeftUpBotton.IsEnabled = true;
                UpBotton.IsEnabled = true;
                RightUpBotton.IsEnabled = true;
                LeftDownBotton.IsEnabled = true;
                DownBotton.IsEnabled = true;
                RightDownBotton.IsEnabled = true;
            }
        }

        public async void ChangeColor(Color color, DropShadowEffect effect)
        {
            LeftUpBotton.Background = new SolidColorBrush(color);
            LeftUpBotton.Effect = effect;
            UpBotton.Background = new SolidColorBrush(color);
            UpBotton.Effect = effect;
            RightUpBotton.Background = new SolidColorBrush(color);
            RightUpBotton.Effect = effect;
            LeftDownBotton.Background = new SolidColorBrush(color);
            LeftDownBotton.Effect = effect;
            DownBotton.Background = new SolidColorBrush(color);
            DownBotton.Effect = effect;
            RightDownBotton.Background = new SolidColorBrush(color);
            RightDownBotton.Effect = effect;
            await Task.Delay(200);
            LeftUpBotton.Background = new SolidColorBrush(Colors.LightGray);
            LeftUpBotton.Effect = LightGray;
            UpBotton.Background = new SolidColorBrush(Colors.LightGray);
            UpBotton.Effect = LightGray;
            RightUpBotton.Background = new SolidColorBrush(Colors.LightGray);
            RightUpBotton.Effect = LightGray;
            LeftDownBotton.Background = new SolidColorBrush(Colors.LightGray);
            LeftDownBotton.Effect = LightGray;
            DownBotton.Background = new SolidColorBrush(Colors.LightGray);
            DownBotton.Effect = LightGray;
            RightDownBotton.Background = new SolidColorBrush(Colors.LightGray);
            RightDownBotton.Effect = LightGray;
        }

        private async void ChangeColor(Color color, DropShadowEffect effect, int botton)
        {
            switch (botton)
            {
                case 1:
                    LeftUpBotton.Background = new SolidColorBrush(color);
                    LeftUpBotton.Effect = effect; break;
                case 2:
                    UpBotton.Background = new SolidColorBrush(color);
                    UpBotton.Effect = effect; break;
                case 3:
                    RightUpBotton.Background = new SolidColorBrush(color);
                    RightUpBotton.Effect = effect; break;
                case 4:
                    LeftDownBotton.Background = new SolidColorBrush(color);
                    LeftDownBotton.Effect = effect; break;
                case 5:
                    DownBotton.Background = new SolidColorBrush(color);
                    DownBotton.Effect = effect; break;
                case 6:
                    RightDownBotton.Background = new SolidColorBrush(color);
                    RightDownBotton.Effect = effect; break;
            }
            await Task.Delay(200);
            switch (botton)
            {
                case 1:
                    LeftUpBotton.Background = new SolidColorBrush(Colors.LightGray);
                    LeftUpBotton.Effect = LightGray; break;
                case 2:
                    UpBotton.Background = new SolidColorBrush(Colors.LightGray);
                    UpBotton.Effect = LightGray; break;
                case 3:
                    RightUpBotton.Background = new SolidColorBrush(Colors.LightGray);
                    RightUpBotton.Effect = LightGray; break;
                case 4:
                    LeftDownBotton.Background = new SolidColorBrush(Colors.LightGray);
                    LeftDownBotton.Effect = LightGray; break;
                case 5:
                    DownBotton.Background = new SolidColorBrush(Colors.LightGray);
                    DownBotton.Effect = LightGray; break;
                case 6:
                    RightDownBotton.Background = new SolidColorBrush(Colors.LightGray);
                    RightDownBotton.Effect = LightGray; break;
            }
        }

        private void PlaySound(string soundUri)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(soundUri));
            mediaPlayer.Play();
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Это игра на память.\n" +
               "Здесь вам нужно запомнить последовательность загоревшихся кнопок и нажать их в этой последовательности.\n" +
               "Сначала вам нужно выбрать уровень сложности.\n" +
               "Далее при нажатии на кнопку \'Начать\', начнут загараться кнопки.\n" +
               "Каждый раз к последовательности будет добавляться ещё одна кнопка.\n" +
               "Если вы правильно нажали кнопки, вы получаете очко!\n" +
               "В противном случае очки обнуляются и вы начинаете с начала.", "Информация");
        }
    }
}