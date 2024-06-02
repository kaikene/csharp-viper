using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.GameplayManager triggers the PlayfieldSizeChanged event.
    /// </summary>
    public class GMPlayfieldSizeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new size.
        /// </summary>
        public int Size { get; }

        public GMPlayfieldSizeChangedEventArgs(int newSize)
        {
            Size = newSize;
        }
    }
}
