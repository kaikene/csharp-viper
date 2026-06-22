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
using System.Reflection.Metadata;

namespace Viper.Game.Elements.UI
{
    public class CustomButton
    {
        private Animate _animate = new();

        public EventHandler? Clicked;

        private StackPanel _button = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40)),
        };

        private FrameworkElement _buttonContent = new();

        public FrameworkElement ButtonContent
        {
            get
            {
                return _buttonContent;
            }

            set
            {
                _buttonContent = value;

                _button.Children.Clear();

                value.VerticalAlignment = VerticalAlignment.Center;
                value.HorizontalAlignment = HorizontalAlignment.Center;

                value.Margin = new Thickness(0, 6, 0, 6);

                _button.Children.Add(_buttonContent);
            }
        }

        public StackPanel NewButton(FrameworkElement content)
        {
            _button.Children.Add(content);

            content.VerticalAlignment = VerticalAlignment.Center;
            content.HorizontalAlignment = HorizontalAlignment.Center;

            content.Margin = new Thickness(0, 6, 0, 6);

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
                Clicked?.Invoke(this, new EventArgs());
            }

            _button.MouseEnter += Element_MouseEnter;

            _button.MouseLeave += Element_MouseLeave;

            _button.PreviewMouseLeftButtonDown += Button_PreviewMouseLeftButtonUp;

            return _button;
        }
    }
}
