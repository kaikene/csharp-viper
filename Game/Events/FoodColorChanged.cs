using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Food triggers the ColorChanged event.
    /// </summary>
    public class FoodColorChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new color selected.
        /// </summary>
        public Color Color { get; }

        public FoodColorChangedEventArgs(Color color)
        {
            Color = color;
        }
    }
}
