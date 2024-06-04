using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Viper.Game.Animations;
using System.Windows.Media.Animation;

namespace Viper.Game.Builders
{
    public class SettingsSection
    {
        public const int BOTTOM_SECTION_MARGIN = 6;

        public const int TOP_BOTTOM_TITLE_MARGIN = 10;

        public const int SIDES_TITLE_MARGIN = 12;

        public StackPanel NewMainSection(string title)
        {
            StackPanel section = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, BOTTOM_SECTION_MARGIN),
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Opacity = 0.5,
            };

            TextBlock name = new()
            {
                Text = title,
                Margin = new Thickness(SIDES_TITLE_MARGIN, TOP_BOTTOM_TITLE_MARGIN, SIDES_TITLE_MARGIN, TOP_BOTTOM_TITLE_MARGIN),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Yu Gothic UI Semibold"),
            };

            section.MouseEnter += Section_MouseEnter;
            section.MouseLeave += Section_MouseLeave;

            void Section_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Animate a = new();

                a.Opacity(section, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }

            void Section_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Animate a = new();

                a.Opacity(section, 0.5, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }

            section.Children.Add(name);

            return section;
        }
    }
}
