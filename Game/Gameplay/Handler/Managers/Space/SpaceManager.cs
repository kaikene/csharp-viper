using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Viper.Game.Gameplay.Handler.Managers.Space
{
    public class SpaceManager
    {
        private List<Grid> _spaces = new();

        public Grid Add(double size)
        {
            Grid space = new()
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Height = size,
                Width = size,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBlock spaceIndexTB = new()
            {
                FontSize = 23,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(120, 255, 255, 255))
            };

            _spaces.Add(space);

            spaceIndexTB.Text = (_spaces.Count() - 1).ToString();

            space.Children.Add(spaceIndexTB);

            return space;
        }

        public Grid SelectSpace(int spaceIndex)
        {
            return _spaces[spaceIndex];
        }
    }
}
