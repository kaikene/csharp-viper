using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Managers;
using Viper.Viper.Game.Managers;

namespace Viper.Game.Screens
{
    public class GameplayScreen()
    {
        public event EventHandler GoBack;

        public const int DEFAULT_VIEWBOX_SIZE = 400;

        public const int DEFAULT_PLAYFIELD_SIZE = 20;

        public PlayfieldManager PlayfieldManager = new();

        public GameplayManager GameplayManager = new();

        private Viewbox CurrentViewbox;

        public Grid Show(int playfieldGridSize = DEFAULT_PLAYFIELD_SIZE)
        {
            Viewbox gameplayViewbox = new()
            {
                Height = DEFAULT_VIEWBOX_SIZE,
                Width = DEFAULT_VIEWBOX_SIZE,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            CurrentViewbox = gameplayViewbox;

            Grid currentPlayfield = PlayfieldManager.Add(GameplayManager.ELEMENTS_SIZE * playfieldGridSize);

            currentPlayfield.Children.Add(GameplayManager.ShowPlayer(new TranslateTransform(0, 0)));

            currentPlayfield.Children.Add(GameplayManager.AddFood());

            gameplayViewbox.Child = currentPlayfield;

            gameplayViewbox.Loaded += (s, e) =>
            {
                GameplayManager.PlayerBody[0].Focus();
            };

            Grid gameplay = new()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Tag = "Viper.Game.Screens.Gameplay"
            };

            TextBlock screenIdetifier = new()
            {
                Text = gameplay.Tag.ToString(),
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(70, 255, 255, 255)),
                Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
            };

            Panel.SetZIndex(screenIdetifier, 5);

            gameplay.Children.Add(gameplayViewbox);

            gameplay.Children.Add(screenIdetifier);

            gameplay.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    GoBack.Invoke(this, e);
                }
            };

            return gameplay;
        }

        public void ChangePlayfieldGridSize(int newSize)
        {
            PlayfieldManager.ChangeSize(GameplayManager.ELEMENTS_SIZE * newSize);
        }

        public void ChangeGameplayZoom(int newZoom)
        {
            CurrentViewbox.Height = newZoom;
            CurrentViewbox.Width = newZoom;
        }
    }
}
