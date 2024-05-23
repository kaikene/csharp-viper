using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Viper.Game.Managers
{
    public class OverlayManager
    {
        private Grid _overlay = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            IsHitTestVisible = true,
        };

        public Grid Displayer { get { return _overlay; } }

        private bool _hasStarted = false;

        public void LoadOverlays()
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
            }
        }

        public void End()
        {
            if (_hasStarted)
            {
                _overlay.Children.Clear();
                _hasStarted = false;
            }
        }
    }
}
