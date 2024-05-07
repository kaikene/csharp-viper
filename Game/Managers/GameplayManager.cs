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

        public event EventHandler? PlayerMovingChanged, PlayerDirectionChanged, PointsChanged, BodyElementsCountChanged, FoodAmountChanged, PlayerXPositionChanged, PlayerYPositionChanged, PlayerDied;

        private enum Event
        {
            MovingChanged,
            DirectionChanged,
            PointsChanged,
            BodyElementsChanged,
            FoodAmountChanged,
            PositionXChanged,
            PositionYChanged,
            PlayerDies
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            None,
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

        private Direction _playerDirection = Direction.None;

        public Direction PlayerDirection
        {
            get
            {
                return _playerDirection;
            }
        }

        private List<Direction> _directionBuffer = new();

        public int DirectionsBuffered
        {
            get
            {
                return _directionBuffer.Count();
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

        // Removes, resets and ends everything
        public void CleanUp()
        {
            try
            {
                Panel parent = _playerBody[0].Parent as Panel;
                parent.Children.Clear();
            }
            catch
            {
                // Player is not loaded, nothing to remove.
            }

            _playerBody.Clear();
            _foods.Clear();
            _foodPositions.Clear();
            _playerDirection = Direction.None;
            _points = 0;
            _isPlayerMoving = false;
            _foodCounter = 0;


            RaiseAllEvents();
        }

        // Removes and resets everything, but reloads everything after it.
        private void ResetGameplay()
        {
            Panel parent = _playerBody[0].Parent as Panel;

            CleanUp();

            parent.Children.Add(ShowPlayer(new TranslateTransform(0, 0)));
            parent.Children.Add(AddFood());
        }

        public void RaiseAllEvents()
        {
            if (AreValueEventsEnabled)
            {
                RaiseEvent(Event.BodyElementsChanged);
                RaiseEvent(Event.FoodAmountChanged);
                RaiseEvent(Event.DirectionChanged);
                RaiseEvent(Event.PointsChanged);
                RaiseEvent(Event.MovingChanged);
            }
        }

        public Rectangle ShowPlayer(TranslateTransform startPosition)
        {
            double currentPosX = startPosition.X, currentPosY = startPosition.Y;

            double playfieldLimitY = 0, playfieldLimitX = 0;

            bool isSizeSaved = false;

            double newPosX = 0, newPosY = 0;

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

                bool isPlayerDead = false;

                RaiseEvent(Event.MovingChanged);

                while (_isPlayerMoving)
                {
                    // Direction buffer updater, moves directions "forward" so the List gets cleaned until just 1 direction is saved
                    if (_directionBuffer.Count > 1)
                    {
                        for (int i = 0; i < _directionBuffer.Count; i++)
                        {
                            if (i + 1 != _directionBuffer.Count)
                            {
                                _directionBuffer[i] = _directionBuffer[i + 1]; // db1 = db2, db2 = db3, db3 = db4, etc...
                            }
                            else
                            {
                                _directionBuffer.RemoveAt(i); // If we reach the last element of the list, remove it.
                            }
                        }
                    }

                    Direction currentDirection = _directionBuffer[0];

                    if (!isSizeSaved)
                    {
                        playfieldLimitY = (_playerBody[index].Parent as FrameworkElement).Height - ELEMENTS_SIZE;
                        playfieldLimitX = (_playerBody[index].Parent as FrameworkElement).Width - ELEMENTS_SIZE;
                    }

                    if (currentDirection == Direction.Up)
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
                    else if (currentDirection == Direction.Down)
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
                    else if (currentDirection == Direction.Left)
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
                    else if (currentDirection == Direction.Right)
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

                    _playerXpos = newPosX;
                    _playerYpos = newPosY;

                    RaiseEvent(Event.PositionXChanged);
                    RaiseEvent(Event.PositionYChanged);

                    _playerBody[index].RenderTransform = new TranslateTransform(newPosX, newPosY);

                    currentPosY = newPosY;
                    currentPosX = newPosX;

                    foreach (var bodyElement in _playerBody)
                    {
                        if (currentPosX == (bodyElement.RenderTransform as TranslateTransform).X && currentPosY == (bodyElement.RenderTransform as TranslateTransform).Y)
                        {
                            if (bodyElement != _playerBody[index])
                            {
                                PlayerDied.Invoke(this, new EventArgs());
                                isPlayerDead = true;
                                ResetGameplay();
                                break;
                            }
                        }
                    }

                    if (isPlayerDead)
                    {
                        break;
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

                    if (index + 1 <= Points)
                    {
                        index+=1;
                    }
                    else
                    {
                        index = 0;
                    }

                    if (_playerDirection == Direction.None)
                    {
                        _isPlayerMoving = false;
                    }

                    await Task.Delay(50);
                }
                RaiseEvent(Event.MovingChanged);
            }

            _playerBody[0].PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    if (e.Key == Key.Up && _playerDirection != Direction.Down)
                    {
                        _playerDirection = Direction.Up;
                    }
                    else if (e.Key == Key.Down && _playerDirection != Direction.Up)
                    {
                        _playerDirection = Direction.Down;
                    }
                    else if (e.Key == Key.Left && _playerDirection != Direction.Right)
                    {
                        _playerDirection = Direction.Left;
                    }
                    else if (e.Key == Key.Right && _playerDirection != Direction.Left)
                    {
                        _playerDirection = Direction.Right;
                    }

                    _directionBuffer.Add(_playerDirection);

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
                RenderTransform = new TranslateTransform(-30, -30),
            };

            _foods.Add(food);
            _foodPositions.Add(new TranslateTransform());

            food.Loaded += (s, e) =>
            {
                RePositionFood(currentIndex);
            };

            _foodCounter += 1;

            RaiseEvent(Event.FoodAmountChanged);

            return _foods[currentIndex];
        }

        public void RemoveFood()
        {
            int currentIndex = _foodCounter - 1;

            (_foods[currentIndex].Parent as Panel).Children.Remove(_foods[currentIndex]);

            _foods.RemoveAt(currentIndex);
            _foodPositions.RemoveAt(currentIndex);

            _foodCounter -= 1;

            RaiseEvent(Event.FoodAmountChanged);
        }

        public void RePositionFood(int foodIndex)
        {
            Random random = new Random();

            int spaceH = Convert.ToInt32((_foods[foodIndex].Parent as Panel).Height);
            int spaceW = Convert.ToInt32((_foods[foodIndex].Parent as Panel).Width);

            int newX, newY;

            CheckReposition();

            void CheckReposition()
            {
                newX = ELEMENTS_SIZE * random.Next(0, spaceH / ELEMENTS_SIZE);
                newY = ELEMENTS_SIZE * random.Next(0, spaceW / ELEMENTS_SIZE);

                foreach (var bodyElement in _playerBody)
                {
                    if (newX == (bodyElement.RenderTransform as TranslateTransform).X && newY == (bodyElement.RenderTransform as TranslateTransform).Y)
                    {
                        CheckReposition();
                    }
                }
            }

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
                    PlayerDirectionChanged.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.MovingChanged)
                {
                    PlayerMovingChanged.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.BodyElementsChanged)
                {
                    BodyElementsCountChanged.Invoke(this, EventArgs.Empty);
                }
                else if (selectedEvent == Event.PointsChanged)
                {
                    PointsChanged.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.PositionXChanged)
                {
                    PlayerXPositionChanged.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.PositionYChanged)
                {
                    PlayerYPositionChanged.Invoke(this, new EventArgs());
                }
                else if (selectedEvent == Event.FoodAmountChanged)
                {
                    FoodAmountChanged.Invoke(this, new EventArgs());
                }
            }
        }
    }
}