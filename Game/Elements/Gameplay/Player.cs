using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Reflection;
using System.Numerics;
using System.Diagnostics;
using Viper.Game.Events;
using Viper.Game.Interfaces;

namespace Viper.Game.Elements.Gameplay
{
    /// <summary>
    /// Add, remove and manage a player.
    /// </summary>
    public class Player : IGameplayElements
    {
        /// <summary>
        /// Triggers when the player dies.
        /// </summary>
        public event EventHandler<PlayerDiedEventArgs>? Died;

        /// <summary>
        /// Triggers when the player lives change value.
        /// </summary>
        public event EventHandler<PlayerLivesChangedEventArgs>? LivesChanged;

        /// <summary>
        /// Triggers when the player changes position.
        /// </summary>
        public event EventHandler<PlayerPositionChangedEventArgs>? PositionChanged;

        /// <summary>
        /// Triggers when the player changes inputs.
        /// </summary>
        public event EventHandler<PlayerInputChangedEventArgs>? InputChanged;

        /// <summary>
        /// Triggers when the player is moving or not
        /// </summary>
        public event EventHandler<PlayerMovingChangedEventArgs>? IsMovingChanged;

        /// <summary>
        /// Triggers when the player grows or shrinks
        /// </summary>
        public event EventHandler<PlayerBodyElementsCountChangedEventArgs>? BodyElementsCountChanged;

        /// <summary>
        /// Triggers when the tick rate is changed.
        /// </summary>
        public event EventHandler<PlayerTickRateChangedEventArgs>? TickrateChanged;

        /// <summary>
        /// Triggers when the player direction is changed.
        /// </summary>
        public event EventHandler<PlayerDirectionChangedEventArgs>? DirectionChanged;

        /// <summary>
        /// Triggers when the player color is changed.
        /// </summary>
        public event EventHandler<PlayerColorChangedEventArgs>? ColorChanged;

        /// <summary>
        /// Used to determine the direction of the player.
        /// </summary>
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }

        private bool _isPlayerMoving; // bool used for the player loop, if its false, then the loop ends.

        /// <summary>
        /// Gets info about if the player is currently moving.
        /// </summary>
        public bool IsPlayerMoving
        {
            get
            {
                return _isPlayerMoving;
            }
        }

        // Current player direction
        private Direction _playerDirection;

        /// <summary>
        /// Gets the current direction tha player is heading.
        /// </summary>
        public Direction CurrentDirection
        {
            get
            {
                return _playerDirection;
            }
        }

        // A list of directions saved when the player pressed the movement keys a bit too much
        private List<Direction> _directionBuffer = new();

        /// <summary>
        /// Shows the amount of inputs that got buffered in case the player presses a lot of keys in a short time.
        /// </summary>
        public int DirectionsBuffered
        {
            get
            {
                return _directionBuffer.Count();
            }
        }

        private int _deathCounter = 0;

        public int DeathCounter
        {
            get
            {
                return _deathCounter;
            }
        }

        // List of the player elements
        private List<Rectangle> _playerBody = new();

        /// <summary>
        /// Gets the current amount of player elements.
        /// </summary>
        public List<Rectangle> BodyElements
        {
            get
            {
                return _playerBody;
            }
        }

        // Player position.
        private double _playerXpos, _playerYpos;

        /// <summary>
        /// Current player position in the X axis.
        /// </summary>
        public double XPosition
        {
            get
            {
                return _playerXpos;
            }
        }

        private int _lives = DEFAULT_LIVES;

        /// <summary>
        /// Current player position in the X axis.
        /// </summary>
        public int Lives
        {
            get
            {
                return _lives;
            }

            set
            {
                if (value >= 1)
                {
                    _lives = value;
                }
            }
        }

        /// <summary>
        /// Current player position in the Y axis.
        /// </summary>
        public double YPosition
        {
            get
            {
                return _playerYpos;
            }
        }

        /// <summary>
        /// Size of the player elements.
        /// </summary>
        public const int SIZE = 10;

        public const int DEFAULT_LIVES = 2;

        public const int DEFAULT_TICKRATE = 100;

        public const byte DEFAULT_BODY_ALPHA = 255;

        public const byte DEFAULT_BODY_RED = 255;

        public const byte DEFAULT_BODY_GREEN = 255;

        public const byte DEFAULT_BODY_BLUE = 255;

        public const byte DEFAULT_STROKE_ALPHA = 0;

