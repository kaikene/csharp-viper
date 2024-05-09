using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Screens;

namespace Viper.Game.Managers
{
    public class ScreenManager()
    {
        public GameplayScreen GameplayScreen = new();

        public MenuScreen MenuScreen = new();

        private Grid _screen = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public Grid Start()
        {
            MenuScreen.PlayClicked += (s, e) =>
            {
                ShowGameplay();
            };

            MenuScreen.ExitGame += (s, e) =>
            {
                ExitGameDialog();
            };

            GameplayScreen.GoBack += (s, e) =>
            {
                ShowMainMenu();
            };

            return _screen;
        }

        public void ShowMainMenu()
        {
            _screen.Children.Clear();
            _screen.Children.Add(MenuScreen.Show());
        }

        public void ShowGameplay()
        {
            _screen.Children.Clear();
            _screen.Children.Add(GameplayScreen.Show());
        }

        private void ExitGameDialog()
        {
            MessageBoxResult result = MessageBox.Show("Estas seguro de que quieres salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
