using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the PlayerMovingChanged event.
    /// </summary>
    public class PlayerMovingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns info about if the player moving now.
        /// </summary>
        public bool IsPlayerMoving { get; }

        public PlayerMovingChangedEventArgs(bool isPlayerMoving)
        {
            IsPlayerMoving = isPlayerMoving;
        }
    }
}
