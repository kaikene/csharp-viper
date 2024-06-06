using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using Viper.Game.Animations;
using System.Windows.Media.Animation;

namespace Viper.Game.Builders
{
    public class SettingOption
    {
        public const int SIDES_BOTTOM_MARGIN = 12;
        public StackPanel NewOption(string title)
        {
            StackPanel miniSection = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(20, 0, 20, 6),
            };

            StackPanel entireText = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 8, 0, 2),
            };

            StackPanel titlePart = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(-3, 0, 0, 0),
            };

            Rectangle revert = new()
            {
                Height = 15,
                Width = 15,
                Fill = new SolidColorBrush(Color.FromArgb(100, 102, 161, 255)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                ToolTip = "Revertir",
                Stroke = new SolidColorBrush(Color.FromArgb(100, 102, 161, 255)),
                StrokeThickness = 2,
                Margin = new Thickness(0, 1, 4, 0),
            };

            TextBlock name = new()
            {
                Text = title,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255)),
                FontSize = 13,
                FontWeight = FontWeights.Light,
                FontFamily = new FontFamily("Yu Gothic UI Semibold"),
            };

            ToolTipService.SetInitialShowDelay(revert, 100);
            titlePart.Children.Add(revert);
            titlePart.Children.Add(name);

            entireText.Children.Add(titlePart);

            miniSection.Children.Add(entireText);

            revert.MouseEnter += Revert_MouseEnter;
            revert.MouseLeave += Revert_MouseLeave;

            void Revert_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Animate a = new();

                a.Color(revert, Animate.ColorProperty.Stroke, Colors.White, new SineEase(), 200, 0);
            }

            void Revert_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Animate a = new();

                a.Color(revert, Animate.ColorProperty.Stroke, Color.FromArgb(120, 102, 161, 255), new SineEase(), 200, 0);
            }

            return miniSection;
        }
    }
}
