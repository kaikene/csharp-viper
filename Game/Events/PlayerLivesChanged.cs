using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the LivesChanged event.
    /// </summary>
    public class PlayerLivesChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the new amount of lives
        /// </summary>
        public int CurrentLives { get; }

        public PlayerLivesChangedEventArgs(int livesLeft)
        {
            CurrentLives = livesLeft;
        }
    }
}
