using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Viper.Game.Screens
{
    // The menu screen of the game, grants access to basically all things of the game, needs instance of the
    public class MenuScreen
    {
        public event EventHandler PlayClicked;

        public event EventHandler ExitGame;

        private Grid _menu = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Focusable = true,
        };

        public Grid Menu { get { return _menu; } }

        public void CleanUp()
        {
            _menu.Children.Clear();
        }

        public void Show()
        {
            const int BUTTON_STACKPANEL_WIDTH = 250;

            const int BUTTON_SEPARATION = 12;

            const int BUTTON_HEIGHT = 25;

            TextBlock screenIdetifier = new()
            {
                Text = "Viper.Game.Screens.MenuScreen",
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(70, 255, 255, 255)),
                Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
            };

            Panel.SetZIndex(screenIdetifier, 5);

            StackPanel buttonStackPanel = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0,0,0,-100),
                Width = BUTTON_STACKPANEL_WIDTH,
            };

            Button startButton = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0,0,0,BUTTON_SEPARATION),
                Content = "Jugar",
                Height = BUTTON_HEIGHT,
            };

            Button settingsButton = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, BUTTON_SEPARATION),
                Content = "Ajustes",
                Height = BUTTON_HEIGHT,
            };

            Button exitButton = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, BUTTON_SEPARATION),
                Content = "Salir",
                Height = BUTTON_HEIGHT,
            };

            buttonStackPanel.Children.Add(startButton);

            buttonStackPanel.Children.Add(settingsButton);

            buttonStackPanel.Children.Add(exitButton);

            _menu.Children.Add(buttonStackPanel);

            _menu.Children.Add(screenIdetifier);

            startButton.PreviewMouseUp += (s, e) =>
            {
                PlayClicked.Invoke(this, e);
            };

            exitButton.PreviewMouseUp += (s, e) =>
            {
                ExitGame.Invoke(this, e);
            };

            _menu.Loaded += (s, e) =>
            {
                startButton.Focus();
            };
        }
    }
}
