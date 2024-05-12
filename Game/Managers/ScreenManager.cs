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
        public Grid Screen
        {
            get
            {
                return _screen;
            }
        }

        /// <summary>
        /// Used to identify the screens
        /// </summary>
        private enum Screens
        {
            Gameplay,
            Menu,
            Testing,
        }

        // A history of each screen you went through, used to determine who is the previous screen when you want to go back.
        private List<Screens> _screenHistory = new();

        // Shows the screen manager and sets events for all instances of the screens.
        public void Start()
        {
            MenuScreen.PlayClicked += (s, e) =>
            {
                ShowGameplay();
            };

            MenuScreen.ExitGame += (s, e) =>
            {
                ExitProgramDialog();
            };

            Application.Current.MainWindow.PreviewKeyDown += OnEscapeKeyPress;
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
                        ShowMainMenu(true);
                    }
                    else if (_screenHistory[_screenHistory.Count - 2] == Screens.Gameplay)
                    {
                        ShowGameplay(true);
                    }
                    else if (_screenHistory[_screenHistory.Count - 2] == Screens.Testing)
                    {
                        ShowTesting(true);
                    }

                    // Remove everything from the screen you just left.
                    if (_screenHistory[_screenHistory.Count - 1] == Screens.Menu)
                    {
                        MenuScreen.CleanUp();
                    }
                    else if (_screenHistory[_screenHistory.Count - 1] == Screens.Gameplay)
                    {
                        GameplayScreen.CleanUp();
                    }
                    else if (_screenHistory[_screenHistory.Count - 1] == Screens.Testing)
                    {
                        TestingScreen.CleanUp();
                    }
                    _screenHistory.RemoveAt(_screenHistory.Count - 1);
                }
                else
                {
                    ExitProgramDialog();
                }
            }
        }

        public void ShowMainMenu(bool isReturningHere = false)
        {
            // If theres no screens, then showing this one is possible.
            // If the current screen is already this one, then prevent it from showing it again.
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != Screens.Menu)
            {
                AddToScreenHistory(!isReturningHere, Screens.Menu);

                _screen.Children.Clear();
                _screen.Children.Add(MenuScreen.Menu);

                if (!isReturningHere)
                {
                    MenuScreen.Show();
                }
            }
        }

        public void ShowGameplay(bool isReturningHere = false)
        {
            // If theres no screens, then showing this one is possible.
            // If the current screen is already this one, then prevent it from showing it again.
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != Screens.Gameplay)
            {
                AddToScreenHistory(!isReturningHere, Screens.Gameplay);

                _screen.Children.Clear();
                _screen.Children.Add(GameplayScreen.Gameplay);

                if (!isReturningHere)
                {
                    GameplayScreen.Show();
                }
            }
        }

        public void ShowTesting(bool isReturningHere = false)
        {
            // If theres no screens, then showing this one is possible.
            // If the current screen is already this one, then prevent it from showing it again.
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != Screens.Testing)
            {
                AddToScreenHistory(!isReturningHere, Screens.Testing);

                _screen.Children.Clear();
                TestingScreen.CleanUp();
                _screen.Children.Add(TestingScreen.Testing);

                // If you are returning here, then the screen elements dont have to be reloaded
                // but if you arent returning, then they have to be loaded otherwise you will just be watching a gray screen
                if (!isReturningHere)
                {
                    TestingScreen.Show();
                }
            }
        }

        // Adds screens to the screen history
        private void AddToScreenHistory(bool canSave, Screens screenToSave)
        {
            if (canSave)
            {
                Screens currrentScreen = screenToSave;
                _screenHistory.Add(currrentScreen);
            }
        }

        public void CleanUp()
        {
            Application.Current.MainWindow.PreviewKeyDown -= OnEscapeKeyPress;
            GameplayScreen.CleanUp();
            MenuScreen.CleanUp();
            TestingScreen.CleanUp();
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
