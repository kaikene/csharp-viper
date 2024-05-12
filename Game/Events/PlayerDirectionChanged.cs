using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Viper.Game.Elements.Player;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the PlayerDirectionChanged event.
    /// </summary>
    public class PlayerDirectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// New direction of the player.
        /// </summary>
        public Direction Direction { get; }

        public PlayerDirectionChangedEventArgs(Direction playerDirection)
        {
            Direction = playerDirection;
        }
    }
}
