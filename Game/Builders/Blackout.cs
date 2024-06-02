using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Viper.Game.Animations;

namespace Viper.Game.Builders
{
    public class Blackout()
    {
        private Animate _animate = new();

        private Grid _blackout = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
            IsHitTestVisible = false,
            Opacity = 0,
        };

        public Grid Overlay
        {
            get
            {
                return _blackout;
            }
        }

        private List<int> _blackouts = new List<int>();

        public void ShowBlackout(int zIndex)
        {
            _blackouts.Add(zIndex);

            _blackout.IsHitTestVisible = true;

            Panel.SetZIndex(_blackout, zIndex);

            _animate.Opacity(_blackout, 1, new ExponentialEase() { EasingMode = EasingMode.EaseOut }, 150, 0);
        }

        public void RemoveBlackout()
        {
            _blackouts.RemoveAt(_blackouts.Count - 1);

            if (_blackouts.Count == 0)
            {
                _blackout.IsHitTestVisible = false;
                _animate.Opacity(_blackout, 0, new ExponentialEase() { EasingMode = EasingMode.EaseOut }, 150, 0);
            }
            else
            {
                Panel.SetZIndex(_blackout, _blackouts[_blackouts.Count - 1]);
            }
        }
    }
}
