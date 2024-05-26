using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Viper.Game.Animations;
using Viper.Game.Events;
using Viper.Game.Screens;
using Viper.Screens;

namespace Viper.Game.Managers
{
    /// <summary>
    /// Manages screens in a better way.
    /// </summary>
    public class ScreenManager
    {
        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public EventHandler<SMScreenChangedEventArgs>? ScreenChanged;

        public EventHandler<SMScreenHistoryChangedEventArgs>? HistoryChanged;

        public EventHandler? NoScreensLeft;

        /// <summary>
        /// Screen that shows the gameplay.
        /// </summary>
        public GameplayScreen GameplayScreen = new();

        /// <summary>
        /// Main menu screen.
        /// </summary>
        public MenuScreen MenuScreen = new();

        /// <summary>
        /// A screen that shows all elements in a list and lets you play with them.
        /// </summary>
        public TestingScreen TestingScreen = new();

        // The main container that will handle the screens (in a private way).
        private Grid _screen = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private TextBlock _startedText = new()
        {
            Text = "ScreenManager has been started, No screens loaded.",
            Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255)),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private Button _backButton = new()
        {
            Height = 40,
            Width = 60,
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(10, 0, 0, 10),
            Content = "Atras",
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Visibility = Visibility.Hidden,
        };

        /// <summary>
        /// The main container that will handle the screens.
        /// </summary>
        public Grid Displayer
        {
            get
            {
                return _screen;
            }
        }

        /// <summary>
        /// Used to identify the screens
        /// </summary>
        public enum Screens
        {
            Gameplay,
            Menu,
            Testing,
            None,
        }

        // A history of each screen you went through, used to determine who is the previous screen when you want to go back.
        private List<Screens> _screenHistory = new();

        /// <summary>
        /// Shows the screen manager and sets events for all instances of the screens.
        /// </summary>
        public void LoadScreens()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _backButton.Click += _backButton_Click;
                _backButton.MouseLeave += Element_MouseLeave;
                _backButton.MouseEnter += Element_MouseEnter;

                Panel.SetZIndex(_backButton, 1);

                _screen.Children.Add(_startedText);
                _screen.Children.Add(_backButton);
                _screen.Children.Add(MenuScreen.Container);
                _screen.Children.Add(GameplayScreen.Container);
                _screen.Children.Add(TestingScreen.Container);
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

        private void _backButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        public void GoBack()
        {
            if (_screenHistory.Count - 1 > 0)
            {
                // Check who is the screen we need to go back.
                if (_screenHistory[_screenHistory.Count - 2] == Screens.Menu)
                {
                    ShowScreen(Screens.Menu, true);
                }
                else if (_screenHistory[_screenHistory.Count - 2] == Screens.Gameplay)
                {
                    ShowScreen(Screens.Gameplay, true);
                }
                else if (_screenHistory[_screenHistory.Count - 2] == Screens.Testing)
                {
                    ShowScreen(Screens.Testing, true);
                }

                _screenHistory.RemoveAt(_screenHistory.Count - 1);
                HistoryChanged?.Invoke(this, new SMScreenHistoryChangedEventArgs(_screenHistory.Count));

                if (_screenHistory.Count == 1)
                {
                    _backButton.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                NoScreensLeft?.Invoke(this, new EventArgs());
            }
        }

        public void ShowScreen(Screens screen, bool isReturning = false)
        {
            if (_isInitialized)
            {
                _startedText.Visibility = Visibility.Hidden;

                // If theres no screens, or theres a screen different from the one selected, then this screen can be shown.
                // If the current screen is already this one, then prevent it from showing it again.
                if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != screen)
                {
                    ScreenChanged?.Invoke(this, new SMScreenChangedEventArgs(screen));

                    // Remove the screen being shown before showing a new screen. 
                    if (_screenHistory.Count > 0)
                    {
                        if (_screenHistory[_screenHistory.Count - 1] == Screens.Menu)
                        {
                            MenuScreen.Clear();
                        }
                        else if (_screenHistory[_screenHistory.Count - 1] == Screens.Gameplay)
                        {
                            GameplayScreen.Clear();
                        }
                        else if (_screenHistory[_screenHistory.Count - 1] == Screens.Testing)
                        {
                            TestingScreen.Clear();
                        }
                    }

                    // Show the selected screen.
                    if (screen == Screens.Menu)
                    {
                        MenuScreen.Show();
                    }
                    else if (screen == Screens.Gameplay)
                    {
                        GameplayScreen.Show();
                    }
                    else if (screen == Screens.Testing)
                    {
                        TestingScreen.Show();
                    }

                    // If we are returning to this screen, then dont add it to the history.
                    if (!isReturning)
                    {
                        _screenHistory.Add(screen);
                        HistoryChanged?.Invoke(this, new SMScreenHistoryChangedEventArgs(_screenHistory.Count));
                    }
                }

                if (_screenHistory.Count > 1)
                {
                    _backButton.Visibility = Visibility.Visible;
                }
            }
        }

        public void End()
        {
            GameplayScreen.Clear();
            MenuScreen.Clear();
            TestingScreen.Clear();
            _screen.Children.Clear();
            _screenHistory.Clear();

            _startedText.Visibility = Visibility.Visible;

            _isInitialized = false;

            _backButton.Click -= _backButton_Click;
            _backButton.MouseLeave -= Element_MouseLeave;
            _backButton.MouseEnter -= Element_MouseEnter;

            HistoryChanged?.Invoke(this, new SMScreenHistoryChangedEventArgs(_screenHistory.Count));
            ScreenChanged?.Invoke(this, new SMScreenChangedEventArgs(Screens.None));
        }
    }
}
