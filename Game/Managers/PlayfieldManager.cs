using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Viper.Viper.Game.Managers
{
    public class PlayfieldManager
    {
        private double _size;

        public double Size
        {
            get
            {
                return _size;
            }
        }

        private Grid CurrentPlayfield;

        public Grid Add(double size)
        {
            _size = size;

            Grid playfield = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Height = size,
                Width = size,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            CurrentPlayfield = playfield;

            return playfield;
        }

        public void ChangeSize(int newSize)
        {
            CurrentPlayfield.Height = newSize;
            CurrentPlayfield.Width = newSize;
        }
    }
}
