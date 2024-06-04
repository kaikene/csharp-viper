using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using Viper.Game.Animations;
using static System.Net.Mime.MediaTypeNames;

namespace Viper.Game.Elements.UI
{
    public class CustomButton
    {
        private Animate _animate = new();

        public EventHandler? Clicked;

        private string _text = "";

        private StackPanel _button = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 27,
            Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40)),
        };

        private TextBlock _buttonText = new()
        {
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            Margin = new Thickness(0, 5, 0, 0),
        };

        public string ButtonText
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;

                _buttonText.Text = value;
            }
        }

        public StackPanel NewButton(string text)
        {
            _text = text;

            _buttonText.Text = text;

            _button.Children.Add(_buttonText);

            void Element_MouseLeave(object sender, MouseEventArgs e)
            {
                _animate.Color(_button, Animate.ColorProperty.Background, Color.FromArgb(255, 40, 40, 40), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }

            void Element_MouseEnter(object sender, MouseEventArgs e)
            {
                _animate.Color(_button, Animate.ColorProperty.Background, Color.FromArgb(255, 50, 50, 50), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }

            void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                _animate.Color(_button, Animate.ColorProperty.Background, Color.FromArgb(255, 50, 50, 50), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Color.FromArgb(255, 200, 200, 200));
                _animate.Color(_buttonText, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Colors.Black);
                Clicked?.Invoke(this, new EventArgs());
            }

            _button.MouseEnter += Element_MouseEnter;

            _button.MouseLeave += Element_MouseLeave;

            _button.PreviewMouseLeftButtonDown += Button_PreviewMouseLeftButtonUp;

            return _button;
        }
    }
}
