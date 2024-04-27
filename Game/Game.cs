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
        Dispatcher dispatcher = System.Windows.Application.Current.Dispatcher;

        // Main method to start the program somewhere.
        public Grid Start()
        {
            PlayfieldManager pm = new();

            GameManager gm = new(window, dispatcher);

            GameplayScreen gs = new(window, dispatcher, pm, gm);

            // New instance of the menu screen
            MenuScreen ms = new(window, dispatcher, gs);

            // Version footer that displays... the game version!, takes multiple forms depending on the configuration.
            TextBlock versionMessage = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

#if DEBUG
            versionMessage.Text = _version + "d - Development Build";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 140, 140)); // Bright Red
#elif ALPHA
            versionMessage.Text = _version + "a - Alpha Build";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 87, 87)); // Bright Yellow
#elif BETA
            versionMessage.Text = _version + "b - Beta Build";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 255, 133)); // Bright Green
#elif RELEASE
            versionMessage.Text = _version + "s";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 133, 208, 255)); // Bright blue
#endif


            _viperContainer.Children.Add(versionMessage);
            Panel.SetZIndex(versionMessage, 5);

            // Settings panel, not done yet.
            _viperContainer.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.S)
                {
                    MessageBox.Show("Playfield size: " + pm.Size.ToString() + Environment.NewLine + "Points: " + gm.PlayerPoints[0] + Environment.NewLine, "Settings test");
                }
            };

            // Show the menu screen as the first thing you see when the game is opened.
            _viperContainer.Children.Add(ms.Show());

            return _viperContainer;
        }
    }
}
