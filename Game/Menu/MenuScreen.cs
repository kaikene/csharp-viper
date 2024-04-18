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
    public class MenuScreen
    {
        public Grid Show(Grid windowGridInstance, Dispatcher dispatcher)
        {
            Grid menuScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            };

            TextBlock description = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Colors.White),
                Text = "Haz click o algo"
            };

            menuScreen.Children.Add(description);

            menuScreen.PreviewMouseDown += MenuScreen_PreviewMouseDown;

            void MenuScreen_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                menuScreen.IsHitTestVisible = false;
                menuScreen.Opacity = 0;

                GameplayScreen gs = new();

                windowGridInstance.Children.Add(gs.Show(dispatcher));
            }

            return menuScreen;
        }
    }
}
