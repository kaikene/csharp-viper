using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.PreCompileData;
using Viper.Game.Screens;
using Viper.Game.Managers;
using System.Windows.Threading;
using System.Windows.Input;
using System.Runtime.Intrinsics;

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
        private string _build = BuildData.DateTime;

        private StackPanel _versionSP = new()
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Center,

        };

        public string Version
        {
            get
            {
                return _build;
            }
        }

        private ScreenManager _screenManager = new();

        private OverlayManager _overlayManager = new();

        private TextBlock _buildFooter = new()
        {
            Foreground = new SolidColorBrush(Color.FromArgb(255, 10, 10, 10)),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 12,
            FontWeight = FontWeights.Bold,
            Height = 17,
        };

        private TextBlock _buildText = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
            Foreground = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255)),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 11,
            Height = 17,
        };

        // Main method to start the game.
        public void Start()
        {
            _buildText.Text = " Build ";
#if DEBUG
            _buildFooter.Text = $" {_build}d ";
            _buildFooter.Background = new SolidColorBrush(Color.FromArgb(130, 255, 123, 74));
#elif RELEASE
            _buildFooter.Text = $" {_build}s ";
            _buildFooter.Background = new SolidColorBrush(Color.FromArgb(80, 105, 220, 255));
#endif
            _versionSP.Children.Add(_buildText);
            _versionSP.Children.Add(_buildFooter);
            _viper.Children.Add(_versionSP);
            _viper.Children.Add(_unfocus);

            // Panel index reminder:
            // 1 = Above anything that doesnt have its index defined.
            // 2 to 4 = Above 1 but still below overlays.
            // 5 = Reserved for OverlayManager, should not be used by anything else.
            // 6 to 9 = For overlays or anything that needs to stay visible on screen, use these for simple notifications/pop-ups (6, 7, 8) or almost full screen dialogs that require attention (9).
            // 10+ = Above everything.
            Panel.SetZIndex(_versionSP, 11);
            Panel.SetZIndex(_unfocus, 10);

            _screenManager.MenuScreen.PlayClicked += MenuScreen_PlayClicked;
            _screenManager.MenuScreen.SettingsClicked += MenuScreen_SettingsClicked;
            _screenManager.MenuScreen.ExitGame += MenuScreen_ExitGame;
            _screenManager.NoScreensLeft += NoScreensLeft;
            _viper.MouseEnter += _viper_MouseEnter;
            _viper.MouseLeave += _viper_MouseLeave;
            Application.Current.MainWindow.Deactivated += MainWindow_Deactivated;
            Application.Current.MainWindow.Activated += MainWindow_Activated;

            void MainWindow_Deactivated(object? sender, EventArgs e)
            {
                _unfocus.Opacity = 1;
            }

            void MainWindow_Activated(object? sender, EventArgs e)
            {
                _unfocus.Opacity = 0;
            }

            void NoScreensLeft(object sender, EventArgs e)
            {
                ExitProgramDialog();
            }

            void _viper_MouseLeave(object sender, MouseEventArgs e)
            {
                Application.Current.MainWindow.PreviewKeyDown -= OnKeyPress;
            }

            void _viper_MouseEnter(object sender, MouseEventArgs e)
            {
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
