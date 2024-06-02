using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Viper.Game.Managers;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.ScreenSwitcher triggers the ScreenChanged event.
    /// </summary>
    public class SSScreenChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the current screen being shown in the ScreenSwitcher
        /// </summary>
        public ScreenSwitcher.Screens CurrentScreen { get; }

        public SSScreenChangedEventArgs(ScreenSwitcher.Screens screen)
        {
            CurrentScreen = screen;
        }
    }
}
