using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace Viper.Game.Gameplay.Managers.Player
{
    public class FoodManager(int foodSize)
    {
        private int _foodCounter = -1;

        private List<Rectangle> _foods = new();

        private List<TranslateTransform> _positions = new();

        public Rectangle Add()
        {
            _positions.Add(new TranslateTransform());

            _foodCounter += 1;
            int currentIndex = _foodCounter;

            Rectangle food = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 108, 245, 66)),
                Height = foodSize,
                Width = foodSize,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            food.Loaded += (s, e) =>
            {
                RePosition(currentIndex);
            };

            _foods.Add(food);

            return food;
        }

        public void RePosition(int index)
        {
            Random random = new Random();

            int spaceH = Convert.ToInt32((_foods[index].Parent as Panel).Height);
            int spaceW = Convert.ToInt32((_foods[index].Parent as Panel).Width);

            int newX = foodSize * random.Next(0, spaceH / foodSize);
            int newY = foodSize * random.Next(0, spaceW / foodSize);

            _foods[index].RenderTransform = new TranslateTransform(newX, newY);
            _positions[index] = _foods[index].RenderTransform as TranslateTransform;
        }

        public Rectangle SelectFood(int foodIndex)
        {
            return _foods[foodIndex];
        }

        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _positions[foodIndex];
        }
    }
}
