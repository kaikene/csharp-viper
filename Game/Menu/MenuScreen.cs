using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Gameplay;

namespace Viper.Game.Menu
{
    public class MenuScreen(Grid viperWindowInstance, Dispatcher dispatcher)
    {
        public Grid Show()
        {
            Grid menuScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Focusable = true,
            };

            Button startButton = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Height = 40,
                Width = 60,
                Content = "Jugar"
            };

            menuScreen.Children.Add(startButton);

            startButton.PreviewMouseDown += StartButton_PreviewMouseDown;

            void StartButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                GameplayScreen gs = new(dispatcher);

                viperWindowInstance.Children.Clear();
                viperWindowInstance.Children.Add(gs.Show());
            }

            return menuScreen;
        }
    }
}
