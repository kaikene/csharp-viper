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
        private Grid _viper = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        private Grid _unfocus = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Opacity = 0,
            IsHitTestVisible = false,
        };

        public Grid Game
        {
            get
            {
                return _viper;
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

        private OverlayManager _overlayManager = new();

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


            _viper.Children.Add(_versionFooter);
            _viper.Children.Add(_unfocus);
            Panel.SetZIndex(_versionFooter, 5);
            Panel.SetZIndex(_unfocus, 15);

            _screenManager.MenuScreen.PlayClicked += MenuScreen_PlayClicked;
            _screenManager.MenuScreen.SettingsClicked += MenuScreen_SettingsClicked;
            _screenManager.MenuScreen.ExitGame += MenuScreen_ExitGame;
            _screenManager.NoScreensLeft += NoScreensLeft;
            _viper.MouseEnter += _viper_MouseEnter;
            _viper.MouseLeave += _viper_MouseLeave;

            void NoScreensLeft(object sender, EventArgs e)
            {
                ExitProgramDialog();
            }

            void _viper_MouseLeave(object sender, MouseEventArgs e)
            {
                _unfocus.Opacity = 1;
                Application.Current.MainWindow.PreviewKeyDown -= OnKeyPress;
            }

            void _viper_MouseEnter(object sender, MouseEventArgs e)
            {
                _unfocus.Opacity = 0;
                Application.Current.MainWindow.PreviewKeyDown += OnKeyPress;
            }

            void MenuScreen_ExitGame(object? sender, EventArgs e)
            {
                ExitProgramDialog();
            }

            void MenuScreen_SettingsClicked(object? sender, EventArgs e)
            {
                // Settings logic here.
            }

            void MenuScreen_PlayClicked(object? sender, EventArgs e)
            {
                _screenManager.ShowScreen(ScreenManager.Screens.Gameplay);
            }

            void OnKeyPress(object sender, KeyEventArgs e)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (e.Key == Key.T)
                    {
                        _screenManager.ShowScreen(ScreenManager.Screens.Testing);
                    }

                    if (e.Key == System.Windows.Input.Key.S)
                    {
                        // Settings logic here.
                    }

                    if (e.Key == System.Windows.Input.Key.M)
                    {
                        _overlayManager.SampleOverlay.FlashMessage();
                    }
                }

                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    _screenManager.GoBack();
                }
            }

            _viper.Children.Add(_screenManager.Displayer);
            _viper.Children.Add(_overlayManager.Displayer);

            _screenManager.LoadScreens();
            _overlayManager.LoadOverlays();
            _screenManager.ShowScreen(ScreenManager.Screens.Menu);
        }

        private void ExitProgramDialog()
        {
            MessageBoxResult result = MessageBox.Show("Estas seguro de que quieres salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CleanUp();
                Application.Current.Shutdown();
            }
        }

        public void CleanUp()
        {
            _screenManager.End();
            _viper.Children.Clear();
        }
    }
}
