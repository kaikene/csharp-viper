using System.Diagnostics;
using System.Numerics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Viper.Game.Elements;
using Viper.Game.Events;

namespace Viper.Game.Managers
{
    /// <summary>
    /// Manages players and food elements in a nicer way, with a custom amount of playfields.
    /// </summary>
    public class GameplayManager()
    {
        public EventHandler<PlayfieldAmountChangedEventArgs>? PlayfieldAmountChanged;

        public EventHandler<PlayerPointChangedEventArgs>? PlayerPointChanged;

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

        private List<Player> _players = new();

        private List<Food> _foods = new();

        private List<int> _points = new();

        public List<Player> Players
        {
            get
            {
                return _players;
            }
        }

        public List<Food> Foods
        {
            get
            {
                return _foods;
            }
        }

        /// <summary>
        /// Add a playfield with a player included.
        /// </summary>
        public void AddPlayfield()
        {
            _playfieldAmount += 1;

            Player player = new();

            Food food = new();

            _players.Add(player);

            _foods.Add(food);

            _points.Add(0);

            int currentIndex = _foods.Count - 1;

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

            player.PositionChanged += Player_PositionChanged;

            player.PlayerDied += (s, e) =>
            {
                _points[currentIndex] = 0;
                PlayerPointChanged?.Invoke(this, new PlayerPointChangedEventArgs(currentIndex, _points[currentIndex]));
            };

            void Player_PositionChanged(object? sender, Events.PlayerPositionChangedEventArgs e)
            {
                for (int i = 0; i < _foods[currentIndex].FoodAmount; i++)
                {
                    if (e.X == _foods[currentIndex].GetFoodPosition(i).X && e.Y == _foods[currentIndex].GetFoodPosition(i).Y)
                    {
                        _foods[currentIndex].RePosition(i);
                        _players[currentIndex].IncreasePlayerSize();
                        _points[currentIndex]++;
                        PlayerPointChanged?.Invoke(this, new PlayerPointChangedEventArgs(currentIndex, _points[currentIndex]));
                    }
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
                ClipToBounds = true,
            };

            // Add player and food to the playfield.
            playfield.Children.Add(player.ShowPlayer());
            playfield.Children.Add(food.AddFood());

            _playfields.Add(playfield);

            _playfieldRows[_playfieldRows.Count - 1].Children.Add(playfield);

            PlayfieldAmountChanged?.Invoke(this, new PlayfieldAmountChangedEventArgs(_playfields.Count));
        }

        /// <summary>
        /// Remove the last playfield.
        /// </summary>
        public void RemovePlayfield()
        {
            if (_playfieldAmount - 1 != -1) // If theres no more playfields left, then theres nothing to remove.
            {
                _players[_players.Count - 1].CleanUp();
                _foods[_players.Count - 1].CleanUp();

                _players.RemoveAt(_players.Count - 1);
                _foods.RemoveAt(_foods.Count - 1);

                PlayerPointChanged?.Invoke(this, new PlayerPointChangedEventArgs(_points.Count - 1, 0, true));
                _points.RemoveAt(_points.Count - 1);

                _playfieldAmount -= 1;

                int lastPlayfieldRow = _playfieldRows.Count - 1;
                int lastPlayfield = _playfieldRows[lastPlayfieldRow].Children.Count - 1;

                void RemoveLastPlayfield()
                {
                    _playfieldRows[_playfieldRows.Count - 1].Children.RemoveAt(lastPlayfield); // Remove playfield from panel.
                    _playfields[_playfields.Count - 1].Children.Clear(); // Remove elements from playfield
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

                PlayfieldAmountChanged?.Invoke(this, new PlayfieldAmountChangedEventArgs(_playfields.Count));
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