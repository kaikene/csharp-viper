using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.PreCompileData;
using Viper.Game.Screens;
using Viper.Game.Managers;
using System.Windows.Threading;

namespace Viper.Game
{
    public class ViperGame()
    {
        // The main grid that holds the entire program.
        public Grid RootScreen = new()
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

        public GameplayScreen GameplayScreen = new();

        public MenuScreen MenuScreen = new();

        // Main method to start the program somewhere.
        public Grid Start()
        {
            ScreenManager ScreenManager = new(GameplayScreen, MenuScreen);

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


            RootScreen.Children.Add(versionFooter);
            Panel.SetZIndex(versionFooter, 5);

            // Settings panel, not done yet.
            RootScreen.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.S)
                {

                }
            };

            RootScreen.Children.Add(ScreenManager.Start());

            ScreenManager.ShowMainMenu();

            return RootScreen;
        }
    }
}
