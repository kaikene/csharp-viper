using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Screens.GameplayScreen triggers the ScaleChanged event.
    /// </summary>
    public class GSScaleChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the new tick rate value.
        /// </summary>
        public double Scale { get; }

        public GSScaleChangedEventArgs(double amount)
        {
            Scale = amount;
        }
    }
}
