using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Viper.Game.Animations;
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

        private bool _isHidden = false;

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool IsHidden { get { return _isHidden; } }

        /// <summary>
        /// Loads and shows the menu elements.
        /// </summary>
        public void Show()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
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
                    Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                    Foreground = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(2, 2, 2, 2),
                    RenderTransform = new TranslateTransform(30, 0),
                    Height = BUTTON_HEIGHT,
                    Opacity = 0,
                };

                Button settingsButton = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0, 0, 0, BUTTON_SEPARATION),
                    Content = "Ajustes",
                    Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                    Foreground = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(2, 2, 2, 2),
                    RenderTransform = new TranslateTransform(30, 0),
                    Height = BUTTON_HEIGHT,
                    Opacity = 0,
                };

                Button exitButton = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0, 0, 0, BUTTON_SEPARATION),
                    Content = "Salir",
                    Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                    Foreground = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(2, 2, 2, 2),
                    RenderTransform = new TranslateTransform(30, 0),
                    Height = BUTTON_HEIGHT,
                    Opacity = 0,
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

                startButton.MouseEnter += Element_MouseEnter;
                startButton.MouseLeave += Element_MouseLeave;

                settingsButton.MouseEnter += Element_MouseEnter;
                settingsButton.MouseLeave += Element_MouseLeave;

                exitButton.MouseEnter += Element_MouseEnter;
                exitButton.MouseLeave += Element_MouseLeave;

                async void ButtonApearAnimation()
                {
                    Animate.Position(startButton, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                    Animate.Opacity(startButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);

                    await Task.Delay(20);

                    Animate.Position(settingsButton, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                    Animate.Opacity(settingsButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);

                    await Task.Delay(20);

                    Animate.Position(exitButton, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                    Animate.Opacity(exitButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                }

                ButtonApearAnimation();

                _menu.Children.Add(buttonStackPanel);
            }
            else if (_isHidden)
            {
                _menu.Visibility = Visibility.Visible;
                _menu.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            Animate.Color((Button)sender, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            Animate.Color((Button)sender, Animate.ColorProperty.Foreground, Colors.Black, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
        }

        public void Hide()
        {
            if (_isInitialized)
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
            _isInitialized = false;
            _menu.IsHitTestVisible = false;
        }
    }
}
