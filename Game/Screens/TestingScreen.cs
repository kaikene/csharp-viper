using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.Game.Elements;

namespace Viper.Screens
{
    public class TestingScreen
    {
        private Grid _testing = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
        };

        public Grid TestingContainer { get { return _testing; } }

        private Grid _testingSpace = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Margin = new System.Windows.Thickness(350, 0, 0, 0),
        };

        private ScrollViewer _testingSelectorSV = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Width = 200,
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
        };

        private StackPanel _testingSelector = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
            Width = 200,
        };

        private ScrollViewer _testingAdditionalSelectorSV = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Width = 150,
            Margin = new System.Windows.Thickness(200, 0, 0, 0),
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
        };

        private StackPanel _testingAdditionalSelector = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40)),
            Width = 150,
        };

        private Button _playerTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Content = "Player"
        };

        private Button _foodTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Content = "Food"
        };

        private Player _player = new();

        private Food _food = new();

        public void Show()
        {
            TextBlock screenIdetifier = new()
            {
                Text = "Viper.Game.Screens.TestingScreen",
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(70, 255, 255, 255)),
                Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
            };

            _playerTest.Click += (s, e) =>
            {
                _testingSpace.Children.Clear();
                _testingAdditionalSelector.Children.Clear();

                int playerDeaths = 0;

                Button addTickRate = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "+10 Tick Rate"
                };

                Button decreaseTickRate = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "-10 Tick Rate"
                };

                Button changeColor = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Random Colour"
                };

                Button increaseSize = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Increase Size"
                };

                Button decreaseSize = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Decrease Size"
                };

                Button reset = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Reset"
                };

                TextBlock testDebugMov = new() { Text = "Player Moving:" };
                TextBlock testDebugDir = new() { Text = "Player Direction:" };
                TextBlock testDebugBE = new() { Text = "Player Elements:" };
                TextBlock testDebugPos = new() { Text = "Position: X: " + _player.PlayerXPosition + " | Y: " + _player.PlayerYPosition };
                TextBlock testDebugPD = new() { Text = "Player Died: 0 times" };
                TextBlock testDebugTR = new() { Text = "Current Tickrate: " + _player.CurrentTickRate };
                TextBlock testDebugPC = new() { Text = "Player Color: " + _player.CurrentColor };

                StackPanel debugStats = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Width = 300,
                };

                Grid tempPlayfield = new()
                {
                    Height = 450,
                    Width = 510,
                    Background = new SolidColorBrush(Colors.DarkGray),
                    ClipToBounds = true,
                };

                addTickRate.Click += (s, e) =>
                {
                    _player.ChangeTickRate(_player.CurrentTickRate + 10);
                };

                decreaseTickRate.Click += (s, e) =>
                {
                    _player.ChangeTickRate(_player.CurrentTickRate - 10);
                };

                changeColor.Click += (s, e) =>
                {
                    Random rnd = new();

                    _player.ChangePlayerColor(Color.FromArgb(255, Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256))));
                };

                increaseSize.Click += (s, e) =>
                {
                    _player.IncreasePlayerSize();
                };

                decreaseSize.Click += (s, e) =>
                {
                    _player.DecreasePlayerSize();
                };

                reset.Click += (s, e) =>
                {
                    _player.ResetGameplay();
                };

                _player.PlayerDirectionChanged += (s, e) =>
                {
                    testDebugDir.Text = "Direction: " + _player.PlayerDirection.ToString();
                };

                _player.BodyElementsCountChanged += (s, e) =>
                {
                    testDebugBE.Text = "Player Elements: " + _player.PlayerBodyCount;
                };

                _player.PlayerMovingChanged += (s, e) =>
                {
                    testDebugMov.Text = "Player Moving: " + _player.IsPlayerMoving;
                };

                _player.PlayerPositionChanged += (s, e) =>
                {
                    testDebugPos.Text = "Position: X: " + _player.PlayerXPosition + " | Y: " + _player.PlayerYPosition;
                };

                _player.PlayerDied += (s, e) =>
                {
                    playerDeaths += 1;
                    testDebugPD.Text = "Player Died: " + playerDeaths + " times";
                };

                _player.TickrateChanged += (s, e) =>
                {
                    testDebugTR.Text = "Current Tickrate: " + _player.CurrentTickRate;
                };

                _player.ColorChanged += (s, e) =>
                {
                    testDebugPC.Text = "Player Color: " + _player.CurrentColor;
                };

                _player.CleanUp();
                tempPlayfield.Children.Add(_player.AddPlayer());
                debugStats.Children.Add(testDebugMov);
                debugStats.Children.Add(testDebugDir);
                debugStats.Children.Add(testDebugPos);
                debugStats.Children.Add(testDebugBE);
                debugStats.Children.Add(testDebugPD);
                debugStats.Children.Add(testDebugTR);
                debugStats.Children.Add(testDebugPC);
                tempPlayfield.Children.Add(debugStats);
                _testingAdditionalSelector.Children.Add(addTickRate);
                _testingAdditionalSelector.Children.Add(decreaseTickRate);
                _testingAdditionalSelector.Children.Add(changeColor);
                _testingAdditionalSelector.Children.Add(increaseSize);
                _testingAdditionalSelector.Children.Add(decreaseSize);
                _testingSpace.Children.Add(tempPlayfield);
            };

            _foodTest.Click += (s, e) =>
            {
                _testingSpace.Children.Clear();
                _testingAdditionalSelector.Children.Clear();

                Grid tempPlayfield = new()
                {
                    Height = 450,
                    Width = 450,
                    Background = new SolidColorBrush(Colors.DarkGray),
                    ClipToBounds = true,
                };

                Button addFood = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "+1 Food"
                };

                Button removeFood = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "-1 Food"
                };

                Button changeColor = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Random Color"
                };

                Button reposition = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Random Respawn"
                };

                addFood.Click += (s, e) =>
                {
                    tempPlayfield.Children.Add(_food.AddFood());
                };

                removeFood.Click += (s, e) =>
                {
                    _food.RemoveFood();
                };

                _food.CleanUp();
                _testingSpace.Children.Add(tempPlayfield);
                tempPlayfield.Children.Add(_food.AddFood());
                _testingAdditionalSelector.Children.Add(addFood);
                _testingAdditionalSelector.Children.Add(removeFood);
                _testingAdditionalSelector.Children.Add(changeColor);
                _testingAdditionalSelector.Children.Add(reposition);
            };
            _testingSelector.Children.Add(_playerTest);
            _testingSelector.Children.Add(_foodTest);

            _testingSelectorSV.Content = _testingSelector;
            _testingAdditionalSelectorSV.Content = _testingAdditionalSelector;

            _testing.Children.Add(_testingSpace);
            _testing.Children.Add(_testingSelectorSV);
            _testing.Children.Add(_testingAdditionalSelectorSV);
            _testing.Children.Add(screenIdetifier);
        }

        public void CleanUp()
        {
            _player.CleanUp();
            _food.CleanUp();
            _testingSelector.Children.Clear();
            _testingSelectorSV.Content = null;
            _testingAdditionalSelectorSV.Content = null;
            _testingSpace.Children.Clear();
            _testing.Children.Clear();
        }
    }
}
