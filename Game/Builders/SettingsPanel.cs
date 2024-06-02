using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Viper.Game.Animations;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Game.Builders
{
    public class SettingsPanel
    {
        private Animate _animate = new();

        private StackPanel _horStack = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Orientation = Orientation.Horizontal,
            IsHitTestVisible = false,
        };

        private Rectangle _outside = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Width = 2000, // idk.
            IsHitTestVisible = false,
        };

        private Grid _settingsGrid = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(200, 20, 20, 20)),
            Width = 300,
            RenderTransform = new TranslateTransform(-300, 0)
        };

        private StackPanel _settingsMainSP = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private Rectangle _coolLine = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 2,
            Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
            Opacity = 0,
        };

        private ScrollViewer _settingsSV = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
        };

        private StackPanel _settingsSP = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private bool _switch = true;

        public StackPanel Overlay
        {
            get
            {
                return _horStack;
            }
        }

        private bool _isShowingSettings = false;

        public bool IsShowingSettings
        {
            get
            {
                return _isShowingSettings;
            }
        }

        private List<StackPanel> _sections = new();

        public void LoadSettingsElements()
        {
            Panel.SetZIndex(_horStack, 7);

            StackPanel topBar = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            };

            StackPanel titleElem = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(25, 13, 0, 13),
            };

            TextBlock titleText = new()
            {
                Text = "Ajustes",
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                FontSize = 20,
                FontFamily = new FontFamily("Yu Gothic UI Semibold"),
            };

            TextBlock titleDesc = new()
            {
                Text = "Personaliza el juego a tu gusto!",
                Width = 200,
                TextWrapping = TextWrapping.WrapWithOverflow,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(220, 128, 202, 255)),
                FontSize = 10,
                FontFamily = new FontFamily("Yu Gothic UI Semibold"),
            };

            titleElem.Children.Add(titleText);
            titleElem.Children.Add(titleDesc);

            topBar.Children.Add(titleElem);

            _settingsSV.Content = _settingsSP;

            SetupSections();

            foreach (StackPanel section in _sections)
            {
                _settingsSP.Children.Add(section);
            }

            _settingsMainSP.Children.Add(topBar);
            _settingsMainSP.Children.Add(_settingsSV);

            _settingsGrid.Children.Add(_settingsMainSP);
            _settingsGrid.Children.Add(_coolLine);

            _horStack.Children.Add(_settingsGrid);
            _horStack.Children.Add(_outside);
        }

        private StackPanel NewMainSection(string title)
        {
            StackPanel section = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 5),
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
            };

            TextBlock name = new()
            {
                Text = title,
                Margin = new Thickness(12, 12, 12, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255)),
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Yu Gothic UI Semibold"),
            };

            section.Children.Add(name);

            _sections.Add(section);

            return section;
        }

        private void SetupSections()
        {
            StackPanel NewMiniSection(string title, string desc)
            {
                StackPanel miniSection = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(14, 0, 14, 4),
                    Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                };

                StackPanel titlePart = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Orientation = Orientation.Horizontal,
                };

                StackPanel extraInfo = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 5, 0, 5),
                };

                Rectangle revert = new()
                {
                    Height = 9,
                    Width = 9,
                    Fill = new SolidColorBrush(Color.FromArgb(255, 143, 255, 51)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    ToolTip = "Revertir",
                };

                TextBlock name = new()
                {
                    Text = title,
                    Margin = new Thickness(4, 5, 0, 5),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                    FontSize = 13,
                    FontWeight = FontWeights.Light,
                    FontFamily = new FontFamily("Yu Gothic UI Semibold"),
                };

                TextBlock expl = new()
                {
                    Text = desc,
                    Margin = new Thickness(8, 0, 0, 5),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    Foreground = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255)),
                    FontSize = 10,
                    FontWeight = FontWeights.Light,
                    FontFamily = new FontFamily("Yu Gothic UI Semibold"),
                    TextWrapping = TextWrapping.Wrap,
                };

                ToolTipService.SetInitialShowDelay(revert, 100);
                titlePart.Children.Add(revert);
                titlePart.Children.Add(name);

                extraInfo.Children.Add(titlePart);
                extraInfo.Children.Add(expl);

                miniSection.Children.Add(extraInfo);

                return miniSection;
            }

            StackPanel gmpSection = NewMainSection("Seccion mayor");

            StackPanel changeSizeGMMS = NewMiniSection("Seccion menor", "Esto es una descripcion de la seccion menor.");

            gmpSection.Children.Add(changeSizeGMMS);
        }

        public void End()
        {
            _settingsSV.Content = null;
            _settingsSP.Children.Clear();
            _settingsGrid.Children.Clear();
            _horStack.Children.Clear();
        }

        public void SettingsShowHide()
        {
            if (_switch)
            {
                ShowSettings();
            }
            else
            {
                HideSettings();
            }

            _switch = !_switch;
        }

        private void ShowSettings()
        {
            _animate.Position(_settingsGrid, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 300, 0);
            _animate.Opacity(_settingsGrid, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            _animate.Opacity(_coolLine, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 700, 0, 1);
            _horStack.IsHitTestVisible = true;
            _outside.IsHitTestVisible = true;
            _isShowingSettings = true;
        }

        private void HideSettings()
        {
            _animate.Position(_settingsGrid, new TranslateTransform(-300, 0), new ExponentialEase() { EasingMode = EasingMode.EaseIn }, 300, 0);
            _animate.Opacity(_settingsGrid, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 500, 0);
            _horStack.IsHitTestVisible = false;
            _outside.IsHitTestVisible = false;
            _isShowingSettings = false;
        }
    }
}