        public const byte DEFAULT_STROKE_RED = 0;

        public const byte DEFAULT_STROKE_GREEN = 0;

        public const byte DEFAULT_STROKE_BLUE = 0;

        // Tick rate.
        private int _tickRate = 50;

        /// <summary>
        /// Gets the current tickrate being used at the moment.
        /// </summary>
        public int CurrentTickRate
        {
            get
            {
                return _tickRate;
            }
        }

        private bool _isShowing = false;

        // Player color.
        private Color _color = Color.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Gets the current player colot being used at the moment.
        /// </summary>
        public Color CurrentColor
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// Determines if value change events can be triggered, like if the amount of food changes or if a food changed position, etc.
        /// </summary>
        public bool CanTriggerValueEvents = false;

        private Key _inputUp = Key.Up;

        private Key _inputDown = Key.Down;

        private Key _inputLeft = Key.Left;

        private Key _inputRight = Key.Right;

        public Key InputUp
        {
            get
            {
                return _inputUp;
            }

            set
            {
                _inputUp = value;
                InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(PlayerInputChangedEventArgs.Input.Up, value));
            }
        }

        public Key InputDown
        {
            get
            {
                return _inputDown;
            }

            set
            {
                _inputDown = value;
                InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(PlayerInputChangedEventArgs.Input.Down, value));
            }
        }

        public Key InputLeft
        {
            get
            {
                return _inputLeft;
            }

            set
            {
                _inputLeft = value;
                InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(PlayerInputChangedEventArgs.Input.Left, value));
            }
        }

        public Key InputRight
        {
            get
            {
                return _inputRight;
            }

            set
            {
                _inputRight = value;
                InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(PlayerInputChangedEventArgs.Input.Right, value));
            }
        }

        private Panel _parent;

        /// <summary>
        /// Adds a player.
        /// </summary>
        /// <returns></returns>
        public void Show(Panel panel)
        {
            if (!_isShowing)
            {
                _parent = panel;

                IncreasePlayerSize();

                Application.Current.MainWindow.PreviewKeyDown += ChangeDirection;

                Application.Current.Exit += Current_Exit;

                BodyElementsCountChanged?.Invoke(this, new PlayerBodyElementsCountChangedEventArgs(_playerBody.Count));
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(_playerDirection));
                PositionChanged?.Invoke(this, new PlayerPositionChangedEventArgs(_playerXpos, _playerYpos));
                IsMovingChanged?.Invoke(this, new PlayerMovingChangedEventArgs(_isPlayerMoving));
                TickrateChanged?.Invoke(this, new PlayerTickRateChangedEventArgs(_tickRate));
                ColorChanged?.Invoke(this, new PlayerColorChangedEventArgs(_color));

                panel.Children.Add(_playerBody[0]);
            }
            else
            {
                throw new Exception("Player is already being shown!");
            }
        }

        // Changes the direction depending on the key pressed.
        private void ChangeDirection(object sender, KeyEventArgs e)
        {
            if (e.Key == _inputUp || e.Key == _inputDown || e.Key == _inputLeft || e.Key == _inputRight)
            {
                if (e.Key == _inputUp && _playerBody.Count == 1 || e.Key == _inputUp && _playerDirection != Direction.Down)
                {
                    _playerDirection = Direction.Up;
                }
                else if (e.Key == _inputDown && _playerBody.Count == 1 || e.Key == _inputDown && _playerDirection != Direction.Up)
                {
                    _playerDirection = Direction.Down;
                }
                else if (e.Key == _inputLeft && _playerBody.Count == 1 || e.Key == _inputLeft && _playerDirection != Direction.Right)
                {
                    _playerDirection = Direction.Left;
                }
                else if (e.Key == _inputRight && _playerBody.Count == 1 || e.Key == _inputRight && _playerDirection != Direction.Left)
                {
                    _playerDirection = Direction.Right;
                }

                _directionBuffer.Add(_playerDirection);

                if (!_isPlayerMoving)
                {
                    _isPlayerMoving = true;
                    IsMovingChanged?.Invoke(this, new PlayerMovingChangedEventArgs(_isPlayerMoving));
                    PlayerMovementLoop();
                }

                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(_playerDirection));
            }
        }

        // Loop that manages the entire player logic, updating its position depending on the key input and handling when it colides with itself.
        private async void PlayerMovementLoop()
        {
            double currentPosX = 0, currentPosY = 0;

            double newPosX = 0, newPosY = 0;

            bool isPlayerDead = false;

            double playfieldLimitY = _parent.Height;
            double playfieldLimitX = _parent.Width;

            double currentSavedHeight = _parent.Height;
            double currentSavedWidth = _parent.Width;

            CheckMaxUsableSpace();

            void CheckMaxUsableSpace()
            {
                if (playfieldLimitY % SIZE != 0)
                {
                    playfieldLimitY--;
                    CheckMaxUsableSpace();
                }

                if (playfieldLimitX % SIZE != 0)
                {
                    playfieldLimitX--;
                    CheckMaxUsableSpace();
                }
            }

            while (_isPlayerMoving)
            {
                for (int i = 0; i < _playerBody.Count; i++)
                {
                    // Direction buffer updater, moves directions "forward" so the List gets cleaned until just 1 direction is saved
                    if (_directionBuffer.Count > 1)
                    {
                        for (int x = 0; x < _directionBuffer.Count; x++)
                        {
                            if (x + 1 != _directionBuffer.Count)
                            {
                                _directionBuffer[x] = _directionBuffer[x + 1]; // db1 = db2, db2 = db3, db3 = db4, etc...
                            }
                            else
                            {
                                _directionBuffer.RemoveAt(x); // If we reach the last element of the list, remove it.
                            }
                        }
                    }

                    Direction currentDirection = _directionBuffer[0];

                    if (currentSavedHeight != _parent.Height)
                    {
                        playfieldLimitY = _parent.Height;

                        CheckMaxUsableSpace();

                        currentSavedHeight = _parent.Height;
                    }

                    if (currentSavedWidth != _parent.Width)
                    {
                        playfieldLimitX = _parent.Width;

                        CheckMaxUsableSpace();

                        currentSavedWidth = _parent.Width;
                    }


                    if (currentDirection == Direction.Up)
                    {
                        if (currentPosY - SIZE < 0)
                        {
                            newPosY = playfieldLimitY - SIZE; // Because of aligments, elements can still appear slightly out of bounds, this makes sure that it doesnt happend by avoiding the last possible bottom Y axis.
                        }
                        else
                        {
                            newPosY = currentPosY - SIZE;
                        }
                    }
                    else if (currentDirection == Direction.Down)
                    {
                        if (currentPosY + SIZE > playfieldLimitY - SIZE)
                        {
                            newPosY = 0;
                        }
                        else
                        {
                            newPosY = currentPosY + SIZE;
                        }
                    }
                    else if (currentDirection == Direction.Left)
                    {
                        if (currentPosX - SIZE < 0)
                        {
                            newPosX = playfieldLimitX - SIZE; // Same comment as above but avoids the last far right X axis.
                        }
                        else
                        {
                            newPosX = currentPosX - SIZE;
                        }
                    }
                    else if (currentDirection == Direction.Right)
                    {
                        if (currentPosX + SIZE > playfieldLimitX - SIZE)
                        {
                            newPosX = 0;
                        }
                        else
                        {
                            newPosX = currentPosX + SIZE;
                        }
                    }

                    _playerXpos = newPosX;
                    _playerYpos = newPosY;

                    PositionChanged?.Invoke(this, new PlayerPositionChangedEventArgs(_playerXpos, _playerYpos));

                    // Apply new position to the last player element
                    _playerBody[i].RenderTransform = new TranslateTransform(newPosX, newPosY);

                    currentPosY = newPosY;
                    currentPosX = newPosX;

                    foreach (var bodyElement in _playerBody)
                    {
                        // If the new position is on the list of player elements positions, then that means you crashed into yourself.
                        if (currentPosX == (bodyElement.RenderTransform as TranslateTransform).X && currentPosY == (bodyElement.RenderTransform as TranslateTransform).Y)
                        {
                            if (bodyElement != _playerBody[i])
                            {
                                _deathCounter++;
                                _lives--;
                                LivesChanged?.Invoke(this, new PlayerLivesChangedEventArgs(_lives));
                                Died?.Invoke(this, new PlayerDiedEventArgs(_deathCounter));
                                isPlayerDead = true;
                                Reset();

                                break;
                            }
                        }
                    }

                    // If dead = gtfo
                    if (isPlayerDead)
                    {
                        break;
                    }

                    await Task.Delay(_tickRate);
                }

            }
        }

        // Stop loops and remove things in case the program is closed abruptly.
        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Remove();
        }

        public void ChangeTickRate(int newTickRate)
        {
            if (newTickRate > 0)
            {
                _tickRate = newTickRate;
                TickrateChanged?.Invoke(this, new PlayerTickRateChangedEventArgs(_tickRate));
            }
        }

        public void IncreasePlayerSize()
        {
            Rectangle playerBodyPart = new()
            {
                Fill = new SolidColorBrush(_color),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = SIZE,
                Width = SIZE,
                RenderTransform = new TranslateTransform(-30, -30), // Spawn out of bounds.
                Focusable = true,
            };

            Panel.SetZIndex(playerBodyPart, 3);

            _playerBody.Add(playerBodyPart);

            if (!_isShowing) // If the player is not showing, then that means no elements have been loaded
            {
                playerBodyPart.RenderTransform = new TranslateTransform(0, 0); // Move it to a position in which is visible as soon as is loaded.

                _isShowing = true; // Now is visible.
            }
            else
            {
                _parent.Children.Add(playerBodyPart); // If is already being shown, then just add a new element to grow the player size.
            }

            BodyElementsCountChanged?.Invoke(this, new PlayerBodyElementsCountChangedEventArgs(_playerBody.Count));
        }

        public void DecreasePlayerSize()
        {
            if (_isShowing && _playerBody.Count - 1 > 0)
            {
                _parent.Children.Remove(_playerBody[_playerBody.Count - 1]); // Remove the last player element from the panel.
                _playerBody.RemoveAt(_playerBody.Count - 1); // Remove from the list.

                BodyElementsCountChanged?.Invoke(this, new PlayerBodyElementsCountChangedEventArgs(_playerBody.Count));
            }
        }

        public void ChangePlayerColor(Color newColor)
        {
            _color = newColor;
            foreach (var player in _playerBody)
            {
                player.Fill = new SolidColorBrush(_color);
            }
            ColorChanged?.Invoke(this, new PlayerColorChangedEventArgs(_color));
        }

        public void Remove()
        {
            ResetOrRemove(true);
        }

        public void Reset()
        {
            ResetOrRemove(false);
        }

        private void ResetOrRemove(bool doRemoval)
        {
            // If theres more than 0 player elements, that means the player is being shown and theres something to remove.
            if (_playerBody.Count > 0)
            {
                _isPlayerMoving = false;
                _directionBuffer.Clear();
                _playerXpos = 0;
                _playerYpos = 0;
                _deathCounter = 0;

                if (_lives == 0)
                {
                    _lives = DEFAULT_LIVES;
                }

                if (doRemoval)
                {
                    foreach (Rectangle playerBody in _playerBody)
                    {
                        _parent.Children.Remove(playerBody);
                    }

                    Application.Current.MainWindow.PreviewKeyDown -= ChangeDirection;
                    Application.Current.Exit -= Current_Exit;

                    InputUp = Key.Up;
                    InputDown = Key.Down;
                    InputLeft = Key.Left;
                    InputRight = Key.Right;

                    _playerBody.Clear();
                    _isShowing = false;
                }
                else
                {
                    int amountOfPlayerBodyElements = _playerBody.Count;

                    for (int i = amountOfPlayerBodyElements - 1; i >= 0; i--)
                    {
                        if (i != 0)
                        {
                            _parent.Children.Remove(_playerBody[i]);
                            _playerBody.Remove(_playerBody[i]);
                        }
                    }

                    _playerBody[0].RenderTransform = new TranslateTransform(_playerXpos, _playerYpos);
                }

                LivesChanged?.Invoke(this, new PlayerLivesChangedEventArgs(_lives));
                BodyElementsCountChanged?.Invoke(this, new PlayerBodyElementsCountChangedEventArgs(_playerBody.Count));
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(_playerDirection));
                PositionChanged?.Invoke(this, new PlayerPositionChangedEventArgs(_playerXpos, _playerYpos));
                IsMovingChanged?.Invoke(this, new PlayerMovingChangedEventArgs(_isPlayerMoving));
                TickrateChanged?.Invoke(this, new PlayerTickRateChangedEventArgs(_tickRate));
                ColorChanged?.Invoke(this, new PlayerColorChangedEventArgs(_color));
                Died?.Invoke(this, new PlayerDiedEventArgs(_deathCounter));
            }
        }
    }
}
