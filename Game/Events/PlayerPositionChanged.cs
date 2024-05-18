using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the PositionChanged event.
    /// </summary>
    public class PlayerPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new X position.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Returns the new X position.
        /// </summary>
        public double Y { get; }

        public PlayerPositionChangedEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
