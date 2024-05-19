using System.Diagnostics;
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

namespace Viper.Game.Managers
{
    /// <summary>
    /// Manages a playfield with a player and custom amounts of food.
    /// </summary>
    public class GameplayManager
    {
        private int _points = 0;

        public EventHandler<PlayerPointChangedEventArgs>? PointsChanged;

        public int Points { get { return _points; } }

        private bool _hasStarted = false;

        private Grid _manager = new()
        {
            Height = 1,
            Width = 1,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private Playfield _playfield = new();

        private Player _player = new();

        private List<Food> _foods = new();

        public Grid Displayer
        {
            get
            {
                return _manager;
            }
        }

        public Playfield Playfield
        {
            get
            {
                return _playfield;
            }
        }

        public Player Player
        {
            get
            {
                return _player;
            }
        }

        public List<Food> Foods
        {
            get
            {
                return _foods;
            }
        }

        public void Start()
        {
            if (!_hasStarted)
            {
                _hasStarted = true;

                _player.Show(_playfield.CurrentPlayfield);
            }            
        }

        public void CleanUp()
        {
            _hasStarted = false;
            _playfield.CurrentPlayfield.Children.Clear();
        }

        public void AddFood()
        {
            Food food = new();

            food.Show(_playfield.CurrentPlayfield);

            _foods.Add(food);
        }
    }
}