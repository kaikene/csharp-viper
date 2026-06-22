using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.ScreenSwitcher triggers the HistoryChanged event.
    /// </summary>
    public class SSScreenHistoryChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the amount of screens currently saved.
        /// </summary>
        public int ScreensSaved { get; }

        public SSScreenHistoryChangedEventArgs(int amount)
        {
            ScreensSaved = amount;
        }
    }
}
