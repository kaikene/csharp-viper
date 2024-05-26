using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.ScreenManager triggers the HistoryChanged event.
    /// </summary>
    public class SMScreenHistoryChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the new tick rate value.
        /// </summary>
        public int ScreensSaved { get; }

        public SMScreenHistoryChangedEventArgs(int amount)
        {
            ScreensSaved = amount;
        }
    }
}
