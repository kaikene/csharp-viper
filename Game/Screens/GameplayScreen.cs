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
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
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

        /// <summary>
        /// Loads and shows the gameplay screen.
        /// </summary>
        public void Show()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                TextBlock screenIdetifier = new()
                {
                    Text = "Viper.Game.Screens.GameplayScreen",
                    FontSize = 15,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(70, 255, 255, 255)),
                    Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
                };

                _gameplay.Children.Add(screenIdetifier);
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
            _isLoaded = false;
            _gameplay.Children.Clear();
        }
    }
}
