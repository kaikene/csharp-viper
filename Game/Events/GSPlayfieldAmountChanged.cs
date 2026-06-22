using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Screens.GameplayScreen triggers the PlayfieldAmountChanged event.
    /// </summary>
    public class GSPlayfieldAmountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new amount of playfields
        /// </summary>
        public int Amount { get; }

        public GSPlayfieldAmountChangedEventArgs(int amount)
        {
            Amount = amount;
        }
    }
}
