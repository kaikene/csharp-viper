using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;

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
            _foodPositions.Add(new TranslateTransform());

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
                Random random = new Random();

                int spaceH = Convert.ToInt32((_foods[0].Parent as Panel).Height);
                int spaceW = Convert.ToInt32((_foods[0].Parent as Panel).Width);

                int newX, newY;

                newX = SIZE * random.Next(0, spaceH / SIZE);
                newY = SIZE * random.Next(0, spaceW / SIZE);

                _foods[foodIndex].RenderTransform = new TranslateTransform(newX, newY);
                _foodPositions[foodIndex] = _foods[foodIndex].RenderTransform as TranslateTransform;
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
