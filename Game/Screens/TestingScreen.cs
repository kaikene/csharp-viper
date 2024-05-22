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
using System.Windows.Shapes;
using Viper.Game.Elements.Gameplay;
using Viper.Game.Events;
using Viper.Game.Interfaces;
using Viper.Game.Managers;

namespace Viper.Screens
{
    public class TestingScreen : IScreenStates
    {
        private bool _isLoaded = false, _isHidden = false;

        public bool IsLoaded { get { return _isLoaded; } }

        public bool IsHidden { get { return _isHidden; } }

        private LinearGradientBrush lGradient = new()
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops = { new GradientStop(Color.FromArgb(255, 151, 255, 77), 0.0), new GradientStop(Color.FromArgb(255, 145, 184, 255), 1.0) },
        };

        private Grid _testing = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            IsHitTestVisible = false,
            Visibility = Visibility.Hidden,
        };

        public Grid Container { get { return _testing; } }

        private Grid _testingSpace = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Margin = new System.Windows.Thickness(380, 30, 30, 30),
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

        private Button _screenManagerTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Content = "ScreenManager"
        };

        public void Hide()
        {
            if (_isLoaded)
            {
                _testing.Visibility = Visibility.Hidden;
                _testing.IsHitTestVisible = false;
                _isHidden = true;
            }
        }

        public void Show()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                _testing.IsHitTestVisible = true;
                _testing.Visibility = Visibility.Visible;
                _testing.Background = lGradient;

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

                    Button addLive = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "+1 Live"
                    };

                    Button removeLive = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "-1 Live"
                    };

                    TextBlock testDebugMov = new() { Text = "Player Moving:", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugDir = new() { Text = "Player Direction:", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugBE = new() { Text = "Player Elements:", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugPos = new() { Text = "Position: X: " + player.Position.X + " | Y: " + player.Position.X, Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugPD = new() { Text = "Player Died: 0 time(s)", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugTR = new() { Text = "Current Tickrate: " + player.TickRate, Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugPC = new() { Text = $"Player Color: Color.FromArgb({player.CurrentColor.A}, {player.CurrentColor.R}, {player.CurrentColor.G}, {player.CurrentColor.B})", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugIN = new() { Text = $"Key Bindings: {player.InputUp}, {player.InputDown}, {player.InputLeft}, {player.InputRight}", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugLV = new() { Text = $"Lives: {player.Lives}", Foreground = new SolidColorBrush(Colors.White), };

                    StackPanel debugStats = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Width = 300,
                    };

                    Grid tempPlayfield = new()
                    {
                        Height = 457,
                        Width = 515,
                        Background = new SolidColorBrush(Colors.DarkGray),
                        ClipToBounds = true,
                    };

                    addTickRate.Click += (s, e) =>
                    {
                        player.TickRate += 10;
                    };

                    decreaseTickRate.Click += (s, e) =>
                    {
                        player.TickRate -= 10;
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
                        player.Reset();
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

                    addLive.Click += (s, e) =>
                    {
                        player.Lives++;
                        testDebugLV.Text = $"Lives: {player.Lives}";
                    };

                    removeLive.Click += (s, e) =>
                    {
                        player.Lives--;
                        testDebugLV.Text = $"Lives: {player.Lives}";
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

                    player.Died += (s, e) =>
                    {
                        testDebugPD.Text = "Player Died: " + e.DeathCounter + " time(s)";
                    };

                    player.LivesChanged += (s, e) =>
                    {
                        testDebugLV.Text = $"Lives: {e.CurrentLives}";

                        if (e.CurrentLives == 0)
                        {
                            MessageBox.Show("You have zero lives!", "Wawa", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
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
                        testDebugIN.Text = $"Key Bindings: {player.InputUp}, {player.InputDown}, {player.InputLeft}, {player.InputRight}";
                    };

                    player.Reset();

                    player.Show(tempPlayfield);

                    debugStats.Children.Add(testDebugMov);
                    debugStats.Children.Add(testDebugDir);
                    debugStats.Children.Add(testDebugPos);
                    debugStats.Children.Add(testDebugBE);
                    debugStats.Children.Add(testDebugPD);
                    debugStats.Children.Add(testDebugTR);
                    debugStats.Children.Add(testDebugPC);
                    debugStats.Children.Add(testDebugIN);
                    debugStats.Children.Add(testDebugLV);

                    _testingAdditionalSelector.Children.Add(addTickRate);
                    _testingAdditionalSelector.Children.Add(decreaseTickRate);
                    _testingAdditionalSelector.Children.Add(changeColor);
                    _testingAdditionalSelector.Children.Add(increaseSize);
                    _testingAdditionalSelector.Children.Add(decreaseSize);
                    _testingAdditionalSelector.Children.Add(reset);
                    _testingAdditionalSelector.Children.Add(changeInputs);
                    _testingAdditionalSelector.Children.Add(addLive);
                    _testingAdditionalSelector.Children.Add(removeLive);

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
                        Height = 34,
                        Width = 45,
                        Background = new SolidColorBrush(Colors.DarkGray),
                        ClipToBounds = false,
                    };

                    TextBlock testDebugFC = new() { Text = $"Food Color: Color.FromArgb({food.CurrentColor.A}, {food.CurrentColor.R}, {food.CurrentColor.G}, {food.CurrentColor.B})", Foreground = new SolidColorBrush(Colors.White), };
                    TextBlock testDebugPS = new() { Text = $"Food Position: X: {food.Position.X} | Y: {food.Position.Y}", Foreground = new SolidColorBrush(Colors.White), };

                    StackPanel debugStats = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Width = 300,
                    };

                    Panel.SetZIndex(debugStats, 10);

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
                        Content = "Reset"
                    };

                    addFood.Click += (s, e) =>
                    {
                        food.Show(tempPlayfield);
                    };

                    removeFood.Click += (s, e) =>
                    {
                        food.Remove();
                    };

                    changeColor.Click += (s, e) =>
                    {
                        Random rnd = new();

                        food.ChangeFoodColor(Color.FromArgb(255, Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256))));
                    };

                    reposition.Click += (s, e) =>
                    {
                        Random random = new();

                        food.Reset();
                    };

                    food.PositionChanged += (s, e) =>
                    {
                        testDebugPS.Text = $"Food position: X: {e.X}, Y: {e.Y}";
                    };

                    food.ColorChanged += (s, e) =>
                    {
                        testDebugFC.Text = $"Food Color: Color.FromArgb({e.Color.A}, {e.Color.R}, {e.Color.G}, {e.Color.B})";
                    };


                    food.Reset();

                    debugStats.Children.Add(testDebugFC);
                    debugStats.Children.Add(testDebugPS);

                    _testingSpace.Children.Add(tempPlayfield);
                    _testingSpace.Children.Add(debugStats);

                    food.Show(tempPlayfield);

                    _testingAdditionalSelector.Children.Add(addFood);
                    _testingAdditionalSelector.Children.Add(removeFood);
                    _testingAdditionalSelector.Children.Add(changeColor);
                    _testingAdditionalSelector.Children.Add(reposition);
                }

                _gameplayManagerTest.Click += OnGameplayManagerClick;

                void OnGameplayManagerClick(Object sender, RoutedEventArgs e)
                {
                    _testingSpace.Children.Clear();
                    _testingAdditionalSelector.Children.Clear();

                    GameplayManager gm = new();

                    int pfSize = GameplayManager.DEFAULT_PLAYFIELD_SIZE, dpSize = GameplayManager.DEFAULT_DISPLAYER_SIZE;

                    _testingSpace.Children.Add(gm.Displayer);

                    gm.Player.TickRate = 200;

                    gm.Start();

                    Button start = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Start"
                    };

                    Button addFood = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Add more food"
                    };

                    Button removeFood = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Remove Food"
                    };

                    Button incrementPlayfieldSize = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "+50 PF Size"
                    };

                    Button decreasePlayfieldSize = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "-50 PF Size"
                    };

                    Button incrementDisplayerSize = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "+50 DP Size"
                    };

                    Button decreaseDisplayerSize = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "-50 DP Size"
                    };

                    Button cleanUp = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Clear"
                    };

                    start.Click += (s, e) =>
                    {
                        gm.Start();
                    };

                    addFood.Click += (s, e) =>
                    {
                        gm.AddFood();
                    };

                    removeFood.Click += (s, e) =>
                    {
                        gm.RemoveFood();
                    };

                    incrementPlayfieldSize.Click += (s, e) =>
                    {
                        pfSize += 1;
                        gm.ChangePlayfieldGridSize(pfSize);

                        foreach (Food food in gm.Food)
                        {
                            food.Reset();
                        }
                    };

                    decreasePlayfieldSize.Click += (s, e) =>
                    {
                        pfSize -= 1;
                        gm.ChangePlayfieldGridSize(pfSize);

                        foreach (Food food in gm.Food)
                        {
                            food.Reset();
                        }
                    };

                    incrementDisplayerSize.Click += (s, e) =>
                    {
                        dpSize += 10;
                        gm.ChangeDisplayerSize(dpSize);
                    };

                    decreaseDisplayerSize.Click += (s, e) =>
                    {
                        dpSize -= 10;
                        gm.ChangeDisplayerSize(dpSize);
                    };

                    cleanUp.Click += (s, e) =>
                    {
                        gm.Stop();
                    };

                    _testingAdditionalSelector.Children.Add(start);
                    _testingAdditionalSelector.Children.Add(cleanUp);
                    _testingAdditionalSelector.Children.Add(addFood);
                    _testingAdditionalSelector.Children.Add(removeFood);
                    _testingAdditionalSelector.Children.Add(incrementPlayfieldSize);
                    _testingAdditionalSelector.Children.Add(decreasePlayfieldSize);
                    _testingAdditionalSelector.Children.Add(incrementDisplayerSize);
                    _testingAdditionalSelector.Children.Add(decreaseDisplayerSize);
                }

                _screenManagerTest.Click += OnScreenManagerClick;

                void OnScreenManagerClick(object sender, RoutedEventArgs e)
                {
                    ScreenManager screenManager = new();

                    _testingSpace.Children.Clear();
                    _testingAdditionalSelector.Children.Clear();

                    _testingSpace.Children.Add(screenManager.Displayer);
                    screenManager.Start();

                    Button start = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Start"
                    };

                    Button cleanUp = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "cleanUp"
                    };

                    Button showMenu = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Show Menu"
                    };

                    Button showGameplay = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Show Gameplay"
                    };

                    Button showTesting = new()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        Margin = new System.Windows.Thickness(2, 2, 2, 2),
                        Height = 25,
                        Content = "Show Testing"
                    };

                    start.Click += (s, e) =>
                    {
                        screenManager.Start();
                    };

                    cleanUp.Click += (s, e) =>
                    {
                        screenManager.Stop();
                    };

                    showMenu.Click += (s, e) =>
                    {
                        screenManager.ShowScreen(ScreenManager.Screens.Menu);
                    };

                    showGameplay.Click += (s, e) =>
                    {
                        screenManager.ShowScreen(ScreenManager.Screens.Gameplay);
                    };

                    showTesting.Click += (s, e) =>
                    {
                        screenManager.ShowScreen(ScreenManager.Screens.Testing);
                    };

                    _testingAdditionalSelector.Children.Add(start);
                    _testingAdditionalSelector.Children.Add(cleanUp);
                    _testingAdditionalSelector.Children.Add(showMenu);
                    _testingAdditionalSelector.Children.Add(showGameplay);
                    _testingAdditionalSelector.Children.Add(showTesting);
                }

                _testingSelector.Children.Add(_playerTest);
                _testingSelector.Children.Add(_foodTest);
                _testingSelector.Children.Add(_gameplayManagerTest);
                _testingSelector.Children.Add(_screenManagerTest);

                _testingSelectorSV.Content = _testingSelector;
                _testingAdditionalSelectorSV.Content = _testingAdditionalSelector;

                _testing.Children.Add(_testingSpace);
                _testing.Children.Add(_testingSelectorSV);
                _testing.Children.Add(_testingAdditionalSelectorSV);
                _testing.Children.Add(screenIdetifier);
            }
            else if (_isHidden)
            {
                _testing.Visibility = Visibility.Visible;
                _testing.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        public void Clear()
        {
            _testingSelector.Children.Clear();
            _testing.Visibility = Visibility.Hidden;
            _testingSelectorSV.Content = null;
            _testingAdditionalSelectorSV.Content = null;
            _testingSpace.Children.Clear();
            _testing.Children.Clear();
            _isLoaded = false;
            _testing.IsHitTestVisible = false;
        }
    }
}
