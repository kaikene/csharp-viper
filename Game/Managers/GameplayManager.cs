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
        private Playfield _playfield = new();

        private Player _player = new();

        private List<Food> _food = new();
        
    }
}