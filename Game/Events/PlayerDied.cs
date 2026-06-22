using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the Died event.
    /// </summary>
    public class PlayerDiedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the current amount of deaths.
        /// </summary>
        public int DeathCounter { get; }

        public PlayerDiedEventArgs(int deathCounter)
        {
            DeathCounter = deathCounter;
        }
    }
}
