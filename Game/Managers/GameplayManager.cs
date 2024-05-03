using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Viper.Game.Managers
{
    public class GameplayManager()
    {
        #region Player logic

        public event EventHandler PlayerMovingChanged, PlayerDirectionChanged, PointsChanged, BodyElementsCountChanged, FoodAmountChanged, PlayerXPositionChanged, PlayerYPositionChanged;

        private enum Event
        {
            MovingChanged,
            DirectionChanged,
            PointsChanged,
            BodyElementsChanged,
            FoodAmountChanged,
            PositionXChanged,
            PositionYChanged,
        }

        public const int ELEMENTS_SIZE = 30;

        private bool _isPlayerMoving;

        public bool IsPlayerMoving
        {
            get
            {
                return _isPlayerMoving;
            }
        }

        private string _playerDirection = "";

        public string PlayerDirection
        {
            get
            {
                return _playerDirection;
            }
        }

        private int _points = 0;

        public int Points
        {
            get
            {
                return _points;
            }
        }

        private List<Rectangle> _playerBody = new();

        public int PlayerBodyCount
        {
            get
            {
                return _playerBody.Count();
            }
        }

        private double _playerXpos, _playerYpos;

        public double PlayerXPosition
        {
            get
            {
                return _playerXpos;
            }
        }

        public double PlayerYPosition
        {
            get
            {
                return _playerYpos;
            }
        }

        public bool AreValueEventsEnabled = false;

        public void CleanUp()
        {
            _playerBody.Clear();
            _foods.Clear();
            _foodPositions.Clear();
            _isPlayerMoving = false;
            _playerDirection = "";
            _points = 0;
            _foodCounter = 0;


            BodyElementsCountChanged?.Invoke(this, new EventArgs());
            FoodAmountChanged?.Invoke(this, new EventArgs());
            PlayerDirectionChanged?.Invoke(this, new EventArgs());
            PointsChanged?.Invoke(this, new EventArgs());
        }

        public void UpdateAllEvents()
        {
            if (AreValueEventsEnabled)
            {
                BodyElementsCountChanged?.Invoke(this, new EventArgs());
                FoodAmountChanged?.Invoke(this, new EventArgs());
                PlayerDirectionChanged?.Invoke(this, new EventArgs());
                PointsChanged?.Invoke(this, new EventArgs());
                PlayerMovingChanged?.Invoke(this, new EventArgs());
            }
        }

        public Rectangle ShowPlayer(TranslateTransform startPosition, bool spawnMoving = false)
        {
            double currentPosX = startPosition.X, currentPosY = startPosition.Y;

            double playfieldLimitY = 0, playfieldLimitX = 0;

            bool isSizeSaved = false;

            double newPosX = 0, newPosY = 0;

            _isPlayerMoving = spawnMoving;

            if (AreValueEventsEnabled)
            {
                RaiseEvent(Event.MovingChanged);
            }

            void CreateNewPlayerBodyElement(bool isPlayerAlreadyShowing = false)
            {
                Rectangle playerBodyPart = new()
                {
                    Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = ELEMENTS_SIZE,
                    Width = ELEMENTS_SIZE,
                    RenderTransform = new TranslateTransform(-30, -30), // Spawn out off bounds.
                    Focusable = true,
                };

                Panel.SetZIndex(playerBodyPart, 3);

                _playerBody.Add(playerBodyPart);

                if (isPlayerAlreadyShowing)
                {
                    playerBodyPart.Focusable = false;
                    (_playerBody[0].Parent as Panel).Children.Add(playerBodyPart);
                }
                else
                {
                    playerBodyPart.Loaded += (s, e) =>
                    {
                        playerBodyPart.Focus();
                        playerBodyPart.RenderTransform = startPosition; // If the player just spawned, move it to a position in which is visible.
                    };
                }

                RaiseEvent(Event.BodyElementsChanged);
            }

            CreateNewPlayerBodyElement();

            async void PlayerMovementLoop()
            {
                int index = 0;

                RaiseEvent(Event.MovingChanged);

                while (IsPlayerMoving)
                {
                    if (!isSizeSaved)
                    {
                        playfieldLimitY = (_playerBody[index].Parent as FrameworkElement).Height - ELEMENTS_SIZE;
                        playfieldLimitX = (_playerBody[index].Parent as FrameworkElement).Width - ELEMENTS_SIZE;
                    }

                    if (PlayerDirection == "up")
                    {
                        if (currentPosY - ELEMENTS_SIZE < 0)
                        {
                            newPosY = playfieldLimitY;
                        }
                        else
                        {
                            newPosY = currentPosY - ELEMENTS_SIZE;
                        }
                    }
                    else if (PlayerDirection == "down")
                    {
                        if (currentPosY + ELEMENTS_SIZE > playfieldLimitY)
                        {
                            newPosY = 0;
                        }
                        else
                        {
                            newPosY = currentPosY + ELEMENTS_SIZE;
                        }
                    }
                    else if (PlayerDirection == "left")
                    {
                        if (currentPosX - ELEMENTS_SIZE < 0)
                        {
                            newPosX = playfieldLimitX;
                        }
                        else
                        {
                            newPosX = currentPosX - ELEMENTS_SIZE;
                        }
                    }
                    else if (PlayerDirection == "right")
                    {
                        if (currentPosX + ELEMENTS_SIZE > playfieldLimitX)
                        {
                            newPosX = 0;
                        }
                        else
                        {
                            newPosX = currentPosX + ELEMENTS_SIZE;
                        }
                    }

                    if (_foodPositions.Count > 0)
                    {
                        for (int i = 0; i < _foodPositions.Count; i++)
                        {
                            double foodXpos = GetFoodPosition(i).X, foodYpos = GetFoodPosition(i).Y;

                            if (currentPosX == foodXpos && currentPosY == foodYpos)
                            {
                                RePositionFood(i);
                                CreateNewPlayerBodyElement(true);
                                _points += 1;

                                RaiseEvent(Event.PointsChanged);
                            }
                        }
                    }

                    _playerXpos = newPosX;
                    _playerYpos = newPosY;

                    RaiseEvent(Event.PositionXChanged);
                    RaiseEvent(Event.PositionYChanged);

                    _playerBody[index].RenderTransform = new TranslateTransform(newPosX, newPosY);

                    currentPosY = newPosY;
                    currentPosX = newPosX;

                    if (index + 1 <= Points)
                    {
                        index+=1;
                    }
                    else
                    {
                        index = 0;
                    }

                    await Task.Delay(50);
                }
                RaiseEvent(Event.MovingChanged);
            }

            _playerBody[0].PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    if (e.Key == Key.Up && _playerDirection != "down")
                    {
                        _playerDirection = "up";
                    }
                    else if (e.Key == Key.Down && _playerDirection != "up")
                    {
                        _playerDirection = "down";
                    }
                    else if (e.Key == Key.Left && _playerDirection != "right")
                    {
                        _playerDirection = "left";
                    }
                    else if (e.Key == Key.Right && _playerDirection != "left")
                    {
                        _playerDirection = "right";
                    }

                    if (!_isPlayerMoving)
                    {
                        _isPlayerMoving = true;
                        PlayerMovementLoop();
                    }

                    RaiseEvent(Event.DirectionChanged);
                }
            };

            Application.Current.Exit += (s, e) =>
            {
                CleanUp();
            };

            return _playerBody[0];
        }
        #endregion

        #region Food logic

        private List<TranslateTransform> _foodPositions = new();

        private int _foodCounter = 0;

        private List<Rectangle> _foods = new();

        public int FoodAmount
        {
            get
            {
                return _foods.Count();
            }
        }

        public Rectangle AddFood()
        {
            int currentIndex = _foodCounter;

            Rectangle food = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 108, 245, 66)),
                Height = ELEMENTS_SIZE,
                Width = ELEMENTS_SIZE,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            food.Loaded += (s, e) =>
            {
                RePositionFood(currentIndex);
            };

            _foods.Add(food);
            _foodPositions.Add(new TranslateTransform());

            _foodCounter += 1;

            RaiseEvent(Event.FoodAmountChanged);

            return _foods[currentIndex];
        }

        public void RePositionFood(int foodIndex)
        {
            Random random = new Random();

            int spaceH = Convert.ToInt32((_foods[foodIndex].Parent as Panel).Height);
            int spaceW = Convert.ToInt32((_foods[foodIndex].Parent as Panel).Width);

            int newX = ELEMENTS_SIZE * random.Next(0, spaceH / ELEMENTS_SIZE);
            int newY = ELEMENTS_SIZE * random.Next(0, spaceW / ELEMENTS_SIZE);

            _foods[foodIndex].RenderTransform = new TranslateTransform(newX, newY);
            _foodPositions[foodIndex] = _foods[foodIndex].RenderTransform as TranslateTransform;
        }

        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _foodPositions[foodIndex];
        }
        #endregion

        private void RaiseEvent(Event selectedEvent)
        {
            if (AreValueEventsEnabled)
            {
                if (selectedEvent == Event.DirectionChanged)
                {
                    PlayerDirectionChanged?.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.MovingChanged)
                {
                    PlayerMovingChanged?.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.BodyElementsChanged)
                {
                    BodyElementsCountChanged?.Invoke(this, EventArgs.Empty);
                }
                else if (selectedEvent == Event.PointsChanged)
                {
                    PointsChanged?.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.PositionXChanged)
                {
                    PlayerXPositionChanged?.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.PositionYChanged)
                {
                    PlayerYPositionChanged?.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.FoodAmountChanged)
                {
                    FoodAmountChanged?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}