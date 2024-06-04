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

        private TextBlock _checkText = new()
        {
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            Margin = new Thickness(5, 0, 0, 0),
        };

        public bool State { get; private set; }

        private string _text = "";

        public string CheckText
        {
            get
            {
                return _text;
            }

            private set
            {
                _text = value;

                _checkText.Text = value;
            }
        }

        private Rectangle _checkBox = new()
        {
            Height = 20,
            Width = 20,
            StrokeThickness = 4,
            Stroke = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0)),
            Fill = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255)),
        };

        private StackPanel _checkContainer = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            Orientation = Orientation.Horizontal,
            Height = 25,
        };

        public StackPanel NewCheckBox(string text)
        {
            _checkText.Text = text;

            _checkContainer.Children.Add(_checkBox);
            _checkContainer.Children.Add(_checkText);

            _checkContainer.MouseEnter += OnMouseEnter;
            _checkContainer.MouseLeave += OnMouseLeave;

            _checkContainer.MouseLeftButtonUp += CheckBox_MouseLeftButtonUp;
            _checkContainer.MouseLeftButtonDown += CheckBox_MouseLeftButtonDown;

            CheckToggle(false);

            return _checkContainer;
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!State)
            {
                _animate.Color(_checkBox, Animate.ColorProperty.Fill, Color.FromArgb(120, 255, 255, 255), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            }
        }

        private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!State)
            {
                _animate.Color(_checkBox, Animate.ColorProperty.Fill, Color.FromArgb(50, 255, 255, 255), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            }
        }

        private void CheckBox_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CheckToggle();
        }

        private void CheckBox_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _animate.Height(_checkBox, 15, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 1000, 0);
            _animate.Width(_checkBox, 15, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 1000, 0);
        }

        public void CheckToggle(bool? forceState = null)
        {
            if (forceState != null)
            {
                State = !(bool)forceState;
            }

            _animate.Height(_checkBox, 20, new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 2 }, 1000, 0);
            _animate.Width(_checkBox, 20, new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 2 }, 1000, 0);

            State = !State;
            StateChanged?.Invoke(this, new CustomCheckBoxStateChangedEventArgs(State));

            if (State)
            {
                _animate.Color(_checkBox, Animate.ColorProperty.Fill, Color.FromArgb(255, 255, 255, 255), new SineEase(), 200, 0);
                _animate.Color(_checkBox, Animate.ColorProperty.Stroke, Color.FromArgb(120, 0, 0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Color.FromArgb(255, 255, 255, 255));
                _animate.Color(_checkText, Animate.ColorProperty.Foreground, Color.FromArgb(255, 255, 255, 255), new SineEase(), 200, 0);
            }
            else
            {
                _animate.Color(_checkBox, Animate.ColorProperty.Fill, Color.FromArgb(50, 255, 255, 255), new SineEase(), 200, 0);
                _animate.Color(_checkBox, Animate.ColorProperty.Stroke, Color.FromArgb(120, 0, 0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, Color.FromArgb(0, 0, 0, 0));
                _animate.Color(_checkText, Animate.ColorProperty.Foreground, Color.FromArgb(120, 255, 255, 255), new SineEase(), 200, 0);
            }
        }
    }
}
