using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Viper.Game.Animations;
using Viper.Game.Builders;
using Viper.Game.Interfaces;

namespace Viper.Game.Screens
{
    /// <summary>
    /// The menu screen of the game.
    /// </summary>
    public class MenuScreen : IScreenStates
    {
        // Container that handles all menu elements.
        private Grid _menu = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsHitTestVisible = false,
            Visibility = Visibility.Hidden,
        };

        /// <summary>
        /// Container that handles all menu elements.
        /// </summary>
        public Grid Container { get { return _menu; } }

        private bool _isHidden = false;

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public MenuButtonsPanel ButtonPanel = new();

        public bool IsHidden { get { return _isHidden; } }

        /// <summary>
        /// Loads and shows the menu elements.
        /// </summary>
        public void Show()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _menu.IsHitTestVisible = true;
                _menu.Visibility = Visibility.Visible;

                _menu.Children.Add(ButtonPanel.NewMenuPanel());
            }
            else if (_isHidden)
            {
                _menu.Visibility = Visibility.Visible;
                _menu.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        public void Hide()
        {
            if (_isInitialized)
            {
                _menu.Visibility = Visibility.Hidden;
                _menu.IsHitTestVisible = false;
                _isHidden = true;
            }
        }

        public void Clear()
        {
            _menu.Children.Clear();
            _menu.Visibility = Visibility.Hidden;
            _isInitialized = false;
            _menu.IsHitTestVisible = false;
        }
    }
}
