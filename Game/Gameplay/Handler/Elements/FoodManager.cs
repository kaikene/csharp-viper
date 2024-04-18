using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace Viper.Game.Gameplay.Handler.Elements
{
    internal class FoodManager
    {
        private List<Rectangle> _foods = new();

        public Rectangle Add()
        {
            Rectangle food = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 255, 50, 50)),
                Height = 40,
                Width = 40,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            _foods.Add(food);

            return food;
        }

        public Rectangle SelectField(int foodIndex)
        {
            return _foods[foodIndex];
        }
    }
}
