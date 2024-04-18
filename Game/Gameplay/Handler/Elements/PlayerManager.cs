using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Viper.Game.Gameplay.Handler.Elements
{
    public class PlayerManager
    {
        private List<Rectangle> _players = new();

        public Rectangle Add(Dispatcher dispatcher)
        {
            Rectangle player = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 30,
                Width = 30,
            };

            player.PreviewKeyDown += Player_KeyDown;

            void Player_KeyDown(object sender, KeyEventArgs e)
            {
                Debug.WriteLine("Key pressed");
            }

            _players.Add(player);

            return player;
        }

        public Rectangle SelectPlayer(int playerIndex)
        {
            return _players[playerIndex];
        }
    }
}
