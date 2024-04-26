using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Gameplay.Managers;

namespace Viper.Game.Gameplay
{
    public class GameplayScreen(Window window, Dispatcher dispatcher)
    {
        public Grid Show(int gridSize = 30)
        {
            GameplayHandler gh = new(window, dispatcher);

            Grid gameplayScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            };

            gameplayScreen.Children.Add(gh.ShowGameplay(gridSize));

            return gameplayScreen;
        }
    }
}
