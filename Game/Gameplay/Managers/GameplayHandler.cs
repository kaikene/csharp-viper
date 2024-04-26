using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Gameplay.Managers.Player;
using Viper.Game.Gameplay.Managers.Space;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Game.Gameplay.Managers
{
    public class GameplayHandler(Window window, Dispatcher dispatcher)
    {
        private const int DEFAULT_GAMEPLAY_SIZE = 400;

        // The overall size of the player and the food.
        private const int OVERALL_SIZE = 30;

        PlayfieldManager pfm = new();

        public Viewbox ShowGameplay(int gridSize)
        {
            FoodManager fm = new(OVERALL_SIZE);

            PlayerManager pm = new(window, dispatcher, fm, OVERALL_SIZE);

            Viewbox gameplayViewbox = new()
            {
                Height = DEFAULT_GAMEPLAY_SIZE,
                Width = DEFAULT_GAMEPLAY_SIZE,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            Grid currentSpace = pfm.Add(OVERALL_SIZE * gridSize);

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
            pfm.SelectSpace(selectedField).Height = OVERALL_SIZE * newSize;
            pfm.SelectSpace(selectedField).Width = OVERALL_SIZE * newSize;
        }
    }
}
