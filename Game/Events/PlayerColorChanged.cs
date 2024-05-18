using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Viper.Game.Events
{
    /// <summary>
    /// Provides data for when Viper.Game.Elements.Player triggers the ColorChanged event.
    /// </summary>
    public class PlayerColorChangedEventArgs : EventArgs
    {
        /// <summary>
        /// New color selected.
        /// </summary>
        public Color Color { get; }

        public PlayerColorChangedEventArgs(Color color)
        {
            Color = color;
        }
    }
}
