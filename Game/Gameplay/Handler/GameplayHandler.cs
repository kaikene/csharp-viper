using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using Viper.Game.Gameplay.Handler.Elements;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Game.Gameplay.Handler
{
    public class GameplayHandler
    {
        public Viewbox ShowGameplay(Dispatcher dispatcher)
        {
            FieldManager fm = new();

            PlayerManager pm = new();

            Viewbox gameplayViewbox = new()
            {
                Height = 200,
                Width = 200,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            Grid currentField = fm.Add();
            Rectangle currentPlayer = pm.Add(dispatcher);

            currentField.Children.Add(currentPlayer);

            gameplayViewbox.Child = currentField;

            return gameplayViewbox;
        }
    }
}
