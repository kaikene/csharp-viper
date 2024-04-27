using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Viper.Game.Gameplay.Managers
{
    public class GameplayManager(Window window, Dispatcher dispatcher)
    {
        #region Player logic

        public const int ELEMENTS_SIZE = 30;

        public bool IsPlayerMoving;

        private string PlayerDirection = "";

        public int Points = 0;

        private List<TranslateTransform> _playerPositions = new();

        public List<Rectangle> PlayerBody = new();

        public Rectangle ShowPlayer(TranslateTransform startPosition, bool spawnMoving = false)
        {
            IsPlayerMoving = spawnMoving;

            for (int i = 0; i < 2; i++)
            {
                Rectangle playerBodyPart = new()
                {
                    Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = ELEMENTS_SIZE,
                    Width = ELEMENTS_SIZE,
                    Tag = i,
                    RenderTransform = startPosition,
                    Focusable = true,
                };

                Panel.SetZIndex(playerBodyPart, 2);

                PlayerBody.Add(playerBodyPart);
            }

            double currentPosX = startPosition.X, currentPosY = startPosition.Y;
            double playfieldLimitY = 0, playfieldLimitX = 0;
            bool isSizeSaved = false;
            double newPosX = 0, newPosY = 0;

            Thread playerMovement = new(() =>
            {
                while (IsPlayerMoving)
                {
                    dispatcher.Invoke(() =>
                    {
                        if (!isSizeSaved)
                        {
                            playfieldLimitY = (PlayerBody[0].Parent as FrameworkElement).Height - ELEMENTS_SIZE;
                            playfieldLimitX = (PlayerBody[0].Parent as FrameworkElement).Width - ELEMENTS_SIZE;
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
                                    Points += 1;
                                }
                            }
                        }

                        PlayerBody[0].RenderTransform = new TranslateTransform(newPosX, newPosY);

                        currentPosY = newPosY;
                        currentPosX = newPosX;
                    });
                    Thread.Sleep(50);
                }
            });

            PlayerBody[0].PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    if (e.Key == Key.Up)
                    {
                        PlayerDirection = "up";
                    }
                    else if (e.Key == Key.Down)
                    {
                        PlayerDirection = "down";
                    }
                    else if (e.Key == Key.Left)
                    {
                        PlayerDirection = "left";
                    }
                    else if (e.Key == Key.Right)
                    {
                        PlayerDirection = "right";
                    }

                    try
                    {
                        IsPlayerMoving = true;
                        playerMovement.Start();
                    }
                    catch
                    {
                        // Thread is already running (player is moving).
                    }
                }
            };

            window.Closed += (s, e) =>
            {
                IsPlayerMoving = false;
            };

            return PlayerBody[0];
        }
        #endregion

        #region Food logic

        private List<TranslateTransform> _foodPositions = new();

        private int _foodCounter = -1;

        public List<Rectangle> Foods = new();

        public Rectangle AddFood()
        {
            _foodCounter += 1;

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

            Foods.Add(food);
            _foodPositions.Add(new TranslateTransform());

            return Foods[currentIndex];
        }

        public void RePositionFood(int foodIndex)
        {
            Random random = new Random();

            int spaceH = Convert.ToInt32((Foods[foodIndex].Parent as Panel).Height);
            int spaceW = Convert.ToInt32((Foods[foodIndex].Parent as Panel).Width);

            int newX = ELEMENTS_SIZE * random.Next(0, spaceH / ELEMENTS_SIZE);
            int newY = ELEMENTS_SIZE * random.Next(0, spaceW / ELEMENTS_SIZE);

            Foods[foodIndex].RenderTransform = new TranslateTransform(newX, newY);
            _foodPositions[foodIndex] = Foods[foodIndex].RenderTransform as TranslateTransform;
        }

        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _foodPositions[foodIndex];
        }
    }
    #endregion
}