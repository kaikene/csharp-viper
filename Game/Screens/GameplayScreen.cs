using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Elements;
using Viper.Game.Interfaces;
using Viper.Game.Managers;

namespace Viper.Game.Screens
{
    /// <summary>
    /// The gameplay screen.
    /// </summary>
    public class GameplayScreen : IScreenStates
    {
        private bool _isLoaded = false, _isHidden = false;

        public bool IsLoaded { get { return _isLoaded; } }

        public bool IsHidden { get { return _isHidden; } }

        private Grid _gameplay = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsHitTestVisible = false,
            Visibility = Visibility.Hidden

        };

        /// <summary>
        /// The main container handling all gameplay elements.
        /// </summary>
        public Grid Container
        {
            get
            {
                return _gameplay;
            }
        }

        private GameplayManager _gameplayManager = new();

        public GameplayManager GameplayManager
        {
            get
            {
                return _gameplayManager;
            }
        }

        /// <summary>
        /// Loads and shows the gameplay screen.
        /// </summary>
        public void Show()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                _gameplay.IsHitTestVisible = true;
                _gameplay.Visibility = Visibility.Visible;

                _gameplay.Children.Add(_gameplayManager.Displayer);
            }
            else if (_isHidden)
            {
                _gameplay.Visibility = Visibility.Visible;
                _gameplay.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        public void Hide()
        {
            if (_isLoaded)
            {
                _gameplay.Visibility = Visibility.Hidden;
                _gameplay.IsHitTestVisible = false;
                _isHidden = true;
            }
        }

        public void Clear()
        {
            _gameplay.Visibility = Visibility.Hidden;
            _isLoaded = false;
            _isHidden = false;
            _gameplay.Children.Clear();
            _gameplayManager.End();
            _gameplay.IsHitTestVisible = false;
        }
    }
}
