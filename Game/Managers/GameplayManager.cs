using System.Diagnostics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Viper.Game.Elements;

namespace Viper.Game.Managers
{
    public class GameplayManager()
    {
        public const int PLAYFIELD_LIMIT_PER_ROW = 7;

        private int _playfieldAmount = 0, _playfieldLimitPerRow = PLAYFIELD_LIMIT_PER_ROW;

        private bool isShowing = false;

        public int PlayfieldAmount { get { return _playfieldAmount; } }

        private StackPanel _playfieldManagerSP = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        private Viewbox _playfieldManagerVB = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        private List<StackPanel> _playfieldRows = new();

        private List<Grid> _playfields = new();

        public Viewbox PlayfieldManager { get { return _playfieldManagerVB; } }

        public void Show()
        {
            if (!isShowing)
            {
                _playfieldManagerVB.Child = _playfieldManagerSP;
            }

            isShowing = true;
        }

        public void Hide()
        {
            _playfieldManagerVB.Child = null;
            isShowing = false;
        }

        public void AddPlayfield()
        {
            _playfieldAmount += 1;

            Player player = new();

            Food food = new();

            if (_playfieldAmount == 1 || _playfieldAmount > _playfieldLimitPerRow)
            {
                StackPanel playfieldRow = new()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Orientation = Orientation.Horizontal,
                };

                _playfieldRows.Add(playfieldRow);
                _playfieldManagerSP.Children.Add(_playfieldRows[_playfieldRows.Count - 1]);

                if (_playfieldAmount != 1)
                {
                    _playfieldLimitPerRow += PLAYFIELD_LIMIT_PER_ROW;
                }
            }

            Grid playfield = new()
            {
                Height = 360,
                Width = 360,
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                Margin = new Thickness(10, 10, 10, 10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            playfield.Children.Add(player.AddPlayer());
            playfield.Children.Add(food.AddFood());

            _playfields.Add(playfield);

            _playfieldRows[_playfieldRows.Count - 1].Children.Add(playfield);
        }

        public void RemovePlayfield()
        {
            if (_playfieldAmount - 1 != -1) // If theres no more playfields left, thgen theres nothing to remove.
            {
                _playfieldAmount -= 1;

                int lastPlayfieldRow = _playfieldRows.Count - 1;
                int lastPlayfield = _playfieldRows[lastPlayfieldRow].Children.Count - 1;

                void RemoveLastPlayfield()
                {
                    _playfieldRows[_playfieldRows.Count - 1].Children.RemoveAt(lastPlayfield);
                }

                void RemoveLastRow()
                {
                    if (_playfieldLimitPerRow - PLAYFIELD_LIMIT_PER_ROW != 0) // Prevent row limit counter from going to 0, otherwise new rows will only have one playfield.
                    {
                        _playfieldLimitPerRow -= PLAYFIELD_LIMIT_PER_ROW;
                    }

                    _playfieldManagerSP.Children.RemoveAt(lastPlayfieldRow);
                    _playfieldRows.RemoveAt(lastPlayfieldRow);
                }

                if (lastPlayfield - 1 == -1) // If removing the last playfield leaves you with no playfields left on the the row, then remove the entire row.
                {
                    RemoveLastPlayfield();
                    RemoveLastRow();
                }
                else
                {
                    RemoveLastPlayfield();
                }
            }
        }

        public void CleanUp()
        {
            _playfields.Clear();
            _playfieldRows.Clear();
            _playfieldManagerSP.Children.Clear();
            _playfieldManagerVB.Child = null;
            _playfieldLimitPerRow = PLAYFIELD_LIMIT_PER_ROW;
        }
    }
}