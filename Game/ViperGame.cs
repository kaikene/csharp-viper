using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.PreCompileData;
using Viper.Game.Screens;
using Viper.Game.Managers;
using System.Windows.Threading;
using System.Windows.Input;
using System.Runtime.Intrinsics;
using System.Windows.Media.Effects;
using System.Diagnostics;
using Viper.Game.Services;
using Viper.Game.Events;

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

        private ScreenSwitcher _screenManager = new();

        private OverlayLayer _overlayManager = new();

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

        private bool _canSetGlobalBackKeybind = true, _canSetCtrlKeybind = true;

        // Main method to start the game.
        public void Start()
        {
            SettingsHandler.CheckIntegrity();

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

            // Panel index reminder:
            // 1 = Above anything that doesnt have its index defined.
            // 2 to 4 = Above 1 but still below overlays.
            // 5 = Reserved for OverlayManager, should not be used by anything else.
            // 6 to 9 = For overlays or anything that needs to stay visible on screen, use these for simple notifications/pop-ups (6, 7, 8) or almost full screen dialogs that require attention (9).
            // 10+ = Above everything.
            Panel.SetZIndex(_versionSP, 11);

            _overlayManager.SettingsOverlay.StateChanged += OnStateChanged;
            _screenManager.MenuScreen.ButtonPanel.PlayClicked += MenuScreen_PlayClicked;
            _screenManager.MenuScreen.ButtonPanel.SettingsClicked += MenuScreen_SettingsClicked;
            _screenManager.MenuScreen.ButtonPanel.ExitClicked += MenuScreen_ExitGame;

            _screenManager.NoScreensLeft += NoScreensLeft;

            _viper.MouseEnter += _viper_MouseEnter;
            _viper.MouseLeave += _viper_MouseLeave;

            Application.Current.MainWindow.Deactivated += MainWindow_Deactivated;
            Application.Current.MainWindow.Activated += MainWindow_Activated;

            _viper.Children.Add(_screenManager.Displayer);
            _viper.Children.Add(_overlayManager.Displayer);

            _screenManager.LoadScreens();
            _overlayManager.LoadOverlays();
            _screenManager.ShowScreen(ScreenSwitcher.Screens.Menu);
        }

        private void OnStateChanged(object? sender, SettingsPanelStateChangedEventArgs e)
        {
            if (e.State)
            {
                Application.Current.MainWindow.PreviewKeyDown -= OnBackKeyPress;

                _canSetGlobalBackKeybind = false;

                _overlayManager.BlackoutOverlay.ShowBlackout(6);

                SetSettingsBackKeybind();
            }
            else
            {
                Application.Current.MainWindow.PreviewKeyDown -= OnSettingsBackKey;

                _canSetGlobalBackKeybind = true;

                _overlayManager.BlackoutOverlay.RemoveBlackout();

                SetGlobalBackKeybindEvents();
            }
        }

        private void MainWindow_Deactivated(object? sender, EventArgs e)
        {
            _overlayManager.BlackoutOverlay.ShowBlackout(10);
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            _overlayManager.BlackoutOverlay.RemoveBlackout();
        }

        private void NoScreensLeft(object sender, EventArgs e)
        {
            ExitProgramDialog();
        }

        private void _viper_MouseEnter(object sender, MouseEventArgs e)
        {
            SetGlobalBackKeybindEvents();
            SetSettingsBackKeybind();
            SetCtrlKeybinds();
        }

        private void _viper_MouseLeave(object sender, MouseEventArgs e)
        {
            RemoveGlobalEvents();
        }

        private void MenuScreen_ExitGame(object? sender, EventArgs e)
        {
            ExitProgramDialog();
        }

        private void MenuScreen_SettingsClicked(object? sender, EventArgs e)
        {
            _overlayManager.SettingsOverlay.SettingsToggle();
        }

        private void MenuScreen_PlayClicked(object? sender, EventArgs e)
        {
            _screenManager.ShowScreen(ScreenSwitcher.Screens.Gameplay);
        }

        // Global ctrl keybinds.
        private void OnCtrlKeyPress(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.T)
                {
                    _screenManager.ShowScreen(ScreenSwitcher.Screens.Testing);
                }

                if (e.Key == System.Windows.Input.Key.S)
                {
                    _overlayManager.SettingsOverlay.SettingsToggle();
                }

                if (e.Key == System.Windows.Input.Key.M)
                {

                }
            }
        }

        private void OnBackKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                _screenManager.GoBack();
            }
        }

        private void OnSettingsBackKey(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                _overlayManager.SettingsOverlay.SettingsToggle();
            }
        }

        private void SetGlobalBackKeybindEvents()
        {
            if (_canSetGlobalBackKeybind)
            {
                Application.Current.MainWindow.PreviewKeyDown += OnBackKeyPress;
            }
        }

        private void SetSettingsBackKeybind()
        {
            if (_overlayManager.SettingsOverlay.IsShowingSettings)
            {
                Application.Current.MainWindow.PreviewKeyDown += OnSettingsBackKey;
            }
        }

        private void SetCtrlKeybinds()
        {
            if (_canSetCtrlKeybind)
            {
                Application.Current.MainWindow.PreviewKeyDown += OnCtrlKeyPress;
            }
        }

        private void RemoveGlobalEvents()
        {
            Application.Current.MainWindow.PreviewKeyDown -= OnCtrlKeyPress;
            Application.Current.MainWindow.PreviewKeyDown -= OnBackKeyPress;
            Application.Current.MainWindow.PreviewKeyDown -= OnSettingsBackKey;
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
