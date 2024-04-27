using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.PreCompileData;
using Viper.Game.Menu;
using Viper.Game.Gameplay;
using Viper.Game.Gameplay.Managers;

namespace Viper.Game
{
    // The Game, needs the instance of the window so it can properly handle things like threads or any other resources after being closed.
    public class ViperGame(Window window)
    {
        // The main grid that holds the entire program.
        private Grid _viperContainer = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        // Compile date.
        private string _version = BuildData.DateTime;

        public string Version
        {
            get
            {
                return _version;
            }
        }

        // Dispatcher that will be passed as a parameter so threads can make changes to the UI thread.
        private Dispatcher _dispatcher = System.Windows.Application.Current.Dispatcher;

        // Main method to start the program somewhere.
        public Grid Start()
        {
            GameplayScreen gameplayScreen = new(window, _dispatcher);

            // New instance of the menu screen
            MenuScreen menuScreen = new(gameplayScreen);

            // Version footer that displays... the game version!, takes multiple forms depending on the configuration.
            TextBlock versionFooter = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

#if DEBUG
            versionFooter.Text = _version + "d - Development Build";
            versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 140, 140)); // Bright Red
#elif ALPHA
            versionFooter.Text = _version + "a - Alpha Build";
            versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 87, 87)); // Bright Yellow
#elif BETA
            versionFooter.Text = _version + "b - Beta Build";
            versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 255, 133)); // Bright Green
#elif RELEASE
            versionFooter.Text = _version + "s";
            versionFooter.Foreground = new SolidColorBrush(Color.FromArgb(255, 133, 208, 255)); // Bright blue
#endif


            _viperContainer.Children.Add(versionFooter);
            Panel.SetZIndex(versionFooter, 5);

            // Settings panel, not done yet.
            _viperContainer.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.S)
                {
                    MessageBox.Show("Playfield size: " + gameplayScreen.PlayfieldManager.Size.ToString() + Environment.NewLine + "Points: " + gameplayScreen.GameplayManager.Points + Environment.NewLine, "Settings test");
                }
            };

            // Show the menu screen as the first thing you see when the game is opened.
            _viperContainer.Children.Add(menuScreen.Show());

            return _viperContainer;
        }
    }
}
