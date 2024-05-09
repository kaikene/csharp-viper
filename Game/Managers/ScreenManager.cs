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
    public class ScreenManager()
    {
        public GameplayScreen GameplayScreen = new();

        public MenuScreen MenuScreen = new();

        public TestingScreen TestingScreen = new();

        private Grid _screen = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private enum Screens
        {
            Gameplay,
            Menu,
            Testing,
        }

        private List<Screens> _screenHistory = new();

        public Grid Start()
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

            return _screen;
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
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != Screens.Menu)
            {
                AddToScreenHistory(!isReturningHere, Screens.Menu);

                _screen.Children.Clear();
                _screen.Children.Add(MenuScreen.MenuContainer);

                if (!isReturningHere)
                {
                    MenuScreen.Show();
                }
            }
        }

        public void ShowGameplay(bool isReturningHere = false)
        {
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != Screens.Gameplay)
            {
                AddToScreenHistory(!isReturningHere, Screens.Gameplay);

                _screen.Children.Clear();
                _screen.Children.Add(GameplayScreen.GameplayContainer);

                if (!isReturningHere)
                {
                    GameplayScreen.Show();
                }
            }
        }

        public void ShowTesting(bool isReturningHere = false)
        {
            if (_screenHistory.Count == 0 || _screenHistory[_screenHistory.Count - 1] != Screens.Testing)
            {
                AddToScreenHistory(!isReturningHere, Screens.Testing);

                _screen.Children.Clear();
                TestingScreen.CleanUp();
                _screen.Children.Add(TestingScreen.TestingContainer);

                if (!isReturningHere)
                {
                    TestingScreen.Show();
                }
            }
        }

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
