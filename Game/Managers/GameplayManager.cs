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
        /// <summary>
        /// This is the default size of a "unit" or square on a grid, this value is used for changing the size of a playfield (30 * "Whatrever Grid Size You Want")
        /// </summary>
        public const int PLAYFIELD_GRID_SIZE = 6;

        public const int DEFAULT_PLAYFIELD_SIZE = PLAYFIELD_GRID_SIZE * 20;

        public const int UI_SIZE = 300;

        public EventHandler<PlayfieldAmountChangedEventArgs>? PlayfieldAmountChanged;

        public EventHandler<PlayerPointChangedEventArgs>? PlayerPointChanged;

        /// <summary>
        /// This is the limit of playfields per row.
        /// </summary>
        public const int PLAYFIELD_LIMIT_PER_ROW = 4;

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

        private List<Grid> _playfieldUIs = new();

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

            Grid playfield = new()
            {
                Height = DEFAULT_PLAYFIELD_SIZE,
                Width = DEFAULT_PLAYFIELD_SIZE,
                Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ClipToBounds = true,
            };

            Viewbox playfieldVB = new()
            {
                Height = UI_SIZE,
                Width = UI_SIZE,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            Grid playfieldUIHandler = new()
            {
                Height = UI_SIZE,
                Width = UI_SIZE,
                Margin = new Thickness(7, 7, 7, 7),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            TextBlock points = new()
            {
                Text = "Puntos: 0",
                Height = 17,
                Width = 70,
                Background = new SolidColorBrush(Color.FromArgb(255, 84, 221, 255)),
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, -17, 0, 0),
            };

            Panel.SetZIndex(points, -10);

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
                points.Text = "Puntos: 0";
                PlayerPointChanged?.Invoke(this, new PlayerPointChangedEventArgs(currentIndex, _points[currentIndex]));
            };

            void Player_PositionChanged(object? sender, Events.PlayerPositionChangedEventArgs e)
            {
                for (int i = 0; i < _foods[currentIndex].FoodAmount; i++)
                {
                    if (e.X == _foods[currentIndex].GetFoodPosition(i).X && e.Y == _foods[currentIndex].GetFoodPosition(i).Y)
                    {
                        CheckReposition();

                        void CheckReposition()
                        {
                            _foods[currentIndex].RePosition(i);

                            for (int x = 0; x < _players[currentIndex].PlayerBody.Count; x++)
                            {
                                // For some reason, if i dont nest these two "if's", they dont work.
                                if (_foods[currentIndex].GetFoodPosition(i).X == (_players[currentIndex].PlayerBody[x].RenderTransform as TranslateTransform).X);
                                {
                                    if ((_foods[currentIndex].GetFoodPosition(i).Y == (_players[currentIndex].PlayerBody[x].RenderTransform as TranslateTransform).Y))
                                    {
                                        CheckReposition();
                                        break;
                                    }
                                }
                            }
                        }

                        _players[currentIndex].IncreasePlayerSize();
                        _points[currentIndex]++;
                        points.Text = $"Puntos: {_points[currentIndex]}";
                        PlayerPointChanged?.Invoke(this, new PlayerPointChangedEventArgs(currentIndex, _points[currentIndex]));
                    }
                }
            }

            // Add player and food to the playfield.
            playfield.Children.Add(player.ShowPlayer());
            playfield.Children.Add(food.AddFood());

            _playfields.Add(playfield);
            _playfieldUIs.Add(playfieldUIHandler);

            playfieldVB.Child = playfield;
            playfieldUIHandler.Children.Add(playfieldVB);
            playfieldUIHandler.Children.Add(points);

            _playfieldRows[_playfieldRows.Count - 1].Children.Add(playfieldUIHandler);

            PlayfieldAmountChanged?.Invoke(this, new PlayfieldAmountChangedEventArgs(_playfields.Count));
        }

        public void ChangePlayfieldSize(int playfieldIndex, int gridXYSize)
        {
            // Size of playfield.
            _playfields[playfieldIndex].Height = PLAYFIELD_GRID_SIZE * gridXYSize;
            _playfields[playfieldIndex].Width = PLAYFIELD_GRID_SIZE * gridXYSize;

            // Size of UI handler.
            _playfieldUIs[playfieldIndex].Height = PLAYFIELD_GRID_SIZE * gridXYSize;
            _playfieldUIs[playfieldIndex].Width = PLAYFIELD_GRID_SIZE * gridXYSize;
        }

        public double GetPlayfieldXYSize(int playfieldIndex)
        {
            return _playfields[playfieldIndex].Height; // Its simetrical, so we can just give one value.
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
                    _playfieldUIs.RemoveAt(_playfieldUIs.Count - 1);
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
            _playfieldUIs.Clear();
            _playfieldRows.Clear();
            _playfieldManagerSP.Children.Clear();
            _playfieldManagerVB.Child = null;
            _playfieldLimitPerRow = PLAYFIELD_LIMIT_PER_ROW;
        }
    }
}