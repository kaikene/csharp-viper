using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class PlayerMovingChangedEventArgs : EventArgs
    {
        public bool IsPlayerMoving { get; }

        public PlayerMovingChangedEventArgs(bool isPlayerMoving)
        {
            IsPlayerMoving = isPlayerMoving;
        }
    }
}
