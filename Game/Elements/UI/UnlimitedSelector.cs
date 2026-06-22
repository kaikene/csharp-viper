using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Viper.Game.Animations;
using Viper.Game.Events;

namespace Viper.Game.Elements.UI
{
    public class UnlimitedSelector
    {
        public EventHandler<UnlimitedSelectorIndexChangedEventArgs>? IndexChanged;

        private Grid _selector = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 27,
            Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40)),
        };

        private TextBlock _selectorText = new()
        {
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
        };

        private Grid _left = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(255, 20, 20, 20)),
            Width = 20,
        };

        private Grid _right = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            Background = new SolidColorBrush(Color.FromArgb(255, 20, 20, 20)),
            Width = 20,
        };

        private int _currentIndex = 1;

        public Grid NewSelector(string text)
        {
            _selectorText.Text = $"{text} {_currentIndex}";

            IndexChanged?.Invoke(this, new UnlimitedSelectorIndexChangedEventArgs(_currentIndex));

            TextBlock leftArrow = new()
            {
                Text = "«",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            };

            TextBlock rightArrow = new()
            {
                Text = "»",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            };

            _left.PreviewMouseLeftButtonDown += _left_PreviewMouseLeftButtonDown;
            _left.MouseEnter += _left_MouseEnter;
            _left.MouseLeave += _left_MouseLeave;

            _right.PreviewMouseLeftButtonDown += _right_PreviewMouseLeftButtonDown;
            _right.MouseEnter += _right_MouseEnter;
            _right.MouseLeave += _right_MouseLeave;

            void _left_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                Animate a = new();

                if (_currentIndex - 1 != 0)
                {
                    _currentIndex--;

                    IndexChanged?.Invoke(this, new UnlimitedSelectorIndexChangedEventArgs(_currentIndex));

                    _selectorText.Text = $"{text} {_currentIndex}";

                    a.Color(_left, Animate.ColorProperty.Background, Color.FromArgb(255, 30, 30, 30), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Colors.White);
                    a.Color(leftArrow, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Colors.Black);

                    a.Position(_selectorText, new TranslateTransform(0, 0), new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 3 }, 500, 0, new TranslateTransform(-10, 0));
                }
                else
                {
                    a.Color(_left, Animate.ColorProperty.Background, Color.FromArgb(255, 30, 30, 30), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Color.FromArgb(255, 255, 52, 41));
                    a.Color(leftArrow, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Colors.Black);
                }
            }

            async void _right_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                Animate a = new();

                _currentIndex++;

                IndexChanged?.Invoke(this, new UnlimitedSelectorIndexChangedEventArgs(_currentIndex));

                _selectorText.Text = $"{text} {_currentIndex}";

                a.Color(_right, Animate.ColorProperty.Background, Color.FromArgb(255, 30, 30, 30), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Colors.White);
                a.Color(rightArrow, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Colors.Black);

                a.Position(_selectorText, new TranslateTransform(0, 0), new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 6 }, 500, 0, new TranslateTransform(10, 0));
            }

            void _left_MouseEnter(object sender, MouseEventArgs e)
            {
                Animate a = new();

                a.Color(_left, Animate.ColorProperty.Background, Color.FromArgb(255, 30, 30, 30), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            }

            void _left_MouseLeave(object sender, MouseEventArgs e)
            {
                Animate a = new();

                a.Color(_left, Animate.ColorProperty.Background, Color.FromArgb(255, 20, 20, 20), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            }

            void _right_MouseEnter(object sender, MouseEventArgs e)
            {
                Animate a = new();
                a.Color(_right, Animate.ColorProperty.Background, Color.FromArgb(255, 30, 30, 30), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            }

            void _right_MouseLeave(object sender, MouseEventArgs e)
            {
                Animate a = new();

                a.Color(_right, Animate.ColorProperty.Background, Color.FromArgb(255, 20, 20, 20), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            }

            _left.Children.Add(leftArrow);

            _right.Children.Add(rightArrow);

            _selector.Children.Add(_left);
            _selector.Children.Add(_selectorText);
            _selector.Children.Add(_right);

            return _selector;
        }
    }
}
