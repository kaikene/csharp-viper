using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.CustomComboBox triggers the ElementAmountChanged event.
    /// </summary>
    public class CustomComboBoxElementAmountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new amount of elements.
        /// </summary>
        public int Amount { get; }

        public CustomComboBoxElementAmountChangedEventArgs(int amount)
        {
            Amount = amount;
        }
    }
}
