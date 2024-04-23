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
    public class PlayerManager(Dispatcher dispatcher)
    {
        private List<Rectangle> _players = new();

        public Rectangle Add()
        {
            Rectangle player = new()
            {
                Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 30,
                Width = 30,
                RenderTransform = new TranslateTransform(0, 0),
        };

            player.PreviewKeyDown += Player_KeyDown; // Event is added

            void Player_KeyDown(object sender, KeyEventArgs e) // But this does nothing for some reason.
            {
                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)
                {
                    TranslateTransform currentPos = player.RenderTransform as TranslateTransform;

                    if (e.Key == Key.Up)
                    {
                        player.RenderTransform = new TranslateTransform(currentPos.X, currentPos.Y - 30);
                    }
                    else if (e.Key == Key.Down)
                    {
                        player.RenderTransform = new TranslateTransform(currentPos.X, currentPos.Y + 30);
                    }
                    else if (e.Key == Key.Left)
                    {
                        player.RenderTransform = new TranslateTransform(currentPos.X - 30, currentPos.Y);
                    }
                    else if (e.Key == Key.Right)
                    {
                        player.RenderTransform = new TranslateTransform(currentPos.X + 30, currentPos.Y);
                    }
                    else
                    {
                        player.RenderTransform = new TranslateTransform(0, 0);
                    }
                }
            }

            _players.Add(player);

            return player;
        }

        public Rectangle Player(int playerIndex)
        {
            return _players[playerIndex];
        }
    }
}
