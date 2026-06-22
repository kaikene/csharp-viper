using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Food triggers the PositionChanged event.
    /// </summary>
    public class FoodPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// X axis position of the food element that was moved.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Y axis position of the food element that was moved.
        /// </summary>
        public double Y { get; }

        public FoodPositionChangedEventArgs(double toWhereX, double toWhereY)
        {
            X = toWhereX;
            Y = toWhereY;
        }
    }
}
