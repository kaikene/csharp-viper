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

namespace Viper.Game.Elements
{
    /// <summary>
    /// Add, remove and manage a player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Triggers when the player dies.
        /// </summary>
        public event EventHandler? PlayerDied;

        /// <summary>
        /// Triggers when the player changes position.
        /// </summary>
        public event EventHandler<PlayerPositionChangedEventArgs>? PositionChanged;

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
        public Direction PlayerDirection
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

        // List of the player elements
        private List<Rectangle> _playerBody = new();

        /// <summary>
        /// Gets the current amount of player elements.
        /// </summary>
        public int PlayerBodyCount
        {
            get
            {
                return _playerBody.Count();
            }
        }

        // Player position.
        private double _playerXpos, _playerYpos;

        /// <summary>
        /// Current player position in the X axis.
        /// </summary>
        public double PlayerXPosition
        {
            get
            {
                return _playerXpos;
            }
        }

        /// <summary>
        /// Current player position in the Y axis.
        /// </summary>
        public double PlayerYPosition
        {
            get
            {
                return _playerYpos;
            }
        }

        /// <summary>
        /// Size of the player elements.
        /// </summary>
        public const int SIZE = 30;

        // Tick rate.
        private int _tickRate = 150;

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

        private bool _isPlayerAlreadyShowing = false;

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

        /// <summary>
        /// Adds a player.
        /// </summary>
        /// <returns></returns>
        public Rectangle AddPlayer()
        {
            IncreasePlayerSize();

            Application.Current.MainWindow.PreviewKeyDown += ChangeDirection;

            Application.Current.Exit += Current_Exit;

            RaiseAllEvents();

            return _playerBody[0];
        }

        // Changes the direction depending on the key pressed.
        private void ChangeDirection(object sender, KeyEventArgs e)
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

                    double playfieldLimitY = (_playerBody[0].Parent as FrameworkElement).Height - SIZE;
                    double playfieldLimitX = (_playerBody[0].Parent as FrameworkElement).Width - SIZE;

                    if (currentDirection == Direction.Up)
                    {
                        if (currentPosY - SIZE < 0)
                        {
                            newPosY = playfieldLimitY;
                        }
                        else
                        {
                            newPosY = currentPosY - SIZE;
                        }
                    }
                    else if (currentDirection == Direction.Down)
                    {
                        if (currentPosY + SIZE > playfieldLimitY)
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
                            newPosX = playfieldLimitX;
                        }
                        else
                        {
                            newPosX = currentPosX - SIZE;
                        }
                    }
                    else if (currentDirection == Direction.Right)
                    {
                        if (currentPosX + SIZE > playfieldLimitX)
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
                                PlayerDied?.Invoke(this, new EventArgs());
                                isPlayerDead = true;
                                ResetGameplay();
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

        // Stop loops and clean things in case the program is closed abruptly.
        private void Current_Exit(object sender, ExitEventArgs e)
        {
            CleanUp();
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

            if (!_isPlayerAlreadyShowing) // If the player is not showing, then that means no elements have been loaded
            {
                playerBodyPart.Loaded += (s, e) =>
                {
                    playerBodyPart.RenderTransform = new TranslateTransform(0, 0); // Move it to a position in which is visible as soon as is loaded.
                };

                _isPlayerAlreadyShowing = true; // Now is visible.
            }
            else
            {
                (_playerBody[0].Parent as Panel).Children.Add(playerBodyPart); // If is already being shown, then just add a new element to grow the player size.
            }

            BodyElementsCountChanged?.Invoke(this, new PlayerBodyElementsCountChangedEventArgs(_playerBody.Count));
        }

        public void DecreasePlayerSize()
        {
            if (_isPlayerAlreadyShowing && _playerBody.Count - 1 > 0)
            {
                (_playerBody[0].Parent as Panel).Children.Remove(_playerBody[_playerBody.Count - 1]); // Remove the last player element from the panel.
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

        // Removes, resets and ends everything
        public void CleanUp()
        {
            Application.Current.MainWindow.PreviewKeyDown -= ChangeDirection;
            Application.Current.Exit -= Current_Exit;
            _isPlayerMoving = false;

            try
            {
                Panel parent = _playerBody[0].Parent as Panel;

                foreach (Rectangle playerBody in _playerBody)
                {
                    parent.Children.Remove(playerBody);
                }
            }
            catch
            {
                // Player is not loaded, nothing to remove.
            }

            _isPlayerAlreadyShowing = false;
            _playerBody.Clear();
            _directionBuffer.Clear();
            _playerXpos = 0;
            _playerYpos = 0;

            RaiseAllEvents();
        }

        /// <summary>
        /// Resets the player state.
        /// </summary>
        public void ResetGameplay()
        {
            Panel parent = _playerBody[0].Parent as Panel;

            CleanUp();
            parent.Children.Add(AddPlayer());
        }

        public void RaiseAllEvents()
        {
            if (CanTriggerValueEvents)
            {
                BodyElementsCountChanged?.Invoke(this, new PlayerBodyElementsCountChangedEventArgs(_playerBody.Count));
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(_playerDirection));
                PositionChanged?.Invoke(this, new PlayerPositionChangedEventArgs(_playerXpos, _playerYpos));
                IsMovingChanged?.Invoke(this, new PlayerMovingChangedEventArgs(_isPlayerMoving));
                TickrateChanged?.Invoke(this, new PlayerTickRateChangedEventArgs(_tickRate));
                ColorChanged?.Invoke(this, new PlayerColorChangedEventArgs(_color));
            }
        }
    }
}
