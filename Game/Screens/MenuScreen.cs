using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Viper.Game.Interfaces;

namespace Viper.Game.Screens
{
    /// <summary>
    /// The menu screen of the game.
    /// </summary>
    public class MenuScreen : IScreenStates
    {
        /// <summary>
        /// Triggers when the user clicks "Play"
        /// </summary>
        public event EventHandler? PlayClicked;

        /// <summary>
        /// Triggers when the user clicks "Settings"
        /// </summary>
        public event EventHandler? SettingsClicked;

        /// <summary>
        /// Triggers when the user clicks Exit
        /// </summary>
        public event EventHandler? ExitGame;

        // Container that handles all menu elements.
        private Grid _menu = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsHitTestVisible = false,
            Visibility = Visibility.Hidden,
        };

        /// <summary>
        /// Container that handles all menu elements.
        /// </summary>
        public Grid Container { get { return _menu; } }

        private bool _isLoaded = false, _isHidden = false;

        public bool IsLoaded { get { return _isLoaded; } }

        public bool IsHidden { get { return _isHidden; } }

        /// <summary>
        /// Loads and shows the menu elements.
        /// </summary>
        public void Show()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                _menu.IsHitTestVisible = true;
                _menu.Visibility = Visibility.Visible;

                const int BUTTON_STACKPANEL_WIDTH = 250;

                const int BUTTON_SEPARATION = 12;

                const int BUTTON_HEIGHT = 25;

                StackPanel buttonStackPanel = new()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, -100),
                    Width = BUTTON_STACKPANEL_WIDTH,
                };

                Button startButton = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0, 0, 0, BUTTON_SEPARATION),
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

                startButton.PreviewMouseUp += (s, e) =>
                {
                    PlayClicked?.Invoke(this, e);
                };

                settingsButton.PreviewMouseUp += (s, e) =>
                {
                    SettingsClicked?.Invoke(this, e);
                };

                exitButton.PreviewMouseUp += (s, e) =>
                {
                    ExitGame?.Invoke(this, e);
                };

                _menu.Children.Add(buttonStackPanel);
            }
            else if (_isHidden)
            {
                _menu.Visibility = Visibility.Visible;
                _menu.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        public void Hide()
        {
            if (_isLoaded)
            {
                _menu.Visibility = Visibility.Hidden;
                _menu.IsHitTestVisible = false;
                _isHidden = true;
            }
        }

        public void Clear()
        {
            _menu.Children.Clear();
            _menu.Visibility = Visibility.Hidden;
            _isLoaded = false;
            _menu.IsHitTestVisible = false;
        }
    }
}
