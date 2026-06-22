using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the TickRateChanged event.
    /// </summary>
    public class PlayerTickRateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the new tick rate value.
        /// </summary>
        public int TickRate { get; }

        public PlayerTickRateChangedEventArgs(int tickRate)
        {
            TickRate = tickRate;
        }
    }
}
