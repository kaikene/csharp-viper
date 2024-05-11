using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;
using System.Diagnostics;
using Viper.Game.Events;

namespace Viper.Game.Elements
{
    public class Food
    {
        public event EventHandler<FoodColorChangedEventArgs> ColorChanged;

        public event EventHandler<FoodAmountChangedEventArgs> FoodAmountChanged;

        public event EventHandler<FoodPositionChangedEventArgs> FoodPositionsChanged;

        public enum FoodAction
        {
            Remove,
            Add,
            Check,
        }

        private Color _color = Color.FromArgb(255, 109, 255, 56);

        public Color CurrentColor
        {
            get
            {
                return _color;
            }
        }

        private enum Event
        {
            FoodAmountChanged,
        }

        public int FoodSize;

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

        public Rectangle AddFood()
        {
            int currentIndex = _foodCounter;

            Rectangle food = new()
            {
                Fill = new SolidColorBrush(_color),
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
                int spaceW = Convert.ToInt32((_foods[0].Parent as Panel).Height), spaceH = Convert.ToInt32((_foods[0].Parent as Panel).Width);

                if ((spaceW / SIZE) * (spaceH / SIZE) != _foods.Count)
                {
                    _foodCounter += 1;

                    FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Add));

                    RePosition(currentIndex);
                }
                else
                {
                    _foods.Remove(food);
                    _foodPositions.Remove(food.RenderTransform as TranslateTransform);
                }
            };

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

                FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Remove));
            }
        }

        public void RePosition(int foodIndex)
        {
            if (_foodPositions.Count > 0)
            {
                int spaceH = Convert.ToInt32((_foods[0].Parent as Panel).Height), spaceW = Convert.ToInt32((_foods[0].Parent as Panel).Width);

                TranslateTransform newPos = new();

                VerifyPositioning();

                void VerifyPositioning()
                {
                    Random random = new Random();

                    newPos = new(SIZE * random.Next(0, spaceW / SIZE), SIZE * random.Next(0, spaceH / SIZE));

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

                FoodPositionsChanged?.Invoke(this, new FoodPositionChangedEventArgs(foodIndex, newPos.X, newPos.Y));
            }
        }   

        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _foodPositions[foodIndex];
        }

        public void ChangeFoodColor(Color newColor)
        {
            _color = newColor;

            foreach (var food in _foods)
            {
                food.Fill = new SolidColorBrush(_color);
            }
            ColorChanged?.Invoke(this, new FoodColorChangedEventArgs(_color));
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

            FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Check));
        }

        private void RaiseAllEvents(Event selectedEvent)
        {
            if (AreValueEventsEnabled)
            {
                FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Check));
                ColorChanged?.Invoke(this, new FoodColorChangedEventArgs(_color));
                FoodPositionsChanged?.Invoke(this, new FoodPositionChangedEventArgs(0, 0, 0));
            }
        }
    }
}
