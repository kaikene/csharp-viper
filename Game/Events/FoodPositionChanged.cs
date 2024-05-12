using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Food triggers the FoodPositionChanged event.
    /// </summary>
    public class FoodPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Index of the food element that was moved.
        /// </summary>
        public int FoodIndex { get; }

        /// <summary>
        /// X axis position of the food element that was moved.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Y axis position of the food element that was moved.
        /// </summary>
        public double Y { get; }

        public FoodPositionChangedEventArgs(int foodIndexThatMoved, double toWhereX, double toWhereY)
        {
            FoodIndex = foodIndexThatMoved;
            X = toWhereX;
            Y = toWhereY;
        }
    }
}
