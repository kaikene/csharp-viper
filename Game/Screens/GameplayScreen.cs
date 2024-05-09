using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Viper.Game.Elements;
using Viper.Game.Managers;

namespace Viper.Game.Screens
{
    public class GameplayScreen()
    {
        private bool isCurrentlyShowing = false;

        private Grid _gameplayContainer = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0)),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public Grid GameplayContainer
        {
            get
            {
                return _gameplayContainer;
            }
        }

        public void Show()
        {
            if (!isCurrentlyShowing)
            {
                isCurrentlyShowing = true;

                TextBlock screenIdetifier = new()
                {
                    Text = "Viper.Game.Screens.GameplayScreen",
                    FontSize = 15,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(70, 255, 255, 255)),
                    Background = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0)),
                };

                _gameplayContainer.Children.Add(screenIdetifier);
            }
        }

        public void CleanUp()
        {
            isCurrentlyShowing = false;
            _gameplayContainer.Children.Clear();
        }
    }
}
