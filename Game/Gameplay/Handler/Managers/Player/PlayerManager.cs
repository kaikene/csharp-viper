using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Viper.Game.Gameplay.Handler.Managers.Player
{
    public class PlayerManager(Window window, Dispatcher dispatcher, FoodManager fm, int playerSize)
    {
        private int _playerCounter = -1;

        public int PlayerAmount
        {
            get
            {
                return _playerCounter;
            }
        }

        public List<Rectangle> Players = new();

        public List<bool> IsPlayerMoving = new();

        public List<string> PlayerDirections = new();

        public List<int> PlayerPoints = new();

        public int PlayerSize { get; } = playerSize;

        public Rectangle Add(bool spawnMoving = false)
        {
            _playerCounter += 1;

            int currentIndex = _playerCounter;

            IsPlayerMoving.Add(spawnMoving);

            PlayerDirections.Add("");

            PlayerPoints.Add(0);

            Rectangle player = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = playerSize,
                Width = playerSize,
                RenderTransform = new TranslateTransform(0, 0),
                Focusable = true,
            };

            Thread playerMovement = new(() =>
            {
                while (IsPlayerMoving[currentIndex])
                {
                    dispatcher.Invoke(() =>
                    {
                        TranslateTransform currentPosition = player.RenderTransform as TranslateTransform;

                        if (PlayerDirections[currentIndex] == "up")
                        {
                            if (currentPosition.Y - playerSize < 0)
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, (player.Parent as FrameworkElement).Height - PlayerSize);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, currentPosition.Y - playerSize);
                            }
                        }
                        else if (PlayerDirections[currentIndex] == "down")
                        {
                            if (currentPosition.Y + playerSize > (player.Parent as FrameworkElement).Height - PlayerSize)
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, 0);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, currentPosition.Y + playerSize);
                            }
                        }
                        else if (PlayerDirections[currentIndex] == "left")
                        {
                            if (currentPosition.X - playerSize < 0)
                            {
                                player.RenderTransform = new TranslateTransform((player.Parent as FrameworkElement).Width - PlayerSize, currentPosition.Y);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X - playerSize, currentPosition.Y);
                            }
                        }
                        else if (PlayerDirections[currentIndex] == "right")
                        {
                            if (currentPosition.X + playerSize > (player.Parent as FrameworkElement).Width - PlayerSize)
                            {
                                player.RenderTransform = new TranslateTransform(0, currentPosition.Y);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X + playerSize, currentPosition.Y);
                            }
                        }

                        double playerXpos = (player.RenderTransform as TranslateTransform).X, playerYpos = (player.RenderTransform as TranslateTransform).Y;
                        double foodXpos = fm.GetFoodPosition(currentIndex).X, foodYpos = fm.GetFoodPosition(currentIndex).Y;

                        if (playerXpos == foodXpos && playerYpos == foodYpos)
                        {
                            fm.RePosition(currentIndex);
                        }
                    });
                    Thread.Sleep(50);
                }
            });

            player.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    if (e.Key == Key.Up)
                    {
                        PlayerDirections[currentIndex] = "up";
                    }
                    else if (e.Key == Key.Down)
                    {
                        PlayerDirections[currentIndex] = "down";
                    }
                    else if (e.Key == Key.Left)
                    {
                        PlayerDirections[currentIndex] = "left";
                    }
                    else if (e.Key == Key.Right)
                    {
                        PlayerDirections[currentIndex] = "right";
                    }

                    try
                    {
                        IsPlayerMoving[currentIndex] = true;
                        playerMovement.Start();
                    }
                    catch
                    {
                        // Thread is already running (player is moving).
                    }
                }
            };
            Players.Add(player);

            window.Closed += (s, e) =>
            {
                IsPlayerMoving[currentIndex] = false;
            };

            return player;
        }
    }
}
