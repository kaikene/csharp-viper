using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Viper.Game.Animations;
using System.Windows.Media.Animation;

namespace Viper.Game.Elements.UI
{
    public class CustomSlider
    {
        public Slider NewSlider()
        {
            Slider slider = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 0, 5, 5),
                AutoToolTipPlacement = System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft,
                IsMoveToPointEnabled = true,
            };

            return slider;
        }
    }
}
