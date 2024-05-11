using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Viper.Game.Elements.Food;

namespace Viper.Game.Events
{
    public class FoodAmountChangedEventArgs : EventArgs
    {
        public int FoodElements { get; }

        public FoodAction Action { get; }

        public FoodAmountChangedEventArgs(int amountOfElements, FoodAction action)
        {
            FoodElements = amountOfElements;
            Action = action;
        }
    }
}
