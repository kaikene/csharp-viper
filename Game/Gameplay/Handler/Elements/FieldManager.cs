using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Viper.Game.Gameplay.Handler.Elements
{
    public class FieldManager
    {
        private List<Grid> _fields = new();

        public Grid Add()
        {
            Grid field = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Height = 210,
                Width = 210,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBlock fieldIndexTB = new()
            {
                FontSize = 23,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment= HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(120,255,255,255))
            };

            _fields.Add(field);

            fieldIndexTB.Text = (_fields.Count() - 1).ToString();

            field.Children.Add(fieldIndexTB);

            return field;
        }

        public Grid SelectField(int fieldIndex)
        {
            return _fields[fieldIndex];
        }
    }
}
