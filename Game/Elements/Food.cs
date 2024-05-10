using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;
using System.Diagnostics;

namespace Viper.Game.Elements
{
    public class Food
    {
        public event EventHandler? FoodAmountChanged;

        private enum Event
        {
            FoodAmountChanged,
        }

        public int FoodSize;

        public int CurrentPlayerXPosition, CurrentPlayerYPosition;

        public bool AreValueEventsEnabled = false;

        private List<TranslateTransform> _foodPositions = new();

        private int _foodCounter = 0;

        private List<Rectangle> _foods = new();

        public const int SIZE = 30;

        public int FoodAmount
        {
            get
            {
                return _foods.Count();
            }
        }

        public enum Axis
        {
            X,
            Y,
        }

        public Rectangle AddFood()
        {
            int currentIndex = _foodCounter;

            Rectangle food = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 108, 245, 66)),
                Height = SIZE,
                Width = SIZE,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                RenderTransform = new TranslateTransform(-30, -30),
            };

            _foods.Add(food);
            _foodPositions.Add(food.RenderTransform as TranslateTransform);

            food.Loaded += (s, e) =>
            {
                RePosition(currentIndex);
            };

            _foodCounter += 1;

            RaiseEvent(Event.FoodAmountChanged);

            return _foods[currentIndex];
        }

        public void RemoveFood()
        {
            if (_foodPositions.Count > 0)
            {
                int currentIndex = _foodCounter - 1;

                (_foods[currentIndex].Parent as Panel).Children.Remove(_foods[currentIndex]);

                _foods.RemoveAt(currentIndex);
                _foodPositions.RemoveAt(currentIndex);

                _foodCounter -= 1;

                RaiseEvent(Event.FoodAmountChanged);
            }
        }

        public void RePosition(int foodIndex)
        {
            if (_foodPositions.Count > 0)
            {
                int spaceH = Convert.ToInt32((_foods[0].Parent as Panel).Height), spaceW = Convert.ToInt32((_foods[0].Parent as Panel).Width);

                TranslateTransform newPos = new();

                if ((spaceH / SIZE) * (spaceH / SIZE) != _foods.Count)
                {
                    VerifyPositioning();

                    void VerifyPositioning()
                    {
                        Random random = new Random();

                        newPos = new(SIZE * random.Next(0, spaceH / SIZE), SIZE * random.Next(0, spaceW / SIZE));

                        for (int i = 0; i < _foodPositions.Count; i++)
                        {
                            if (newPos.X == _foodPositions[i].X && newPos.Y == _foodPositions[i].Y)
                            {
                                VerifyPositioning();
                            }
                        }
                    }
                    _foods[foodIndex].RenderTransform = new TranslateTransform(newPos.X, newPos.Y);
                    _foodPositions[foodIndex] = newPos;
                }
            }
        }   

        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _foodPositions[foodIndex];
        }

        public void CleanUp()
        {
            try
            {
                Panel parent = _foods[0].Parent as Panel;

                foreach (Rectangle food in _foods)
                {
                    parent.Children.Remove(food);
                }
            }
            catch
            {
                // Food is not loaded, nothing to remove.
            }

            _foods.Clear();
            _foodPositions.Clear();
            _foodCounter = 0;


            RaiseEvent(Event.FoodAmountChanged);
        }

        private void RaiseEvent(Event selectedEvent)
        {
            if (AreValueEventsEnabled)
            {
                if (selectedEvent == Event.FoodAmountChanged)
                {
                    FoodAmountChanged.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
