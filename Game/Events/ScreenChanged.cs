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
    /// Provides data for when Viper.Game.Managers.ScreenManager triggers the ScreenChanged event.
    /// </summary>
    public class ScreenChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns the new color selected.
        /// </summary>
        public ScreenManager.Screens CurrentScreen { get; }

        public ScreenChangedEventArgs(ScreenManager.Screens screen)
        {
            CurrentScreen = screen;
        }
    }
}
