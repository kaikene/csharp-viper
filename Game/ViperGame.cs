using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.PreCompileData;
using Viper.Game.Screens;
using Viper.Game.Managers;
using System.Windows.Threading;
using System.Windows.Input;

namespace Viper.Game
{
    public class ViperGame()
    {
        // The main grid that holds the entire program.
        private Grid _viperContainer = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        public Grid GameContainer
        {
            get
            {
                return _viperContainer;
            }
        }

        // Compile date.
        private string _version = BuildData.DateTime;

        public string Version
        {
            get
            {
                return _version;
            }
        }

        private ScreenManager _screenManager = new();

        // Version footer that displays... the game version!, takes multiple forms depending on the configuration.
        private TextBlock _versionFooter = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
            Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        // Main method to start the program somewhere.
        public void Start()
        {
#if DEBUG
            _versionFooter.Text = _version + "d - Development Build";
            _versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 140, 140)); // Bright Red
#elif ALPHA
            _versionFooter.Text = _version + "a - Alpha Build";
            _versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 87, 87)); // Bright Yellow
#elif BETA
            _versionFooter.Text = _version + "b - Beta Build";
            _versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 255, 133)); // Bright Green
#elif RELEASE
            _versionFooter.Text = _version + "s";
            _versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 133, 208, 255)); // Bright blue
#endif


            _viperContainer.Children.Add(_versionFooter);
            Panel.SetZIndex(_versionFooter, 5);

            // Settings panel, not done yet.
            _viperContainer.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.S)
                {

                }
            };

            // Show TestingScreen.
            Application.Current.MainWindow.PreviewKeyDown += OnCtrlTKeyPress;

            void OnCtrlTKeyPress(object sender, KeyEventArgs e)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (e.Key == Key.T)
                    {
                        _screenManager.ShowTesting();
                    }
                }
            }

            _viperContainer.Children.Add(_screenManager.ScreenContainer);

            _screenManager.Start();
            _screenManager.ShowMainMenu();
        }

        public void CleanUp()
        {
            _screenManager.CleanUp();
            _viperContainer.Children.Clear();
        }
    }
}
