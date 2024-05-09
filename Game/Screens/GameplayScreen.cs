using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Elements;
using Viper.Game.Managers;

namespace Viper.Game.Screens
{
    public class GameplayScreen()
    {
        public event EventHandler GoBack;

        public const int DEFAULT_VIEWBOX_SIZE = 400;

        public const int DEFAULT_PLAYFIELD_SIZE = 30;

        public const int DEBUG_FONT_SIZE = 10;

        public Color DebugFontColor = Color.FromArgb(255, 255, 248, 38);

        public GameplayManager GameplayManager = new();

        private Viewbox _currentViewbox;

        private Grid _gameplayElementsHandler;

        public Grid Show()
        {
            bool _canRaiseEvents = false;

            Grid gameplayElementsHandler = new()
            {
                Height = DEFAULT_VIEWBOX_SIZE,
                Width = DEFAULT_VIEWBOX_SIZE,
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            Grid overlay = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            Viewbox gameplayViewbox = new()
            {
                Height = DEFAULT_VIEWBOX_SIZE,
                Width = DEFAULT_VIEWBOX_SIZE,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            TextBlock description = new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 17,
                Text = "Presiona una tecla",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            StackPanel debugDataStackPanel = new()
            {
                Width = 150,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(-150, 0, 0, 0),
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                Opacity = 0,
            };

            TextBlock debugPointsTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "Points: ",
            };

            TextBlock debugFoodTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "Food Amount:"
            };

            TextBlock debugMovTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "Moving:"
            };

            TextBlock debugDirTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "Direction:"
            };

            TextBlock debugBodyTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "PlayerBody Elements:"
            };

            TextBlock debugXposTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "X Axis:"
            };

            TextBlock debugYposTB = new()
            {
                Foreground = new SolidColorBrush(DebugFontColor),
                FontSize = DEBUG_FONT_SIZE,
                Text = "Y Axis:"
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

            _currentViewbox = gameplayViewbox;
            _gameplayElementsHandler = gameplayElementsHandler;

            Panel.SetZIndex(screenIdetifier, 5);
            Panel.SetZIndex(gameplayElementsHandler, 10);

            gameplayElementsHandler.Children.Add(gameplayViewbox);

            gameplayElementsHandler.Children.Add(debugDataStackPanel);

            gameplayElementsHandler.Children.Add(overlay);

            overlay.Children.Add(description);

            gameplay.Children.Add(_gameplayElementsHandler);

            gameplay.Children.Add(screenIdetifier);

            debugDataStackPanel.Children.Add(debugPointsTB);

            debugDataStackPanel.Children.Add(debugDirTB);

            debugDataStackPanel.Children.Add(debugFoodTB);

            debugDataStackPanel.Children.Add(debugMovTB);

            debugDataStackPanel.Children.Add(debugBodyTB);

            debugDataStackPanel.Children.Add(debugXposTB);

            debugDataStackPanel.Children.Add(debugYposTB);

            gameplay.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    GoBack.Invoke(this, e);
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (e.Key == Key.D)
                    {
                        _canRaiseEvents = !_canRaiseEvents;


                        if (_canRaiseEvents)
                        {
                            debugDataStackPanel.Opacity = 1;
                        }
                        else
                        {
                            debugDataStackPanel.Opacity = 0;
                            overlay.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                        }
                    }
                }
            };

            return gameplay;
        }

        public void ChangeGameplayZoom(int newZoom)
        {
            _currentViewbox.Height = newZoom;
            _currentViewbox.Width = newZoom;
            _gameplayElementsHandler.Height = _currentViewbox.Height;
            _gameplayElementsHandler.Width = _currentViewbox.Width;
        }
    }
}
