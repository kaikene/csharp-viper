using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Managers;

namespace Viper.Game.Screens
{
    public class GameplayScreen()
    {
        public event EventHandler GoBack;

        public const int DEFAULT_VIEWBOX_SIZE = 400;

        public const int DEFAULT_PLAYFIELD_SIZE = 20;

        public const int DEBUG_FONT_SIZE = 10;

        public Color DebugFontColor = Color.FromArgb(255, 255, 248, 38);

        public PlayfieldManager PlayfieldManager = new();

        public GameplayManager GameplayManager = new();

        private Viewbox _currentViewbox;

        private Grid _gameplayElementsHandler;

        public Grid Show(int playfieldGridSize = DEFAULT_PLAYFIELD_SIZE)
        {
            bool _canRaiseEvents = false;

            Grid gameplayElementsHandler = new()
            {
                Height = DEFAULT_VIEWBOX_SIZE,
                Width = DEFAULT_VIEWBOX_SIZE,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            Viewbox gameplayViewbox = new()
            {
                Height = DEFAULT_VIEWBOX_SIZE,
                Width = DEFAULT_VIEWBOX_SIZE,
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

            Grid currentPlayfield = PlayfieldManager.Add(GameplayManager.ELEMENTS_SIZE * playfieldGridSize);

            Panel.SetZIndex(screenIdetifier, 5);

            gameplayElementsHandler.Children.Add(gameplayViewbox);

            gameplayElementsHandler.Children.Add(debugDataStackPanel);

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
                        GameplayManager.AreValueEventsEnabled = _canRaiseEvents;
                        GameplayManager.UpdateAllEvents();

                        if (_canRaiseEvents)
                        {
                            debugDataStackPanel.Opacity = 1;
                        }
                        else
                        {
                            debugDataStackPanel.Opacity = 0;
                        }
                    }
                }
            };

            GameplayManager.PointsChanged += (s, e) =>
            {
                debugPointsTB.Text = "Points: " + GameplayManager.Points;
            };

            GameplayManager.FoodAmountChanged += (s, e) =>
            {
                debugFoodTB.Text = "FoodAmount: " + GameplayManager.FoodAmount;
            };

            GameplayManager.PlayerMovingChanged += (s, e) =>
            {
                debugMovTB.Text = "IsPlayerMoving: " + GameplayManager.IsPlayerMoving;
            };

            GameplayManager.PlayerDirectionChanged += (s, e) =>
            {
                debugDirTB.Text = "PlayerDirection: " + GameplayManager.PlayerDirection.ToString();
            };

            GameplayManager.BodyElementsCountChanged += (s, e) =>
            {
                debugBodyTB.Text = "PlayerBodyElements: " + GameplayManager.PlayerBodyCount;
            };

            GameplayManager.PlayerXPositionChanged += (s, e) =>
            {
                debugXposTB.Text = "currentPosX:" + GameplayManager.PlayerXPosition;
            };

            GameplayManager.PlayerYPositionChanged += (s, e) =>
            {
                debugYposTB.Text = "currentPosY:" + GameplayManager.PlayerYPosition;
            };

            currentPlayfield.Children.Add(GameplayManager.ShowPlayer(new TranslateTransform(0, 0)));

            currentPlayfield.Children.Add(GameplayManager.AddFood());

            gameplayViewbox.Child = currentPlayfield;

            return gameplay;
        }

        public void ChangePlayfieldGridSize(int newSize)
        {
            PlayfieldManager.ChangeSize(GameplayManager.ELEMENTS_SIZE * newSize);
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
