using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.UnlimitedSelector triggers the IndexChanged event.
    /// </summary>
    public class UnlimitedSelectorIndexChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new inedx of the element selected
        /// </summary>
        public int Index { get; }

        public UnlimitedSelectorIndexChangedEventArgs(int newIndex)
        {
            Index = newIndex;
        }
    }
}
