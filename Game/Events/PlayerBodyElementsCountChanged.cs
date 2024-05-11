using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viper.Game.Events
{
    public class PlayerBodyElementsCountChangedEventArgs : EventArgs
    {
        public int BodyElements { get; }

        public PlayerBodyElementsCountChangedEventArgs(int amountOfElements)
        {
            BodyElements = amountOfElements;
        }
    }
}
