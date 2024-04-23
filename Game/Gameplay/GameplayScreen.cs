using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Viper.Game.Gameplay.Handler;

namespace Viper.Game.Gameplay
{
    public class GameplayScreen(Dispatcher dispatcher)
    {
        public Grid Show()
        {
            GameplayHandler gh = new(dispatcher);

            Grid gameplayScreen = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            };

            gameplayScreen.Children.Add(gh.ShowGameplay());

            return gameplayScreen;
        }
    }
}
