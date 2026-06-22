using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.Game.Builders;

namespace Viper.Game.Managers
{
    public class OverlayLayer
    {
        private Grid _overlay = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            IsHitTestVisible = true,
        };

        public Grid Displayer { get { return _overlay; } }

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        private SettingsPanel _sp = new();

        private Blackout _bo = new();

        public SettingsPanel SettingsOverlay
        {
            get
            {
                return _sp;
            }
        }

        public Blackout BlackoutOverlay
        {
            get
            {
                return _bo;
            }
        }

        public void LoadOverlays()
        {
            if (!_isInitialized)
            {
                _overlay.Children.Add(_sp.Overlay);

                _overlay.Children.Add(_bo.Overlay);

                _sp.LoadSettingsElements();

                _isInitialized = true;
            }
        }

        public void End()
        {
            if (_isInitialized)
            {
                _overlay.Children.Clear();
                _isInitialized = false;
            }
        }
    }
}
