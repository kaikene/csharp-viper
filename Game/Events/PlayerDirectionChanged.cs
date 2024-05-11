using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Viper.Game.Elements.Player;

namespace Viper.Game.Events
{
    public class PlayerDirectionChangedEventArgs : EventArgs
    {
        public Direction Direction { get; }

        public PlayerDirectionChangedEventArgs(Direction playerDirection)
        {
            Direction = playerDirection;
        }
    }
}
