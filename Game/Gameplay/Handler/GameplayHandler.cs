using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Gameplay.Handler.Managers.Player;
using Viper.Game.Gameplay.Handler.Managers.Space;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Game.Gameplay.Handler
{
    public class GameplayHandler(Window window, Dispatcher dispatcher)
    {
        private const int DEFAULT_GAMEPLAY_SIZE = 400;

        // The overall size of the player and the food.
        private const int OVERALL_SIZE = 30;

        SpaceManager sm = new();

        public Viewbox ShowGameplay(int gridSize)
        {
            FoodManager fm = new(OVERALL_SIZE);

            PlayerManager pm = new(window, dispatcher, fm, OVERALL_SIZE);

            Viewbox gameplayViewbox = new()
            {
                Height = DEFAULT_GAMEPLAY_SIZE,
                Width = DEFAULT_GAMEPLAY_SIZE,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };

            Grid currentSpace = sm.Add(OVERALL_SIZE * gridSize);

            currentSpace.Children.Add(pm.Add());

            currentSpace.Children.Add(fm.Add());

            gameplayViewbox.Child = currentSpace;

            gameplayViewbox.PreviewMouseDown += (s, e) =>
            {
                pm.Players[0].Focus();
            };

            return gameplayViewbox;
        }

        public void ChangeGridSize(int selectedField, int newSize)
        {
            sm.SelectSpace(selectedField).Height = OVERALL_SIZE * newSize;
            sm.SelectSpace(selectedField).Width = OVERALL_SIZE * newSize;
        }
    }
}
