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

namespace Viper.Game.Elements
{
    public class Player
    {
        public event EventHandler? PlayerMovingChanged, PlayerDirectionChanged, BodyElementsCountChanged, PlayerPositionChanged, PlayerDied, TickrateChanged, ColorChanged;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }

        private bool _isPlayerMoving;

        public bool IsPlayerMoving
        {
            get
            {
                return _isPlayerMoving;
            }
        }

        private Direction? _playerDirection;

        public Direction? PlayerDirection
        {
            get
            {
                return _playerDirection;
            }
        }

        private List<Direction?> _directionBuffer = new();

        public int DirectionsBuffered
        {
            get
            {
                return _directionBuffer.Count();
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

        public const int SIZE = 30;

        private int _tickRate = 150;

        public int CurrentTickRate
        {
            get
            {
                return _tickRate;
            }
        }

        private bool _isPlayerAlreadyShowing = false;

        private Color Color = Color.FromArgb(255, 255, 255, 255);

        public string CurrentColor
        {
            get
            {
                return $"Color.FromArgb({Color.A}, {Color.R}, {Color.G}, {Color.B})";
            }
        }

        public bool AreValueEventsEnabled = false;

        public Rectangle AddPlayer()
        {
            IncreasePlayerSize();

            Application.Current.MainWindow.PreviewKeyDown += ChangeDirection;

            Application.Current.Exit += Current_Exit;

            RaiseAllEvents();

            return _playerBody[0];
        }

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
                    PlayerMovingChanged.Invoke(this, new EventArgs());
                    PlayerMovementLoop();
                }

                PlayerDirectionChanged.Invoke(this, new EventArgs());
            }
        }

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

                    Direction? currentDirection = _directionBuffer[0];

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

                    PlayerPositionChanged.Invoke(this, new EventArgs());

                    Debug.WriteLine(i);
                    _playerBody[i].RenderTransform = new TranslateTransform(newPosX, newPosY);

                    currentPosY = newPosY;
                    currentPosX = newPosX;

                    foreach (var bodyElement in _playerBody)
                    {
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

                    if (isPlayerDead)
                    {
                        break;
                    }

                    await Task.Delay(_tickRate);
                }

            }
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            CleanUp();
        }

        public void ChangeTickRate(int newTickRate)
        {
            if (newTickRate > 0)
            {
                _tickRate = newTickRate;
                TickrateChanged.Invoke(this, new EventArgs());
            }
        }

        public void IncreasePlayerSize()
        {
            Rectangle playerBodyPart = new()
            {
                Fill = new SolidColorBrush(Color),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = SIZE,
                Width = SIZE,
                RenderTransform = new TranslateTransform(-30, -30), // Spawn out of bounds.
                Focusable = true,
            };

            Panel.SetZIndex(playerBodyPart, 3);

            _playerBody.Add(playerBodyPart);

            if (!_isPlayerAlreadyShowing)
            {
                playerBodyPart.Loaded += (s, e) =>
                {
                    playerBodyPart.RenderTransform = new TranslateTransform(0, 0); // If the player just spawned, move it to a position in which is visible.
                };

                _isPlayerAlreadyShowing = true;
            }
            else
            {
                (_playerBody[0].Parent as Panel).Children.Add(playerBodyPart);
            }

            BodyElementsCountChanged.Invoke(this, EventArgs.Empty);
        }

        public void DecreasePlayerSize()
        {
            if (_isPlayerAlreadyShowing && _playerBody.Count - 1 > 0)
            {
                (_playerBody[0].Parent as Panel).Children.Remove(_playerBody[_playerBody.Count - 1]);
                _playerBody.RemoveAt(_playerBody.Count - 1);

                BodyElementsCountChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public void ChangePlayerColor(Color newColor)
        {
            Color = newColor;
            foreach (var player in _playerBody)
            {
                player.Fill = new SolidColorBrush(Color);
            }
            ColorChanged.Invoke(this, new EventArgs());
        }

        // Removes, resets and ends everything
        public void CleanUp(bool softClean = false)
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
            _playerDirection = null;
            _playerXpos = 0;
            _playerYpos = 0;

            RaiseAllEvents();
        }

        // Removes certain things to reset the player state.
        public void ResetGameplay()
        {
            Panel parent = _playerBody[0].Parent as Panel;

            CleanUp();
            parent.Children.Add(AddPlayer());
        }

        public void RaiseAllEvents()
        {
            if (AreValueEventsEnabled)
            {
                BodyElementsCountChanged.Invoke(this, EventArgs.Empty);
                PlayerDirectionChanged.Invoke(this, new EventArgs());
                PlayerPositionChanged.Invoke(this, new EventArgs());
                PlayerMovingChanged.Invoke(this, new EventArgs());
                TickrateChanged.Invoke(this, new EventArgs());
                ColorChanged.Invoke(this, new EventArgs());
            }
        }
    }
}
