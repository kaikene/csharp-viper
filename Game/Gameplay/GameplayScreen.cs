using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Gameplay.Managers;

namespace Viper.Game.Gameplay
{
    public class GameplayScreen(Window window, Dispatcher dispatcher, PlayfieldManager pm, GameManager gm)
    {
        private const int DEFAULT_GAMEPLAY_SIZE = 400;

        private Viewbox gameplayViewbox = new()
        {
            Height = DEFAULT_GAMEPLAY_SIZE,
            Width = DEFAULT_GAMEPLAY_SIZE,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        public Grid Show(int playfieldGridSize)
        {
            Grid currentSpace = pm.Add(GameManager.ELEMENTS_SIZE * playfieldGridSize);

            currentSpace.Children.Add(gm.AddPlayer());

            currentSpace.Children.Add(gm.AddFood());

            gameplayViewbox.Child = currentSpace;

            gameplayViewbox.PreviewMouseDown += (s, e) =>
            {
                gm.Players[0].Focus();
            };

            Grid gameplayScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            };

            gameplayScreen.Children.Add(gameplayViewbox);

            return gameplayScreen;
        }

        public void ChangePlayfieldGridSize(int selectedField, int newSize)
        {
            pm.SelectSpace(selectedField).Height = GameManager.ELEMENTS_SIZE * newSize;
            pm.SelectSpace(selectedField).Width = GameManager.ELEMENTS_SIZE * newSize;
        }

        public void ChangeGameplayZoom(int newZoom)
        {
            gameplayViewbox.Height = newZoom;
            gameplayViewbox.Width = newZoom;
        }
    }
}
