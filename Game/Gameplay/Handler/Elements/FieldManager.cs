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
        // private List<Grid> _fields = new();

        public Grid Add()
        {
            Grid field = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Height = 220,
                Width = 220,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // _fields.Add(field);

            return field;
        }

        //public Grid SelectField(int fieldIndex)
        //{
        //    return _fields[fieldIndex];
        //}
    }
}
