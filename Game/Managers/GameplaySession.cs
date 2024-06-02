using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Viper.Game.Elements.Gameplay;
using Viper.Game.Events;
using Color = System.Windows.Media.Color;

namespace Viper.Game.Managers
{
    /// <summary>
    /// Manages a playfield with a player and custom amounts of food.
    /// </summary>
    public class GameplaySession
    {
        public const int DEFAULT_DISPLAYER_SIZE = 200, DEFAULT_PLAYFIELD_SIZE = 10;

        private double? _pfSize = null;

        public double? PlayfieldSize
        {
            get
            {
                return _pfSize;
            }
        }

        private int _points = 0;

        public EventHandler<GMPointChangedEventArgs>? PointsChanged;

        public EventHandler<GMFoodAmountChangedEventArgs>? FoodAmountChanged;

        public EventHandler<GMPlayfieldSizeChangedEventArgs>? PlayfieldSizeChanged;

        public int Points { get { return _points; } }

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        private Player _player = new();

        private List<Food> _foods = new();

        public Player Player
        {
            get
            {
                return _player;
            }
        }

        public List<Food> Food
        {
            get
            {
                return _foods;
            }
        }

        private Grid _playfield = new()
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Background = new SolidColorBrush(Color.FromArgb(120,0,0,0)),
        };

        public Grid Displayer
        {
            get
            {
                return _playfield;
            }
        }

        public void LoadElements()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                _player.PositionChanged += _player_PositionChanged;

                if (_pfSize == null)
                {
                    _pfSize = DEFAULT_PLAYFIELD_SIZE;
                    _playfield.Height = (double)(_pfSize * Player.SIZE);
                    _playfield.Width = (double)(_pfSize * Player.SIZE);
                }

                _player.Died += _player_Died;

                _player.Show(_playfield);
            }            
        }

        private void _player_Died(object? sender, PlayerDiedEventArgs e)
        {
            foreach (Food food in _foods)
            {
                food.Reset();
            }
        }

        private void _player_PositionChanged(object? sender, PlayerPositionChangedEventArgs e)
        {
            for (int i = 0; i < _foods.Count; i++)
            {
                if (_foods[i].Position.X == _player.Position.X && _foods[i].Position.Y == _player.Position.Y)
                {
                    // If the lenght of the player and the amount of food is equal or higher than the space availible
                    // then that means theres no more space to reposition the food, so we move it outside the view.
                    if (_player.BodyElements.Count + _foods.Count >= _pfSize * _pfSize)
                    {
                        _foods[i].ResetOutOfBounds();
                    }
                    else
                    {
                        CheckFoodOverposition();

                        // Check if the new position is equal to the position of another food element or a player element.
                        void CheckFoodOverposition()
                        {
                            _foods[i].Reset();

                            for (int z = 0; z < _foods.Count; z++)
                            {
                                if (i != z)
                                {
                                    if (_foods[i].Position.X == _foods[z].Position.X && _foods[i].Position.Y == _foods[z].Position.Y)
                                    {
                                        CheckFoodOverposition();
                                        break;
                                    }
                                }
                            }

                            for (int x = 0; x < _player.BodyElements.Count; x++)
                            {
                                if (_foods[i].Position.X == (_player.BodyElements[x].RenderTransform as TranslateTransform).X && _foods[i].Position.Y == (_player.BodyElements[x].RenderTransform as TranslateTransform).Y)
                                {
                                    CheckFoodOverposition();
                                    break;
                                }
                            }
                        }
                    }

                    _player.IncreasePlayerSize();
                    _points++;
                    PointsChanged?.Invoke(this, new GMPointChangedEventArgs(_points));

                    break;
                }
            }
        }

        public void End()
        {
            _isInitialized = false;
            _points = 0;
            PointsChanged?.Invoke(this, new GMPointChangedEventArgs(_points));
            _player.Remove();

            foreach (Food food in _foods)
            {
                food.Remove();
            }

            _playfield.Children.Clear();
            _foods.Clear();
        }

        public void AddFood()
        {
            if (_isInitialized)
            {
                Food food = new();

                food.Show(_playfield);

                _foods.Add(food);

                FoodAmountChanged?.Invoke(this, new GMFoodAmountChangedEventArgs(_foods.Count));
            }
        }

        public void RemoveFood()
        {
            if (_isInitialized && _foods.Count > 0)
            {
                _foods[_foods.Count - 1].Remove();
                _foods.RemoveAt(_foods.Count - 1);
                FoodAmountChanged?.Invoke(this, new GMFoodAmountChangedEventArgs(_foods.Count));
            }
        }

        public void ChangePlayfieldGridSize(int newGridSize)
        {
            _pfSize = newGridSize;
            _playfield.Height = newGridSize * Player.SIZE;
            _playfield.Width = newGridSize * Player.SIZE;
            PlayfieldSizeChanged.Invoke(this, new GMPlayfieldSizeChangedEventArgs(newGridSize));
        }

        public void ChangePlayfieldBrush(Brush newBrush)
        {
            Brush shush = new SolidColorBrush();

            _playfield.Background = newBrush;
        }
    }
}