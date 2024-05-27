using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Viper.Game.Animations;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Game.Elements.UI
{
    public class SettingsOverlay
    {
        private ScrollViewer _settingsSV = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
        };

        private StackPanel _settingsSP = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
        };

        private Grid _settingsGrid = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
            Width = 300,
            Opacity = 0,
            RenderTransform = new TranslateTransform(-300, 0),
        };

        private Rectangle _coolLine = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Width = 2,
            Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            Opacity = 0,
        };

        private bool _switch = true;

        public Grid Displayer
        {
            get
            {
                return _settingsGrid;
            }
        }

        public void LoadSettingsElements()
        {
            _settingsGrid.Children.Add(_settingsSV);

            _settingsGrid.Children.Add(_coolLine);

            _settingsSV.Content = _settingsSP;
        }

        public void End()
        {
            _settingsSV.Content = null;
            _settingsSP.Children.Clear();
            _settingsGrid.Children.Clear();
        }

        public void SettingsShowHide()
        {
            if (_switch)
            {
                ShowSettings();
            }
            else
            {
                HideSettings();
            }

            _switch = !_switch;
        }

        private void ShowSettings()
        {
            Animate.Position(_settingsGrid, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut}, 300, 0);
            Animate.Opacity(_settingsGrid, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            Animate.Opacity(_coolLine, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 700, 0, 1);
            _settingsSP.IsHitTestVisible = true;
        }

        private void HideSettings()
        {
            Animate.Position(_settingsGrid, new TranslateTransform(-300, 0), new ExponentialEase() { EasingMode = EasingMode.EaseIn }, 300, 0);
            Animate.Opacity(_settingsGrid, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 500, 0);
            _settingsSP.IsHitTestVisible = false;
        }
    }
}
