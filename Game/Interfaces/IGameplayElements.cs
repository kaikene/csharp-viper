using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Viper.Game.Interfaces
{
    public interface IGameplayElements
    {
        /// <summary>
        /// Returns the gameplay element specified, cannot be added more than one time.
        /// </summary>
        /// <returns></returns>
        void Show(Panel panel);

        /// <summary>
        /// Removes the element added.
        /// </summary>
        void Remove();

        /// <summary>
        /// Resets the state of the element.
        /// </summary>
        void Reset();
    }
}
