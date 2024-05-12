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
    /// <summary>
    /// Manages players and food elements in a nicer way, with a custom amount of playfields.
    /// </summary>
    public class GameplayManager()
    {
        /// <summary>
        /// This is the limit of playfields per row.
        /// </summary>
        public const int PLAYFIELD_LIMIT_PER_ROW = 7;

        // Amount of playfields, and a counter for the playfield row limit.
        private int _playfieldAmount = 0, _playfieldLimitPerRow = PLAYFIELD_LIMIT_PER_ROW;

        private bool isShowing = false;

        /// <summary>
        /// Gets the current amount of playfields.
        /// </summary>
        public int PlayfieldAmount { get { return _playfieldAmount; } }

        // A StackPanel that handles the rows of playfields.
        private StackPanel _playfieldManagerSP = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        // A ViewBox that works as a "zoom".
        private Viewbox _playfieldManagerVB = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        // List of horizontal rows.
        private List<StackPanel> _playfieldRows = new();

        // List of playfields.
        private List<Grid> _playfields = new();

        /// <summary>
        /// Returns the main component handling all elements of the gameplay manager, empty until "Show()" is used
        /// </summary>
        public Viewbox PlayfieldManager { get { return _playfieldManagerVB; } }

        /// <summary>
        /// Shows the elements on the PlayfieldManager viewbox
        /// </summary>
        public void Show()
        {
            if (!isShowing)
            {
                _playfieldManagerVB.Child = _playfieldManagerSP;
            }

            isShowing = true;
        }

        /// <summary>
        /// Hide the PlayfieldManager.
        /// </summary>
        public void Hide()
        {
            _playfieldManagerVB.Child = null;
            isShowing = false;
        }

        /// <summary>
        /// Add a playfield with a player included.
        /// </summary>
        public void AddPlayfield()
        {
            _playfieldAmount += 1;

            Player player = new();

            Food food = new();

            // If theres no playfields or the amount of playfields is higher than the limit availible per row, put the newly created playfield in a new row.
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
                    _playfieldLimitPerRow += PLAYFIELD_LIMIT_PER_ROW; // Increment the limit temporarly, so when the new limit is reached it puts the new playfields in a new row again.
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

            // Add player and food to the playfield.
            playfield.Children.Add(player.AddPlayer());
            playfield.Children.Add(food.AddFood());

            _playfields.Add(playfield);

            _playfieldRows[_playfieldRows.Count - 1].Children.Add(playfield);
        }

        /// <summary>
        /// Remove the last playfield.
        /// </summary>
        public void RemovePlayfield()
        {
            if (_playfieldAmount - 1 != -1) // If theres no more playfields left, then theres nothing to remove.
            {
                _playfieldAmount -= 1;

                int lastPlayfieldRow = _playfieldRows.Count - 1;
                int lastPlayfield = _playfieldRows[lastPlayfieldRow].Children.Count - 1;

                void RemoveLastPlayfield()
                {
                    _playfieldRows[_playfieldRows.Count - 1].Children.RemoveAt(lastPlayfield); // Remove playfield from panel.
                    _playfields.RemoveAt(_playfields.Count - 1); // Remove playfield from list.
                }

                void RemoveLastRow()
                {
                    if (_playfieldLimitPerRow - PLAYFIELD_LIMIT_PER_ROW != 0) // Prevent row limit counter from going to 0, otherwise new rows will only have one playfield.
                    {
                        _playfieldLimitPerRow -= PLAYFIELD_LIMIT_PER_ROW;
                    }

                    _playfieldManagerSP.Children.RemoveAt(lastPlayfieldRow); // Remove row from panel.
                    _playfieldRows.RemoveAt(lastPlayfieldRow); // Remove row from list.
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