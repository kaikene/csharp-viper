using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.GameplayManager triggers the AmountChanged event.
    /// </summary>
    public class PlayfieldAmountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the new tick rate value.
        /// </summary>
        public int NewAmount { get; }

        public PlayfieldAmountChangedEventArgs(int amount)
        {
            NewAmount = amount;
        }
    }
}
