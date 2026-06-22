using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.CustomCombokBox triggers the SelectionChanged event.
    /// </summary>
    public class CustomComboBoxSelectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new selection.
        /// </summary>
        public string Selection { get; }

        /// <summary>
        /// Returns the index of the element selected.
        /// </summary>
        public int SelectionIndex { get; }

        public CustomComboBoxSelectionChangedEventArgs(string selection, int selectionIndex)
        {
            Selection = selection;
            SelectionIndex = selectionIndex;
        }
    }
}
