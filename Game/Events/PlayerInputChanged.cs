using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the InputChanged event.
    /// </summary>
    public class PlayerInputChangedEventArgs : EventArgs
    {
        public enum Input
        {
            Up, Down, Left, Right,
        }

        /// <summary>
        /// New input selected.
        /// </summary>
        public Key NewInput { get; }

        public Input InputToChange { get; }

        public PlayerInputChangedEventArgs(Input input, Key key)
        {
            InputToChange = input;
            NewInput = key;
        }
    }
}
