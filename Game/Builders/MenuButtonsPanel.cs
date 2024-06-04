using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using Viper.Game.Animations;
using Viper.Game.Elements.UI;

namespace Viper.Game.Builders
{
    public class MenuButtonsPanel
    {
        private Animate _animate = new();

        /// <summary>
        /// Triggers when the user clicks "Play"
        /// </summary>
        public event EventHandler? PlayClicked;

        /// <summary>
        /// Triggers when the user clicks "Settings"
        /// </summary>
        public event EventHandler? SettingsClicked;

        /// <summary>
        /// Triggers when the user clicks "Info"
        /// </summary>
        public event EventHandler? InfoClicked;

        /// <summary>
        /// Triggers when the user clicks Exit
        /// </summary>
        public event EventHandler? ExitClicked;

        public const int BUTTON_STACKPANEL_WIDTH = 250;

        public const int BUTTON_BOTTOM_MARGIN = 12;

        public const int BUTTON_ANIM_TIME = 200;

        public const double TRANSLATETRANSFORM_FROM_POSITION = 120;

        public StackPanel NewMenuPanel()
        {
            TranslateTransform from = new TranslateTransform(TRANSLATETRANSFORM_FROM_POSITION, 0);
            TranslateTransform to = new TranslateTransform(0, 0);

            StackPanel buttonStackPanel = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, -100),
                Width = BUTTON_STACKPANEL_WIDTH,
            };

            CustomButton start = new();
            CustomButton settings = new();
            CustomButton info = new();
            CustomButton exit = new();

            StackPanel startButton = start.NewButton("Jugar");
            startButton.Margin = new Thickness(0, 0, 0, BUTTON_BOTTOM_MARGIN);
            startButton.RenderTransform = from;
            startButton.Opacity = 0;

            StackPanel settingsButton = settings.NewButton("Ajustes");
            settingsButton.Margin = new Thickness(0, 0, 0, BUTTON_BOTTOM_MARGIN);
            settingsButton.RenderTransform = from;
            settingsButton.Opacity = 0;

            StackPanel infoButton = info.NewButton("Info");
            infoButton.Margin = new Thickness(0, 0, 0, BUTTON_BOTTOM_MARGIN);
            infoButton.RenderTransform = from;
            infoButton.Opacity = 0;

            StackPanel exitButton = exit.NewButton("Salir del juego");
            exitButton.Margin = new Thickness(0, 0, 0, BUTTON_BOTTOM_MARGIN);
            exitButton.RenderTransform = from;
            exitButton.Opacity = 0;

            ButtonApearAnimation();

            buttonStackPanel.Children.Add(startButton);

            buttonStackPanel.Children.Add(settingsButton);

            buttonStackPanel.Children.Add(infoButton);

            buttonStackPanel.Children.Add(exitButton);

            start.Clicked += (s, e) =>
            {
                PlayClicked?.Invoke(this, new EventArgs());
            };

            settings.Clicked += (s, e) =>
            {
                SettingsClicked?.Invoke(this, new EventArgs());
            };

            info.Clicked += (s, e) =>
            {
                InfoClicked?.Invoke(this, new EventArgs());
            };

            exit.Clicked += (s, e) =>
            {
                ExitClicked?.Invoke(this, new EventArgs());
            };

            async void ButtonApearAnimation()
            {
                _animate.Position(startButton, to, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);
                _animate.Opacity(startButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);

                await Task.Delay(20);

                _animate.Position(settingsButton, to, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);
                _animate.Opacity(settingsButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);

                await Task.Delay(20);

                _animate.Position(infoButton, to, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);
                _animate.Opacity(infoButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);

                await Task.Delay(20);

                _animate.Position(exitButton, to, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);
                _animate.Opacity(exitButton, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, BUTTON_ANIM_TIME, 0);
            }

            return buttonStackPanel;
        }
    }
}
