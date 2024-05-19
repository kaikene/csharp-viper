using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Viper.Game.Elements.Gameplay
{
    public class Playfield
    {
        private int _size = 0;

        public const int DEFAULT_SIZE = 7;

        public const int SPACE_SIZES = 10;

        public int Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;

                _playfield.Height = _size * SPACE_SIZES;
                _playfield.Width = _size * SPACE_SIZES;
            }
        }

        private Grid _playfield = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
        };

        public Grid CurrentPlayfield
        {
            get
            {
                if (_size == 0)
                {
                    Size = DEFAULT_SIZE;
                }

                return _playfield;
            }
        }
    }
}
