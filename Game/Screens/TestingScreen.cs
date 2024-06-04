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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Viper.Game.Animations;
using Viper.Game.Builders;
using Viper.Game.Elements.Gameplay;
using Viper.Game.Elements.UI;
using Viper.Game.Events;
using Viper.Game.Interfaces;
using Viper.Game.Managers;
using Viper.Game.Screens;
using GradientStop = System.Windows.Media.GradientStop;
using LinearGradientBrush = System.Windows.Media.LinearGradientBrush;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace Viper.Screens
{
    public class TestingScreen : IScreenStates
    {
        private Animate _animate = new();

        private bool _isHidden = false;

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool IsHidden { get { return _isHidden; } }

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
            Margin = new System.Windows.Thickness(360, 10, 10, 10),
            ClipToBounds = true,
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

        private Button _clear = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "Clear testing space"
        };

        private Button _playerTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "Player"
        };

        private Button _foodTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "Food"
        };

        private Button _customElementsTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "UI Elements"
        };

        private Button _menuPanelTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "MenuButtonPanel"
        };

        private Button _settingsPanelTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "SettingsPanel"
        };

        private Button _gameplayManagerTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "GameplayManager"
        };

        private Button _screenManagerTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "ScreenManager"
        };

        private Button _gameplayScreenTest = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new System.Windows.Thickness(3, 3, 3, 3),
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            Content = "GameplayScreen"
        };

        private TextBlock _notice = new()
        {
            Text = "Nothing to test",
            Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255)),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        public void Hide()
        {
            if (_isInitialized)
            {
                _testing.Visibility = Visibility.Hidden;
                _testing.IsHitTestVisible = false;
                _isHidden = true;
            }
        }
        public void Show()
        {
            if (!_isInitialized)
            {
                Random rnd = new();

                LinearGradientBrush lGradient = new()
                {
                    StartPoint = new Point(rnd.NextDouble(), rnd.NextDouble()),
                    EndPoint = new Point(rnd.NextDouble(), rnd.NextDouble()),
                    GradientStops = { new GradientStop(Color.FromRgb(Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255))), 0.0), new GradientStop(Color.FromRgb(Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255))), 1.0) },
                };

                _isInitialized = true;
                _testing.IsHitTestVisible = true;
                _testing.Visibility = Visibility.Visible;
                _testing.Background = lGradient;

                _testingSpace.Children.Clear();
                _testingAdditionalSelector.Children.Clear();
                _testingSpace.Children.Add(_notice);

                _clear.Click += OnClearButtonClicked;
                _playerTest.Click += OnPlayerButtonClick;
                _foodTest.Click += OnFoodtestClick;
                _gameplayManagerTest.Click += OnGameplayManagerClick;
                _screenManagerTest.Click += OnScreenManagerClick;
                _gameplayScreenTest.Click += _gameplayScreenTest_Click;
                _customElementsTest.Click += OnUITestClicked;
                _menuPanelTest.Click += OnMenuPanelClick;
                _settingsPanelTest.Click += OnSettingsPanelClick;

                _clear.MouseLeave += Element_MouseLeave;
                _playerTest.MouseLeave += Element_MouseLeave;
                _foodTest.MouseLeave += Element_MouseLeave;
                _gameplayManagerTest.MouseLeave += Element_MouseLeave;
                _screenManagerTest.MouseLeave += Element_MouseLeave;
                _gameplayScreenTest.MouseLeave += Element_MouseLeave;

                _clear.MouseEnter += Element_MouseEnter;
                _playerTest.MouseEnter += Element_MouseEnter;
                _foodTest.MouseEnter += Element_MouseEnter;
                _gameplayManagerTest.MouseEnter += Element_MouseEnter;
                _screenManagerTest.MouseEnter += Element_MouseEnter;
                _gameplayScreenTest.MouseEnter += Element_MouseEnter;

                _testingSelector.Children.Add(_clear);
                _testingSelector.Children.Add(_playerTest);
                _testingSelector.Children.Add(_foodTest);
                _testingSelector.Children.Add(_customElementsTest);
                _testingSelector.Children.Add(_menuPanelTest);
                _testingSelector.Children.Add(_settingsPanelTest);
                _testingSelector.Children.Add(_gameplayManagerTest);
                _testingSelector.Children.Add(_screenManagerTest);
                _testingSelector.Children.Add(_gameplayScreenTest);

                _testingSelectorSV.Content = _testingSelector;
                _testingAdditionalSelectorSV.Content = _testingAdditionalSelector;

                _testing.Children.Add(_testingSpace);
                _testing.Children.Add(_testingSelectorSV);
                _testing.Children.Add(_testingAdditionalSelectorSV);
            }
            else if (_isHidden)
            {
                _testing.Visibility = Visibility.Visible;
                _testing.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        private void OnClearButtonClicked(Object Sender, RoutedEventArgs e)
        {
            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            _testingSpace.Children.Add(_notice);
        }

        private void OnPlayerButtonClick(Object Sender, RoutedEventArgs e)
        {
            Player player = new();

            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            Grid tempPlayfield = new()
            {
                Height = 457,
                Width = 515,
                Background = new SolidColorBrush(Colors.DarkGray),
                ClipToBounds = true,
            };

            Button add = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show player"
            };

            Button addAuto = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show as automatic"
            };

            Button addTickRate = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "+10 Tick Rate"
            };

            Button decreaseTickRate = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "-10 Tick Rate"
            };

            Button changeColor = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Random Colour"
            };

            Button increaseSize = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Increase Size"
            };

            Button decreaseSize = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Decrease Size"
            };

            Button reset = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Reset"
            };

            Button changeInputs = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Change to WASD/Arrows"
            };

            Button addLive = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "+1 Live"
            };

            Button removeLive = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "-1 Live"
            };

            Button remove = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Exit"
            };

            Slider changeTempPlayfieldHeightSize = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Maximum = 1000,
                Minimum = 0,
                Value = tempPlayfield.Height,
            };

            Slider changeTempPlayfieldWidthSize = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Maximum = 1000,
                Minimum = 0,
                Value = tempPlayfield.Width,
            };

            add.MouseEnter += Element_MouseEnter;
            add.MouseLeave += Element_MouseLeave;

            addAuto.MouseEnter += Element_MouseEnter;
            addAuto.MouseLeave += Element_MouseLeave;

            addTickRate.MouseEnter += Element_MouseEnter;
            addTickRate.MouseLeave += Element_MouseLeave;

            decreaseTickRate.MouseEnter += Element_MouseEnter;
            decreaseTickRate.MouseLeave += Element_MouseLeave;

            changeColor.MouseEnter += Element_MouseEnter;
            changeColor.MouseLeave += Element_MouseLeave;

            increaseSize.MouseEnter += Element_MouseEnter;
            increaseSize.MouseLeave += Element_MouseLeave;

            decreaseSize.MouseEnter += Element_MouseEnter;
            decreaseSize.MouseLeave += Element_MouseLeave;

            reset.MouseEnter += Element_MouseEnter;
            reset.MouseLeave += Element_MouseLeave;

            changeInputs.MouseEnter += Element_MouseEnter;
            changeInputs.MouseLeave += Element_MouseLeave;

            addLive.MouseEnter += Element_MouseEnter;
            addLive.MouseLeave += Element_MouseLeave;

            removeLive.MouseEnter += Element_MouseEnter;
            removeLive.MouseLeave += Element_MouseLeave;

            remove.MouseEnter += Element_MouseEnter;
            remove.MouseLeave += Element_MouseLeave;


            TextBlock testDebugMov = new() { Text = "Player Moving:", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugDir = new() { Text = "Player Direction:", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugBE = new() { Text = "Player Elements:", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugPos = new() { Text = "Position: X: " + player.Position.X + " | Y: " + player.Position.X, Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugPD = new() { Text = "Player Died: 0 time(s)", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugTR = new() { Text = "Current Tickrate: " + player.TickRate, Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugPC = new() { Text = $"Player Color: Color.FromArgb({player.CurrentColor.A}, {player.CurrentColor.R}, {player.CurrentColor.G}, {player.CurrentColor.B})", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugIN = new() { Text = $"Key Bindings: {player.InputUp}, {player.InputDown}, {player.InputLeft}, {player.InputRight}", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugLV = new() { Text = $"Lives: {player.Lives}", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugAU = new() { Text = $"Is Automatic: {player.IsAutomatic}", Foreground = new SolidColorBrush(Colors.White), };

            StackPanel debugStats = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Width = 300,
                IsHitTestVisible = false,
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

            remove.Click += (s, e) =>
            {
                player.Remove();
            };

            add.Click += (s, e) =>
            {
                player.Show(tempPlayfield);
            };

            addAuto.Click += (s, e) =>
            {
                List<Player.Direction> presetDirections = [Player.Direction.Up, Player.Direction.Up, Player.Direction.Up, Player.Direction.Left, Player.Direction.Left, Player.Direction.Left, Player.Direction.Down, Player.Direction.Down, Player.Direction.Left];
                player.Show(tempPlayfield, presetDirections, true);
            };

            changeTempPlayfieldHeightSize.ValueChanged += (s, e) =>
            {
                tempPlayfield.Height = changeTempPlayfieldHeightSize.Value;
            };

            changeTempPlayfieldWidthSize.ValueChanged += (s, e) =>
            {
                tempPlayfield.Width = changeTempPlayfieldWidthSize.Value;
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

            player.AutomatizationChanged += (s, e) =>
            {
                string debugString = $"Is Automatic: {player.IsAutomatic}";

                foreach (Player.Direction dir in player.DirectionsBuffered)
                {
                    debugString += $" {dir}";
                }

                testDebugAU.Text = debugString;
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
            debugStats.Children.Add(testDebugAU);

            _testingAdditionalSelector.Children.Add(add);
            _testingAdditionalSelector.Children.Add(addAuto);
            _testingAdditionalSelector.Children.Add(addTickRate);
            _testingAdditionalSelector.Children.Add(decreaseTickRate);
            _testingAdditionalSelector.Children.Add(changeColor);
            _testingAdditionalSelector.Children.Add(increaseSize);
            _testingAdditionalSelector.Children.Add(decreaseSize);
            _testingAdditionalSelector.Children.Add(reset);
            _testingAdditionalSelector.Children.Add(changeInputs);
            _testingAdditionalSelector.Children.Add(addLive);
            _testingAdditionalSelector.Children.Add(removeLive);
            _testingAdditionalSelector.Children.Add(remove);
            _testingAdditionalSelector.Children.Add(changeTempPlayfieldHeightSize);
            _testingAdditionalSelector.Children.Add(changeTempPlayfieldWidthSize);

            _testingSpace.Children.Add(tempPlayfield);
            _testingSpace.Children.Add(debugStats);
        }
        private void OnFoodtestClick(Object sender, RoutedEventArgs e)
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
                IsHitTestVisible = false,
            };

            Panel.SetZIndex(debugStats, 1);

            Slider changeTempPlayfieldHeightSize = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Maximum = 1000,
                Minimum = 0,
                Value = tempPlayfield.Height,
            };

            Slider changeTempPlayfieldWidthSize = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Maximum = 1000,
                Minimum = 0,
                Value = tempPlayfield.Width,
            };

            Button addFood = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Add Food"
            };

            Button removeFood = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Remove Food"
            };

            Button changeColor = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Random Color"
            };

            Button reposition = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Reset"
            };

            addFood.MouseEnter += Element_MouseEnter;
            addFood.MouseLeave += Element_MouseLeave;

            removeFood.MouseEnter += Element_MouseEnter;
            removeFood.MouseLeave += Element_MouseLeave;

            changeColor.MouseEnter += Element_MouseEnter;
            changeColor.MouseLeave += Element_MouseLeave;

            reposition.MouseEnter += Element_MouseEnter;
            reposition.MouseLeave += Element_MouseLeave;


            addFood.Click += (s, e) =>
            {
                try
                {
                    food.Show(tempPlayfield);
                }
                catch
                {
                    // Already added
                }
            };

            changeTempPlayfieldHeightSize.ValueChanged += (s, e) =>
            {
                tempPlayfield.Height = changeTempPlayfieldHeightSize.Value;
            };

            changeTempPlayfieldWidthSize.ValueChanged += (s, e) =>
            {
                tempPlayfield.Width = changeTempPlayfieldWidthSize.Value;
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

            _testingAdditionalSelector.Children.Add(addFood);
            _testingAdditionalSelector.Children.Add(removeFood);
            _testingAdditionalSelector.Children.Add(changeColor);
            _testingAdditionalSelector.Children.Add(reposition);
            _testingAdditionalSelector.Children.Add(changeTempPlayfieldHeightSize);
            _testingAdditionalSelector.Children.Add(changeTempPlayfieldWidthSize);
        }

        private void OnUITestClicked(Object Sender, RoutedEventArgs e)
        {
            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            int counter = 0;

            StackPanel testThing = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 200,
            };

            StackPanel debugStats = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Width = 300,
                IsHitTestVisible = false,
            };

            Button addElement = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "CB Add element"
            };

            Button removeElement = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "CB Remove element"
            };

            CustomComboBox combo = new();
            CustomCheckBox check = new();
            CustomButton button = new();
            CustomSlider slider = new();
            UnlimitedSelector selector = new();

            TextBlock testDebugComBoxSel = new() { Text = $"ComboBox selection: {combo.SelectedElementName} | Index: {combo.SelectedElementIndex}", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugCheBoxSta = new() { Text = $"Is checked: {check.State}", Foreground = new SolidColorBrush(Colors.White), };

            StackPanel cmb = combo.NewComboBox();
            StackPanel cb = check.NewCheckBox("This is a checkBox");
            StackPanel bt = button.NewButton("This is a button");

            cb.Margin = new Thickness(0, 5, 0, 5);
            cmb.Margin = new Thickness(0, 5, 0, 5);
            bt.Margin = new Thickness(0, 5, 0, 5);

            testThing.Children.Add(cmb);
            testThing.Children.Add(cb);
            testThing.Children.Add(bt);
            testThing.Children.Add(slider.NewSlider());
            testThing.Children.Add(selector.NewSelector("UnlimitedSelector"));

            addElement.Click += (s, e) =>
            {
                Random rnd = new();
                combo.AddElement($"Test {rnd.Next(0, 1000)}");
            };

            removeElement.Click += (s, e) =>
            {
                if (combo.ElementAmount != 0)
                {
                    combo.RemoveAt(combo.ElementAmount - 1);
                }
            };

            combo.SelectionChanged += (s, e) =>
            {
                testDebugComBoxSel.Text = $"ComboBox selection: {e.Selection} | Index: {e.SelectionIndex}";
            };

            check.StateChanged += (s, e) =>
            {
                testDebugCheBoxSta.Text = $"Is CheckBox checked: {e.State}";
            };

            button.Clicked += (s, e) =>
            {
                counter++;

                button.ButtonText = $"Button clicked {counter} times!";
            };

            debugStats.Children.Add(testDebugCheBoxSta);
            debugStats.Children.Add(testDebugComBoxSel);

            _testingSpace.Children.Add(testThing);
            _testingSpace.Children.Add(debugStats);

            _testingAdditionalSelector.Children.Add(addElement);
            _testingAdditionalSelector.Children.Add(removeElement);
        }

        private void OnMenuPanelClick(Object sender, RoutedEventArgs e)
        {
            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            MenuButtonsPanel panel = new();

            _testingSpace.Children.Add(panel.NewMenuPanel());
        }

        private void OnSettingsPanelClick(Object sender, RoutedEventArgs e)
        {
            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            SettingsPanel panel = new();

            Button toggle = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show/Hide"
            };

            toggle.Click += (s, e) =>
            {
                panel.SettingsToggle();
            };

            panel.LoadSettingsElements();

            _testingSpace.Children.Add(panel.Overlay);

            _testingAdditionalSelector.Children.Add(toggle);
        }

        private void OnGameplayManagerClick(Object sender, RoutedEventArgs e)
        {
            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            GameplaySession gm = new();

            int pfSize = GameplaySession.DEFAULT_PLAYFIELD_SIZE, dpSize = GameplaySession.DEFAULT_DISPLAYER_SIZE;

            _testingSpace.Children.Add(gm.Displayer);

            gm.Player.TickRate = 200;

            gm.LoadElements();

            StackPanel debugStats = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Width = 300,
                IsHitTestVisible = false,
            };

            TextBlock testDebugFA = new() { Text = $"Food amount: {gm.Food.Count}", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugPS = new() { Text = $"Playfield Size: {gm.PlayfieldSize}", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugPT = new() { Text = $"Points: {gm.Points}", Foreground = new SolidColorBrush(Colors.White), };

            Button start = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Start"
            };

            Button addFood = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Add more food"
            };

            Button removeFood = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
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
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
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
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "-50 DP Size"
            };

            Button cleanUp = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Clear"
            };

            start.MouseEnter += Element_MouseEnter;
            start.MouseLeave += Element_MouseLeave;

            addFood.MouseEnter += Element_MouseEnter;
            addFood.MouseLeave += Element_MouseLeave;

            removeFood.MouseEnter += Element_MouseEnter;
            removeFood.MouseLeave += Element_MouseLeave;

            incrementPlayfieldSize.MouseEnter += Element_MouseEnter;
            incrementPlayfieldSize.MouseLeave += Element_MouseLeave;

            decreasePlayfieldSize.MouseEnter += Element_MouseEnter;
            decreasePlayfieldSize.MouseLeave += Element_MouseLeave;

            incrementDisplayerSize.MouseEnter += Element_MouseEnter;
            incrementDisplayerSize.MouseLeave += Element_MouseLeave;

            decreaseDisplayerSize.MouseEnter += Element_MouseEnter;
            decreaseDisplayerSize.MouseLeave += Element_MouseLeave;

            cleanUp.MouseEnter += Element_MouseEnter;
            cleanUp.MouseLeave += Element_MouseLeave;


            start.Click += (s, e) =>
            {
                gm.LoadElements();
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

            cleanUp.Click += (s, e) =>
            {
                gm.End();
            };

            gm.PointsChanged += (s, e) =>
            {
                testDebugPT.Text = $"Points {e.Points}";
            };

            gm.FoodAmountChanged += (s, e) =>
            {
                testDebugFA.Text = $"Food amount {e.Amount}";
            };

            gm.PlayfieldSizeChanged += (s, e) =>
            {
                testDebugPS.Text = $"Playfield Size: {e.Size}";
            };

            debugStats.Children.Add(testDebugFA);
            debugStats.Children.Add(testDebugPS);
            debugStats.Children.Add(testDebugPT);

            _testingSpace.Children.Add(debugStats);

            _testingAdditionalSelector.Children.Add(start);
            _testingAdditionalSelector.Children.Add(cleanUp);
            _testingAdditionalSelector.Children.Add(addFood);
            _testingAdditionalSelector.Children.Add(removeFood);
            _testingAdditionalSelector.Children.Add(incrementPlayfieldSize);
            _testingAdditionalSelector.Children.Add(decreasePlayfieldSize);
        }
        private void OnScreenManagerClick(object sender, RoutedEventArgs e)
        {
            ScreenSwitcher sm = new();

            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            _testingSpace.Children.Add(sm.Displayer);
            sm.LoadScreens();

            StackPanel debugStats = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Width = 300,
                IsHitTestVisible = false,
            };

            TextBlock testDebugCS = new() { Text = $"Current Screen: None", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugSH = new() { Text = $"Screens Saved: 0", Foreground = new SolidColorBrush(Colors.White), };

            Button start = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Start"
            };

            Button cleanUp = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "cleanUp"
            };

            Button showMenu = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show Menu"
            };

            Button showGameplay = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show Gameplay"
            };

            Button showTesting = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show Testing"
            };

            start.MouseEnter += Element_MouseEnter;
            start.MouseLeave += Element_MouseLeave;

            cleanUp.MouseEnter += Element_MouseEnter;
            cleanUp.MouseLeave += Element_MouseLeave;

            showMenu.MouseEnter += Element_MouseEnter;
            showMenu.MouseLeave += Element_MouseLeave;

            showGameplay.MouseEnter += Element_MouseEnter;
            showGameplay.MouseLeave += Element_MouseLeave;

            showTesting.MouseEnter += Element_MouseEnter;
            showTesting.MouseLeave += Element_MouseLeave;

            start.Click += (s, e) =>
            {
                sm.LoadScreens();
            };

            cleanUp.Click += (s, e) =>
            {
                sm.End();
            };

            showMenu.Click += (s, e) =>
            {
                sm.ShowScreen(ScreenSwitcher.Screens.Menu);
            };

            showGameplay.Click += (s, e) =>
            {
                sm.ShowScreen(ScreenSwitcher.Screens.Gameplay);
            };

            showTesting.Click += (s, e) =>
            {
                sm.ShowScreen(ScreenSwitcher.Screens.Testing);
            };

            sm.ScreenChanged += (s, e) =>
            {
                testDebugCS.Text = $"Current Screen: {e.CurrentScreen}";
            };

            sm.HistoryChanged += (s, e) =>
            {
                testDebugSH.Text = $"Screens Saved: {e.ScreensSaved}";
            };

            debugStats.Children.Add(testDebugCS);
            debugStats.Children.Add(testDebugSH);

            _testingSpace.Children.Add(debugStats);

            _testingAdditionalSelector.Children.Add(start);
            _testingAdditionalSelector.Children.Add(cleanUp);
            _testingAdditionalSelector.Children.Add(showMenu);
            _testingAdditionalSelector.Children.Add(showGameplay);
            _testingAdditionalSelector.Children.Add(showTesting);
        }

        private void _gameplayScreenTest_Click(object sender, RoutedEventArgs e)
        {
            GameplayScreen gs = new();

            _testingSpace.Children.Clear();
            _testingAdditionalSelector.Children.Clear();

            _testingSpace.Children.Add(gs.Container);
            gs.Show();
            gs.AddPlayfield();

            StackPanel debugStats = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Width = 300,
                IsHitTestVisible = false,
            };

            TextBlock testDebugPA = new() { Text = $"GameplayManagers amount: {gs.GameplayManagers.Count}", Foreground = new SolidColorBrush(Colors.White), };
            TextBlock testDebugSC = new() { Text = $"Scale: {gs.ManagerScale}", Foreground = new SolidColorBrush(Colors.White), };

            Button start = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Show"
            };

            Button hide = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Hide"
            };

            Button clear = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Clear"
            };

            Button addPF = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Add Playfield"
            };

            Button removePF = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Remove Playfield"
            };

            Button addFoodToAll = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Add food to all"
            };

            Button removeFoodFromAll = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Remove food from all"
            };

            Button randomizePlayerColors = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Content = "Random Player Colors"
            };

            start.MouseEnter += Element_MouseEnter;
            start.MouseLeave += Element_MouseLeave;

            hide.MouseEnter += Element_MouseEnter;
            hide.MouseLeave += Element_MouseLeave;

            clear.MouseEnter += Element_MouseEnter;
            clear.MouseLeave += Element_MouseLeave;

            addPF.MouseEnter += Element_MouseEnter;
            addPF.MouseLeave += Element_MouseLeave;

            removePF.MouseEnter += Element_MouseEnter;
            removePF.MouseLeave += Element_MouseLeave;

            addFoodToAll.MouseEnter += Element_MouseEnter;
            addFoodToAll.MouseLeave += Element_MouseLeave;

            removeFoodFromAll.MouseEnter += Element_MouseEnter;
            removeFoodFromAll.MouseLeave += Element_MouseLeave;

            randomizePlayerColors.MouseEnter += Element_MouseEnter;
            randomizePlayerColors.MouseLeave += Element_MouseLeave;


            Slider changeScale = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new System.Windows.Thickness(2, 2, 2, 2),
                Height = 25,
                Value = gs.ManagerScale,
                IsSelectionRangeEnabled = true,
                Maximum = 500,
                Minimum = 0,
            };

            start.Click += (s, e) =>
            {
                gs.Show();
            };

            hide.Click += (s, e) =>
            {
                gs.Hide();
            };

            clear.Click += (s, e) =>
            {
                gs.Clear();
            };

            addPF.Click += (s, e) =>
            {
                gs.AddPlayfield();
            };

            removePF.Click += (s, e) =>
            {
                gs.RemovePlayfield();
            };

            addFoodToAll.Click += (s, e) =>
            {
                foreach (GameplaySession gmL in gs.GameplayManagers)
                {
                    gmL.AddFood();
                }
            };

            removeFoodFromAll.Click += (s, e) =>
            {
                foreach (GameplaySession gmL in gs.GameplayManagers)
                {
                    gmL.RemoveFood();
                }
            };

            randomizePlayerColors.Click += (s, e) =>
            {
                foreach (GameplaySession gmL in gs.GameplayManagers)
                {
                    Random rnd = new();

                    gmL.Player.ChangePlayerColor(Color.FromArgb(255, Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256))));
                }
            };

            changeScale.ValueChanged += (s, e) =>
            {
                gs.ChangeGameplayScaling(changeScale.Value);
            };

            gs.PlayfieldAmountChanged += (s, e) =>
            {
                testDebugPA.Text = $"Playfield amount: {e.Amount}";
            };

            gs.ScaleChanged += (s, e) =>
            {
                testDebugSC.Text = $"Scale: {e.Scale}";
            };

            debugStats.Children.Add(testDebugPA);
            debugStats.Children.Add(testDebugSC);

            _testingSpace.Children.Add(debugStats);

            _testingAdditionalSelector.Children.Add(start);
            _testingAdditionalSelector.Children.Add(hide);
            _testingAdditionalSelector.Children.Add(clear);
            _testingAdditionalSelector.Children.Add(addPF);
            _testingAdditionalSelector.Children.Add(removePF);
            _testingAdditionalSelector.Children.Add(addFoodToAll);
            _testingAdditionalSelector.Children.Add(removeFoodFromAll);
            _testingAdditionalSelector.Children.Add(randomizePlayerColors);
            _testingAdditionalSelector.Children.Add(changeScale);
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            _animate.Color((Button)sender, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            _animate.Color((Button)sender, Animate.ColorProperty.Foreground, Colors.Black, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
        }

        public void Clear()
        {
            _clear.Click -= OnClearButtonClicked;
            _playerTest.Click -= OnPlayerButtonClick;
            _foodTest.Click -= OnFoodtestClick;
            _gameplayManagerTest.Click -= OnGameplayManagerClick;
            _screenManagerTest.Click -= OnScreenManagerClick;
            _gameplayScreenTest.Click -= _gameplayScreenTest_Click;
            _customElementsTest.Click -= OnUITestClicked;
            _menuPanelTest.Click -= OnMenuPanelClick;
            _settingsPanelTest.Click -= OnSettingsPanelClick;

            _clear.MouseLeave -= Element_MouseLeave;
            _playerTest.MouseLeave -= Element_MouseLeave;
            _foodTest.MouseLeave -= Element_MouseLeave;
            _gameplayManagerTest.MouseLeave -= Element_MouseLeave;
            _screenManagerTest.MouseLeave -= Element_MouseLeave;
            _gameplayScreenTest.MouseLeave -= Element_MouseLeave;

            _clear.MouseEnter -= Element_MouseEnter;
            _playerTest.MouseEnter -= Element_MouseEnter;
            _foodTest.MouseEnter -= Element_MouseEnter;
            _gameplayManagerTest.MouseEnter -= Element_MouseEnter;
            _screenManagerTest.MouseEnter -= Element_MouseEnter;
            _gameplayScreenTest.MouseEnter -= Element_MouseEnter;

            _testingSelector.Children.Clear();
            _testing.Visibility = Visibility.Hidden;
            _testingSelectorSV.Content = null;
            _testingAdditionalSelectorSV.Content = null;
            _testingSpace.Children.Clear();
            _testing.Children.Clear();
            _isInitialized = false;
            _testing.IsHitTestVisible = false;
        }
    }
}
