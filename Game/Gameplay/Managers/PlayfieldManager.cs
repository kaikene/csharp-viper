using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Viper.Game.Gameplay.Managers
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

        private List<Grid> _playfields = new();

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

            TextBlock playfieldIndexTB = new()
            {
                FontSize = 23,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(120, 255, 255, 255))
            };

            _playfields.Add(playfield);

            playfieldIndexTB.Text = (_playfields.Count() - 1).ToString();

            playfield.Children.Add(playfieldIndexTB);

            return playfield;
        }

        public Grid SelectSpace(int spaceIndex)
        {
            return _playfields[spaceIndex];
        }
    }
}
