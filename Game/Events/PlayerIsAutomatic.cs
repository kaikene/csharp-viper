using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the IsAutomatic event.
    /// </summary>
    public class PlayerIsAutomaticEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the current amount of deaths.
        /// </summary>
        public bool IsAutomatic { get; }

        public PlayerIsAutomaticEventArgs(bool isItAutomatic)
        {
            IsAutomatic = isItAutomatic;
        }
    }
}
