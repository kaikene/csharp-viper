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
    public class ScreenManager(GameplayScreen gs, MenuScreen ms)
    {
        private Grid _screen = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public Grid Start()
        {
            ms.PlayClicked += (s, e) =>
            {
                ShowGameplay();
            };

            ms.ExitGame += (s, e) =>
            {
                ExitGameDialog();
            };

            gs.GoBack += (s, e) =>
            {
                ShowMainMenu();
            };

            return _screen;
        }

        public void ShowMainMenu()
        {
            _screen.Children.Clear();
            _screen.Children.Add(ms.Show());
        }

        public void ShowGameplay()
        {
            gs.GameplayManager.CleanUp();
            _screen.Children.Clear();
            _screen.Children.Add(gs.Show());
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
