using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Viper.Game.Elements.Gameplay
{
    public class Playfield
    {
        private Grid _playfield = new();

        public Grid CurrentPlayfield
        {
            get { return _playfield; }
        }

        private int _size = 0;

        public const int DEFAULT_SIZE = 7;

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

        public const int SPACE_SIZES = 10;

        private Panel? _parent;

        private bool _showing = false;

        public void Show(Panel panel)
        {
            if (_showing == false)
            {
                _showing = true;
                _parent = panel;

                _playfield.Tag = "Viper.Game.Elements.Playfield";

                panel.Children.Add(_playfield);
            }
        }

        public void Remove()
        {
            if (_showing == true)
            {
                _parent.Children.Remove(_playfield);
                _playfield = new();
                _parent = null;
                _showing = false;
            }
        }

        public void Reset()
        {
            if (_showing == true)
            {
                _playfield.Height = DEFAULT_SIZE * SPACE_SIZES;
                _playfield.Width = DEFAULT_SIZE * SPACE_SIZES;
            }
        }
    }
}
