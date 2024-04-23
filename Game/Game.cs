using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.PreCompileData;
using Viper.Game.Menu;

namespace Viper.Game
{
    public class ViperGame
    {
        private Grid _viperWindow = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        private string _version = BuildData.DateTime;

        public string Version
        {
            get
            {
                return _version;
            }
        }

        public Grid Start()
        {
            Dispatcher dispatcher = System.Windows.Application.Current.Dispatcher;

            MenuScreen ms = new(_viperWindow, dispatcher);

            TextBlock versionMessage = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

#if DEBUG
            versionMessage.Text = _version + "d - Development Build";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 140, 140)); // Bright Red
#elif ALPHA
            versionMessage.Text = _version + "a - Alpha Build";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 87, 87)); // Bright Yellow
#elif BETA
            versionMessage.Text = _version + "b - Beta Build";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 255, 133)); // Bright Green
#elif RELEASE
            versionMessage.Text = _version + "s";
            versionMessage.Foreground = new SolidColorBrush(Color.FromArgb(255, 133, 208, 255)); // Bright blue
#endif


            _viperWindow.Children.Add(versionMessage);

            Panel.SetZIndex(versionMessage, 5);

            _viperWindow.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.S)
                {
                    MessageBox.Show("Settings panel stil not added D:", "Oops");
                }
            };

            _viperWindow.Children.Add(ms.Show());

            return _viperWindow;
        }
    }
}
