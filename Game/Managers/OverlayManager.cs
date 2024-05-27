using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Viper.Game.Elements.UI;

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

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        private SettingsOverlay _so = new();

        public SettingsOverlay SettingsOverlay
        {
            get
            {
                return _so;
            }
        }

        public void LoadOverlays()
        {
            if (!_isInitialized)
            {
                _overlay.Children.Add(_so.Displayer);

                _so.LoadSettingsElements();

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
