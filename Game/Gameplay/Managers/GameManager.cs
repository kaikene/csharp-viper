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
using System.Windows.Threading;

namespace Viper.Game.Gameplay.Managers
{
    public class GameManager(Window window, Dispatcher dispatcher)
    {
        private int _playerCounter = -1;

        private int _foodCounter = -1;

        public int PlayerAmount
        {
            get
            {
                return _playerCounter;
            }
        }

        public int FoodAmount
        {
            get
            {
                return _foodCounter;
            }
        }

        public List<Rectangle> Players = new();

        public List<Rectangle> Foods = new();

        public List<bool> IsPlayerMoving = new();

        public List<string> PlayerDirections = new();

        public List<int> PlayerPoints = new();

        public const int ELEMENTS_SIZE = 30;

        private List<TranslateTransform> _positions = new();

        public Rectangle AddPlayer(bool spawnMoving = false)
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
                Height = ELEMENTS_SIZE,
                Width = ELEMENTS_SIZE,
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
                            if (currentPosition.Y - ELEMENTS_SIZE < 0)
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, (player.Parent as FrameworkElement).Height - ELEMENTS_SIZE);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, currentPosition.Y - ELEMENTS_SIZE);
                            }
                        }
                        else if (PlayerDirections[currentIndex] == "down")
                        {
                            if (currentPosition.Y + ELEMENTS_SIZE > (player.Parent as FrameworkElement).Height - ELEMENTS_SIZE)
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, 0);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X, currentPosition.Y + ELEMENTS_SIZE);
                            }
                        }
                        else if (PlayerDirections[currentIndex] == "left")
                        {
                            if (currentPosition.X - ELEMENTS_SIZE < 0)
                            {
                                player.RenderTransform = new TranslateTransform((player.Parent as FrameworkElement).Width - ELEMENTS_SIZE, currentPosition.Y);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X - ELEMENTS_SIZE, currentPosition.Y);
                            }
                        }
                        else if (PlayerDirections[currentIndex] == "right")
                        {
                            if (currentPosition.X + ELEMENTS_SIZE > (player.Parent as FrameworkElement).Width - ELEMENTS_SIZE)
                            {
                                player.RenderTransform = new TranslateTransform(0, currentPosition.Y);
                            }
                            else
                            {
                                player.RenderTransform = new TranslateTransform(currentPosition.X + ELEMENTS_SIZE, currentPosition.Y);
                            }
                        }

                        double playerXpos = (player.RenderTransform as TranslateTransform).X, playerYpos = (player.RenderTransform as TranslateTransform).Y;
                        double foodXpos = GetFoodPosition(currentIndex).X, foodYpos = GetFoodPosition(currentIndex).Y;

                        if (playerXpos == foodXpos && playerYpos == foodYpos)
                        {
                            RePositionFood(currentIndex);
                            PlayerPoints[currentIndex] += 1;
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

        public Rectangle AddFood()
        {
            _positions.Add(new TranslateTransform());

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

            return food;
        }

        public void RePositionFood(int index)
        {
            Random random = new Random();

            int spaceH = Convert.ToInt32((Foods[index].Parent as Panel).Height);
            int spaceW = Convert.ToInt32((Foods[index].Parent as Panel).Width);

            int newX = ELEMENTS_SIZE * random.Next(0, spaceH / ELEMENTS_SIZE);
            int newY = ELEMENTS_SIZE * random.Next(0, spaceW / ELEMENTS_SIZE);

            Foods[index].RenderTransform = new TranslateTransform(newX, newY);
            _positions[index] = Foods[index].RenderTransform as TranslateTransform;
        }

        public Rectangle SelectFood(int foodIndex)
        {
            return Foods[foodIndex];
        }

        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _positions[foodIndex];
        }
    }
}