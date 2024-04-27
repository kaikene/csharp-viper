using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Gameplay;
using Viper.Game.Gameplay.Managers;

namespace Viper.Game.Menu
{
    // The menu screen of the game, grants access to basically all things of the game, needs instance of the
    public class MenuScreen(Window window, Dispatcher dispatcher, GameplayScreen gs)
    {
        public Grid Show()
        {
            Grid menuScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Focusable = true,
            };

            Button startButton = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Height = 40,
                Width = 60,
                Content = "Jugar"
            };

            menuScreen.Children.Add(startButton);

            startButton.PreviewMouseDown += StartButton_PreviewMouseDown;

            void StartButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                (menuScreen.Parent as Panel).Children.Add(gs.Show(20));
                (menuScreen.Parent as Panel).Children.Remove(menuScreen);
            }

            return menuScreen;
        }
    }
}
