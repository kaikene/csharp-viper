using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.GameplayManager triggers the FoodAmountChanged event.
    /// </summary>
    public class GMFoodAmountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new food amount.
        /// </summary>
        public int Amount { get; }

        public GMFoodAmountChangedEventArgs(int amount)
        {
            Amount = amount;
        }
    }
}
