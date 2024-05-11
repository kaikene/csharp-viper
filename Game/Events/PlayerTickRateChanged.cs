using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class PlayerTickRateChangedEventArgs : EventArgs
    {
        public int TickRate { get; }

        public PlayerTickRateChangedEventArgs(int tickRate)
        {
            TickRate = tickRate;
        }
    }
}
