using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Viper.Game.Elements.Food;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Food triggers the FoodAmountChanged event.
    /// </summary>
    public class FoodAmountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The new amount of food being shown.
        /// </summary>
        public int FoodElements { get; }

        /// <summary>
        /// The action that was taken when this event was raised.
        /// </summary>
        public FoodAction Action { get; }

        public FoodAmountChangedEventArgs(int amountOfElements, FoodAction action)
        {
            FoodElements = amountOfElements;
            Action = action;
        }
    }
}
