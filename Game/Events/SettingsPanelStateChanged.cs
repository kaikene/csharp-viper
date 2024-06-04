using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Builders.SettingsPanel triggers the StateChanged event.
    /// </summary>
    public class SettingsPanelStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new state of setting panel.
        /// </summary>
        public bool State { get; }

        public SettingsPanelStateChangedEventArgs(bool state)
        {
            State = state;
        }
    }
}
