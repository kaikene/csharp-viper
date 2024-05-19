using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Viper.Game.Screens;
using Viper.Screens;

namespace Viper.Game.Managers
{
    /// <summary>
    /// Manages screens in a better way.
    /// </summary>
    public class ScreenManager()
    {
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
        }

        // A history of each screen you went through, used to determine who is the previous screen when you want to go back.
        private List<Screens> _screenHistory = new();

        /// <summary>
        /// Shows the screen manager and sets events for all instances of the screens.
        /// </summary>
        public void Start()
        {
            // MenuScreen events.
            MenuScreen.PlayClicked += MenuScreen_PlayClicked;
            MenuScreen.SettingsClicked += MenuScreen_SettingsClicked;
            MenuScreen.ExitGame += MenuScreen_ExitGame;

            Application.Current.MainWindow.PreviewKeyDown += OnEscapeKeyPress;
        }

        private void MenuScreen_SettingsClicked(object? sender, EventArgs e)
        {
            MessageBox.Show("Settings still not added");
        }

        private void MenuScreen_PlayClicked(object? sender, EventArgs e)
        {
            ShowScreen(Screens.Gameplay);
        }

        private void MenuScreen_ExitGame(object? sender, EventArgs e)
        {
            ExitProgramDialog();
        }

        private void OnEscapeKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (_screenHistory.Count - 1 > 0)
                {
                    // Check who is the screen we need to go back.
                    if (_screenHistory[_screenHistory.Count - 2] == Screens.Menu)
                    {
                        ShowScreen(Screens.Menu);
                    }
                    else if (_screenHistory[_screenHistory.Count - 2] == Screens.Gameplay)
                    {
                        ShowScreen(Screens.Gameplay);
                    }
                    else if (_screenHistory[_screenHistory.Count - 2] == Screens.Testing)
                    {
                        ShowScreen(Screens.Testing);
                    }

                    // Remove everything from the screen you just left.
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
                    _screenHistory.RemoveAt(_screenHistory.Count - 1);
                }
                else // If theres no screens left to go back, just tell the user if they want to exit.
                {
                    ExitProgramDialog();
                }
            }
        }

        public void ShowScreen(Screens screen)
        {
            // If theres no screens, or theres a screen different from the one selected, then this screen can be shown.
            // If the current screen is already this one, then prevent it from showing it again.
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != screen)
            {
                if (screen == Screens.Menu)
                {
                    // If the screen was loaded, it means that its already on the screen history and is not cleared, so we dont need to add it again.
                    if (!MenuScreen.IsLoaded)
                    {
                        _screenHistory.Add(screen);
                    }

                    _screen.Children.Clear();
                    _screen.Children.Add(MenuScreen.Container);

                    MenuScreen.Show();
                }
                else if (screen == Screens.Gameplay)
                {
                    if (!GameplayScreen.IsLoaded)
                    {
                        _screenHistory.Add(screen);
                    }

                    _screen.Children.Clear();
                    _screen.Children.Add(GameplayScreen.Container);

                    GameplayScreen.Show();
                }
                else if (screen == Screens.Testing)
                {
                    if (!TestingScreen.IsLoaded)
                    {
                        _screenHistory.Add(screen);
                    }

                    _screen.Children.Clear();
                    _screen.Children.Add(TestingScreen.Container);

                    TestingScreen.Show();
                }
            }
        }

        public void CleanUp()
        {
            Application.Current.MainWindow.PreviewKeyDown -= OnEscapeKeyPress;
            MenuScreen.PlayClicked -= MenuScreen_PlayClicked;
            MenuScreen.ExitGame -= MenuScreen_ExitGame;
            GameplayScreen.Clear();
            MenuScreen.Clear();
            TestingScreen.Clear();
            _screen.Children.Clear();
            _screenHistory.Clear();
        }

        // Dialog used when theres no screens left and the user presses "Escape", or when a button for exiting is visible to the user.
        private void ExitProgramDialog()
        {
            MessageBoxResult result = MessageBox.Show("Estas seguro de que quieres salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CleanUp();
                Application.Current.Shutdown();
            }
        }
    }
}
