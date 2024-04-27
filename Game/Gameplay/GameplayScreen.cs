using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Gameplay.Managers;

namespace Viper.Game.Gameplay
{
    public class GameplayScreen(Window window, Dispatcher dispatcher)
    {
        private const int DEFAULT_GAMEPLAY_SIZE = 400;

        public PlayfieldManager PlayfieldManager = new();

        public GameplayManager GameplayManager = new(window, dispatcher);

        private Viewbox _gameplayViewbox = new()
        {
            Height = DEFAULT_GAMEPLAY_SIZE,
            Width = DEFAULT_GAMEPLAY_SIZE,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        public Grid Show(int playfieldGridSize)
        {
            Grid currentPlayfield = PlayfieldManager.Add(GameplayManager.ELEMENTS_SIZE * playfieldGridSize);

            currentPlayfield.Children.Add(GameplayManager.ShowPlayer(new TranslateTransform(0, 0)));

            currentPlayfield.Children.Add(GameplayManager.AddFood());

            _gameplayViewbox.Child = currentPlayfield;

            _gameplayViewbox.PreviewMouseDown += (s, e) =>
            {
                GameplayManager.PlayerBody[0].Focus();
            };

            Grid gameplayScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            };

            gameplayScreen.Children.Add(_gameplayViewbox);

            return gameplayScreen;
        }

        public void ChangePlayfieldGridSize(int selectedField, int newSize)
        {
            PlayfieldManager.SelectSpace(selectedField).Height = GameplayManager.ELEMENTS_SIZE * newSize;
            PlayfieldManager.SelectSpace(selectedField).Width = GameplayManager.ELEMENTS_SIZE * newSize;
        }

        public void ChangeGameplayZoom(int newZoom)
        {
            _gameplayViewbox.Height = newZoom;
            _gameplayViewbox.Width = newZoom;
        }
    }
}
