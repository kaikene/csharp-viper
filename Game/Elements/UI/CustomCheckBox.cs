using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Viper.Game.Animations;
using Viper.Game.Events;
using static System.Net.Mime.MediaTypeNames;

namespace Viper.Game.Elements.UI
{
    public class CustomCheckBox
    {
        private Animate _animate = new();

        public EventHandler<CustomCheckBoxStateChangedEventArgs>? StateChanged;

        private bool _state = false;

        private TextBlock _checkText = new()
        {
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            Margin = new Thickness(5, 0, 0, 0),
        };

        public bool State
        {
            get
            {
                return _state;
            }
        }

        private string _text = "";

        public string CheckText
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;

                _checkText.Text = value;
            }
        }

        public StackPanel NewCheckBox(string text)
        {
            Rectangle checkBox = new()
            {
                Height = 20,
                Width = 20,
                StrokeThickness = 4,
                Stroke = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
                Fill = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255)),
            };

            StackPanel checkContainer = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Orientation = Orientation.Horizontal,
                Height = 25,
            };

            _checkText.Text = text;

            checkContainer.Children.Add(checkBox);
            checkContainer.Children.Add(_checkText);

            checkContainer.MouseEnter += OnMouseEnter;

            checkContainer.MouseLeave += OnMouseLeave;

            checkContainer.MouseLeftButtonUp += CheckBox_MouseLeftButtonUp;

            checkContainer.MouseLeftButtonDown += CheckBox_MouseLeftButtonDown;

            void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                if (!_state)
                {
                    _animate.Color(checkBox, Animate.ColorProperty.Fill, Color.FromArgb(120, 255, 255, 255), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                }
            }

            void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                if (!_state)
                {
                    _animate.Color(checkBox, Animate.ColorProperty.Fill, Color.FromArgb(50, 255, 255, 255), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                }
            }

            void CheckBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                _animate.Height(checkBox, 20, new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 2 }, 1000, 0);
                _animate.Width(checkBox, 20, new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 2 }, 1000, 0);

                _state = !_state;
                StateChanged?.Invoke(this, new CustomCheckBoxStateChangedEventArgs(_state));

                if (_state)
                {
                    _animate.Color(checkBox, Animate.ColorProperty.Fill, Color.FromArgb(255, 255, 255, 255), new SineEase(), 200, 0);
                    _animate.Color(checkBox, Animate.ColorProperty.Stroke, Color.FromArgb(120, 0, 0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    _animate.Color(checkBox, Animate.ColorProperty.Fill, Color.FromArgb(50, 255, 255, 255), new SineEase(), 200, 0);
                    _animate.Color(checkBox, Animate.ColorProperty.Stroke, Color.FromArgb(120, 0, 0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Color.FromArgb(0, 0, 0, 0));
                }
            }

            void CheckBox_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                _animate.Height(checkBox, 15, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 1000, 0);
                _animate.Width(checkBox, 15, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 1000, 0);
            }

            return checkContainer;
        }
    }
}
