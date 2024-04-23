using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Menu;

namespace Viper.Game
{
    public class ViperGame
    {
        private Grid viperWindow = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        public Grid Start()
        {
            Dispatcher dispatcher = System.Windows.Application.Current.Dispatcher;

            MenuScreen ms = new(viperWindow, dispatcher);

            viperWindow.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.A)
                {
                    MessageBox.Show("zzz");
                    viperWindow.Focus();
                }
            };

            viperWindow.Children.Add(ms.Show());

            return viperWindow;
        }
    }
}
