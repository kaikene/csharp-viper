using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.GameplayManager triggers the PointChanged event.
    /// </summary>
    public class GMPointChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new points value.
        /// </summary>
        public int Points { get; }

        public GMPointChangedEventArgs(int amount)
        {
            Points = amount;
        }
    }
}
