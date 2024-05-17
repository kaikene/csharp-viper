using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Managers.GameplayManager triggers the PlayerPointChanged event.
    /// </summary>
    public class PlayerPointChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Return the new tick rate value.
        /// </summary>
        public int NewPoints { get; }

        public int PlayerIndex { get; }

        public bool IsBeingRemoved { get; }

        public PlayerPointChangedEventArgs(int playerIndex, int amount, bool isBeingRemoved = false)
        {
            NewPoints = amount;
            PlayerIndex = playerIndex;
            IsBeingRemoved = isBeingRemoved;
        }
    }
}
