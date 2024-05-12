using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the PlayerBodyElementsCountChanged event.
    /// </summary>
    public class PlayerBodyElementsCountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new amount of elements.
        /// </summary>
        public int BodyElements { get; }

        public PlayerBodyElementsCountChangedEventArgs(int amountOfElements)
        {
            BodyElements = amountOfElements;
        }
    }
}
