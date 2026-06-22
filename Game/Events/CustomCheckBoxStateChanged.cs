using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.CustomCheckBox triggers the StateChanged event.
    /// </summary>
    public class CustomCheckBoxStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new state of the check box.
        /// </summary>
        public bool State { get; }

        public CustomCheckBoxStateChangedEventArgs(bool state)
        {
            State = state;
        }
    }
}
