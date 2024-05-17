using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Viper.Game.Elements;
using Viper.Game.Events;
using Viper.Game.Managers;

namespace Viper.Screens
{
    public class TestingScreen
    {
        private Grid _testing = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
        };

        public Grid Testing { get { return _testing; } }

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

        private Button _gameplayManagerTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Content = "GameplayManager"
        };

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

            _playerTest.Click += OnPlayerButtonClick;

            void OnPlayerButtonClick(Object Sender, RoutedEventArgs e)
            {
                Player player = new();

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

                Button changeInputs = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Change to WASD/Arrows"
                };

                TextBlock testDebugMov = new() { Text = "Player Moving:", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugDir = new() { Text = "Player Direction:", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugBE = new() { Text = "Player Elements:", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugPos = new() { Text = "Position: X: " + player.PlayerXPosition + " | Y: " + player.PlayerYPosition, Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugPD = new() { Text = "Player Died: 0 times", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugTR = new() { Text = "Current Tickrate: " + player.CurrentTickRate, Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugPC = new() { Text = $"Player Color: Color.FromArgb({player.CurrentColor.A}, {player.CurrentColor.R}, {player.CurrentColor.G}, {player.CurrentColor.B})", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugUP = new() { Text = $"{player.InputUp}", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugDOWN = new() { Text = $"{player.InputDown}", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugLEFT = new() { Text = $"{player.InputLeft}", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugRIGHT = new() { Text = $"{player.InputRight}", Foreground = new SolidColorBrush(Colors.White), };

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
                    player.ChangeTickRate(player.CurrentTickRate + 10);
                };

                decreaseTickRate.Click += (s, e) =>
                {
                    player.ChangeTickRate(player.CurrentTickRate - 10);
                };

                changeColor.Click += (s, e) =>
                {
                    Random rnd = new();

                    player.ChangePlayerColor(Color.FromArgb(255, Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256))));
                };

                increaseSize.Click += (s, e) =>
                {
                    player.IncreasePlayerSize();
                };

                decreaseSize.Click += (s, e) =>
                {
                    player.DecreasePlayerSize();
                };

                reset.Click += (s, e) =>
                {
                    player.ResetGameplay();
                };

                bool inputSwitch = false;

                changeInputs.Click += (s, e) =>
                {
                    if (!inputSwitch)
                    {
                        player.InputUp = Key.W;
                        player.InputDown = Key.S;
                        player.InputLeft = Key.A;
                        player.InputRight = Key.D;
                    }
                    else
                    {
                        player.InputUp = Key.Up;
                        player.InputDown = Key.Down;
                        player.InputLeft = Key.Left;
                        player.InputRight = Key.Right;
                    }

                    inputSwitch = !inputSwitch;
                };

                player.DirectionChanged += (s, e) =>
                {
                    testDebugDir.Text = "Direction: " + e.Direction.ToString();
                };

                player.BodyElementsCountChanged += (s, e) =>
                {
                    testDebugBE.Text = "Player Elements: " + e.BodyElements;
                };

                player.IsMovingChanged += (s, e) =>
                {
                    testDebugMov.Text = "Player Moving: " + e.IsPlayerMoving;
                };

                player.PositionChanged += (s, e) =>
                {
                    testDebugPos.Text = "Position: X: " + e.X + " | Y: " + e.Y;
                };

                player.PlayerDied += (s, e) =>
                {
                    playerDeaths += 1;
                    testDebugPD.Text = "Player Died: " + playerDeaths + " times";
                };

                player.TickrateChanged += (s, e) =>
                {
                    testDebugTR.Text = "Current Tickrate: " + e.TickRate;
                };

                player.ColorChanged += (s, e) =>
                {
                    testDebugPC.Text = $"Player Color: Color.FromArgb({e.Color.A}, {e.Color.R}, {e.Color.G}, {e.Color.B})";
                };

                player.InputChanged += (s, e) =>
                {
                    testDebugUP.Text = $"{player.InputUp}";
                    testDebugDOWN.Text = $"{player.InputDown}";
                    testDebugLEFT.Text = $"{player.InputLeft}";
                    testDebugRIGHT.Text = $"{player.InputRight}";
                };

                player.CleanUp(true);

                tempPlayfield.Children.Add(player.ShowPlayer());

                debugStats.Children.Add(testDebugMov);
                debugStats.Children.Add(testDebugDir);
                debugStats.Children.Add(testDebugPos);
                debugStats.Children.Add(testDebugBE);
                debugStats.Children.Add(testDebugPD);
                debugStats.Children.Add(testDebugTR);
                debugStats.Children.Add(testDebugPC);
                debugStats.Children.Add(testDebugUP);
                debugStats.Children.Add(testDebugDOWN);
                debugStats.Children.Add(testDebugLEFT);
                debugStats.Children.Add(testDebugRIGHT);

                _testingAdditionalSelector.Children.Add(addTickRate);
                _testingAdditionalSelector.Children.Add(decreaseTickRate);
                _testingAdditionalSelector.Children.Add(changeColor);
                _testingAdditionalSelector.Children.Add(increaseSize);
                _testingAdditionalSelector.Children.Add(decreaseSize);
                _testingAdditionalSelector.Children.Add(reset);
                _testingAdditionalSelector.Children.Add(changeInputs);

                _testingSpace.Children.Add(tempPlayfield);
                _testingSpace.Children.Add(debugStats);
            }

            _foodTest.Click += OnFoodtestClick;

            void OnFoodtestClick(Object sender, RoutedEventArgs e)
            {
                Food food = new();

                _testingSpace.Children.Clear();
                _testingAdditionalSelector.Children.Clear();

                Grid tempPlayfield = new()
                {
                    Height = 450,
                    Width = 510,
                    Background = new SolidColorBrush(Colors.DarkGray),
                    ClipToBounds = true,
                };

                TextBlock testDebugFA = new() { Text = $"Food Amount: {food.FoodAmount}", Foreground = new SolidColorBrush(Colors.White), };
                TextBlock testDebugFC = new() { Text = $"Food Color: Color.FromArgb({food.CurrentColor.A}, {food.CurrentColor.R}, {food.CurrentColor.G}, {food.CurrentColor.B})", Foreground = new SolidColorBrush(Colors.White), };

                StackPanel debugStats = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Width = 300,
                };

                Panel.SetZIndex(debugStats, 10);

                ScrollViewer debugPosSW = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Width = 100,
                };

                StackPanel debugPosStats = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Width = 300,
                };

                List<TextBlock> foodPositionsTB = new();

                Button addFood = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Add Food"
                };

                Button removeFood = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Remove Food"
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
                    tempPlayfield.Children.Add(food.AddFood());
                };

                removeFood.Click += (s, e) =>
                {
                    food.RemoveFood();
                };

                changeColor.Click += (s, e) =>
                {
                    Random rnd = new();

                    food.ChangeFoodColor(Color.FromArgb(255, Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256))));
                };

                reposition.Click += (s, e) =>
                {
                    Random random = new();

                    food.RePosition(random.Next(0, food.FoodAmount));
                };

                food.FoodAmountChanged += (s, e) =>
                {
                    testDebugFA.Text = "Food Amount: " + e.FoodElements;

                    TextBlock tempTB = new()
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                    };

                    if (e.Action == Food.FoodAction.Add)
                    {
                        foodPositionsTB.Add(tempTB);
                        debugPosStats.Children.Add(tempTB);
                    }
                    else if (e.Action == Food.FoodAction.Remove)
                    {
                        debugPosStats.Children.RemoveAt(foodPositionsTB.Count - 1);
                        foodPositionsTB.RemoveAt(foodPositionsTB.Count - 1);
                    }
                    else
                    {
                        // Value is just being checked.
                    }
                };

                food.FoodPositionsChanged += (s, e) =>
                {
                    foodPositionsTB[e.FoodIndex].Text = $"X: {e.X}, Y: {e.Y}";
                };

                food.ColorChanged += (s, e) =>
                {
                    testDebugFC.Text = $"Food Color: Color.FromArgb({e.Color.A}, {e.Color.R}, {e.Color.G}, {e.Color.B})";
                };


                food.CleanUp();

                debugStats.Children.Add(testDebugFA);
                debugStats.Children.Add(testDebugFC);

                debugPosSW.Content = debugPosStats;

                _testingSpace.Children.Add(tempPlayfield);
                _testingSpace.Children.Add(debugStats);
                _testingSpace.Children.Add(debugPosSW);

                tempPlayfield.Children.Add(food.AddFood());

                _testingAdditionalSelector.Children.Add(addFood);
                _testingAdditionalSelector.Children.Add(removeFood);
                _testingAdditionalSelector.Children.Add(changeColor);
                _testingAdditionalSelector.Children.Add(reposition);
            }

            _gameplayManagerTest.Click += OnGameplayManagerClick;

            void OnGameplayManagerClick(Object sender, RoutedEventArgs e)
            {
                int counter = 0;

                _testingSpace.Children.Clear();
                _testingAdditionalSelector.Children.Clear();

                GameplayManager gameplayManager = new();

                _testingSpace.Children.Add(gameplayManager.PlayfieldManager);

                gameplayManager.AddPlayfield();

                SetInputsForFirstThreePlayers();

                counter++;

                gameplayManager.Show();

                Button show = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Show"
                };

                Button hide = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Hide"
                };

                Button addPlayfield = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Add Playfield"
                };

                Button removePlayfield = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                    Margin = new System.Windows.Thickness(2, 2, 2, 2),
                    Height = 25,
                    Content = "Remove Playfield"
                };

                TextBlock testDebugPF = new() { Text = $"Playfields: {gameplayManager.PlayfieldAmount}", Foreground = new SolidColorBrush(Colors.White), };

                StackPanel debugStats = new()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    Width = 300,
                };

                show.Click += (s, e) =>
                {
                    gameplayManager.Show();
                };

                hide.Click += (s, e) =>
                {
                    gameplayManager.Hide();
                };

                addPlayfield.Click += (s, e) =>
                {
                    gameplayManager.AddPlayfield();

                    SetInputsForFirstThreePlayers();

                    counter++;
                };

                removePlayfield.Click += (s, e) =>
                {
                    gameplayManager.RemovePlayfield();

                    if (!(counter - 1 < 0))
                    {
                        counter--;
                    }
                };

                // jank but is for testing after all.
                void SetInputsForFirstThreePlayers()
                {
                    if (counter == 0)
                    {
                        gameplayManager.Players[counter].InputUp = Key.W;
                        gameplayManager.Players[counter].InputDown = Key.S;
                        gameplayManager.Players[counter].InputLeft = Key.A;
                        gameplayManager.Players[counter].InputRight = Key.D;
                    }
                    else if (counter == 1)
                    {
                        gameplayManager.Players[counter].InputUp = Key.T;
                        gameplayManager.Players[counter].InputDown = Key.G;
                        gameplayManager.Players[counter].InputLeft = Key.F;
                        gameplayManager.Players[counter].InputRight = Key.H;
                    }
                    else if (counter == 2)
                    {
                        gameplayManager.Players[counter].InputUp = Key.I;
                        gameplayManager.Players[counter].InputDown = Key.K;
                        gameplayManager.Players[counter].InputLeft = Key.J;
                        gameplayManager.Players[counter].InputRight = Key.L;
                    }
                }

                gameplayManager.PlayfieldAmountChanged += (s, e) =>
                {
                    testDebugPF.Text = $"Playfields: {e.NewAmount}";
                };

                debugStats.Children.Add(testDebugPF);

                _testingSpace.Children.Add(debugStats);

                _testingAdditionalSelector.Children.Add(show);
                _testingAdditionalSelector.Children.Add(hide);
                _testingAdditionalSelector.Children.Add(addPlayfield);
                _testingAdditionalSelector.Children.Add(removePlayfield);
            }

            _testingSelector.Children.Add(_playerTest);
            _testingSelector.Children.Add(_foodTest);
            _testingSelector.Children.Add(_gameplayManagerTest);

            _testingSelectorSV.Content = _testingSelector;
            _testingAdditionalSelectorSV.Content = _testingAdditionalSelector;

            _testing.Children.Add(_testingSpace);
            _testing.Children.Add(_testingSelectorSV);
            _testing.Children.Add(_testingAdditionalSelectorSV);
            _testing.Children.Add(screenIdetifier);
        }

        public void CleanUp()
        {
            _testingSelector.Children.Clear();
            _testingSelectorSV.Content = null;
            _testingAdditionalSelectorSV.Content = null;
            _testingSpace.Children.Clear();
            _testing.Children.Clear();
        }
    }
}
