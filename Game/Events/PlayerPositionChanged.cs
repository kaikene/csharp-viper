using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class PlayerPositionChangedEventArgs : EventArgs
    {
        public double X { get; }
        public double Y { get; }

        public PlayerPositionChangedEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
