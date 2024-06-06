using ColorPickerWPF;
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
using Viper.Game.Elements.UI;
using Viper.Game.Events;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Viper.Game.Builders
{
    public class SettingsPanel
    {
        public EventHandler<SettingsPanelStateChangedEventArgs>? StateChanged;

        private Animate _animate = new();

        public int PANEL_SIZE = 300;

        private StackPanel _horStack = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Orientation = Orientation.Horizontal,
            IsHitTestVisible = false,
        };

        private Grid _outside = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
            Width = 2000, // idk.
            Visibility = Visibility.Collapsed,
            IsHitTestVisible = true,
        };

        private Grid _settingsGrid = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(200, 20, 20, 20)),
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
            Margin = new Thickness(0, 70, 0, 0),
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
        };

        private StackPanel _settingsSP = new()
        {
            VerticalAlignment = VerticalAlignment.Top,
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

        private SettingsSection _sectionMaker = new();

        private SettingOption _optionMaker = new();

        public const int ZINDEX = 7;

        public const int TOP_BOTTOM_TITLE_ELEM_MARGIN = 12;

        public const int SIDES_TITLE_ELEM_MARGIN = 25;

        // THESE CONST ARE USED FOR OPTION DESCRIPTIONS:
        private const VerticalAlignment VERTICAL_ALIGNMENT = VerticalAlignment.Top;

        private const HorizontalAlignment HORIZONTAL_ALIGNMENT = HorizontalAlignment.Left;

        private static readonly SolidColorBrush BACKGROUND_BRUSH = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

        private static readonly SolidColorBrush FOREGROUND_BRUSH = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255));

        private const double FONT_SIZE = 11;

        private static readonly FontWeight FONT_WEIGHT = FontWeights.Light;

        private static readonly FontFamily FONT_FAMILY = new FontFamily("Yu Gothic UI Semibold");

        private const TextWrapping TEXT_WRAPPING = TextWrapping.Wrap;

        private static readonly Thickness MARGIN = new Thickness(4, 4, 4, 4);

        /// <summary>
        /// Loads all settings elements.
        /// </summary>
        public void LoadSettingsElements()
        {
            _settingsGrid.RenderTransform = new TranslateTransform(-PANEL_SIZE, 0);
            _settingsGrid.Width = PANEL_SIZE;

            Panel.SetZIndex(_horStack, ZINDEX);
            Panel.SetZIndex(_settingsSP, 1);

            // Bar that shows the title of the panel.
            StackPanel topBar = new()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 70,
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            };

            // StackPanel to arrange title elements.
            StackPanel titleElem = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(SIDES_TITLE_ELEM_MARGIN, TOP_BOTTOM_TITLE_ELEM_MARGIN, SIDES_TITLE_ELEM_MARGIN, TOP_BOTTOM_TITLE_ELEM_MARGIN),
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

            _settingsGrid.Children.Add(topBar);
            _settingsGrid.Children.Add(_settingsSV);
            _settingsGrid.Children.Add(_coolLine);

            _horStack.Children.Add(_settingsGrid);
            _horStack.Children.Add(_outside);
        }

        private void _outside_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SettingsToggle();
        }

        private void SetupSections()
        {
            _sections.Clear();

            SetupGameplaysOptions();
            SetupAudioOptions();
            SetupPlayerPreferencesOptions();
            SetupOverallOptions();
            SetupExtraOptions();

            foreach (StackPanel sc in _sections)
            {
                _settingsSP.Children.Add(sc);
            }
        }

        private void SetupPlayerPreferencesOptions()
        {
            StackPanel section = _sectionMaker.NewMainSection("Preferencias de jugadores");

            UnlimitedSelector unlimitedSelector = new();

            Grid us = unlimitedSelector.NewSelector("Jugador");

            StackPanel bodyColorOption = _optionMaker.NewOption("Color de cuerpo");
            StackPanel strokeColorOption = _optionMaker.NewOption("Color de borde");
            StackPanel playfieldOption = _optionMaker.NewOption("Campo de juego");
            StackPanel tickrateOption = _optionMaker.NewOption("Tickrate");
            StackPanel raiseSpeedOption = _optionMaker.NewOption("Aceleracion");
            StackPanel inputOption = _optionMaker.NewOption("Controles");
            StackPanel colOption = _optionMaker.NewOption("Colisiones");
            StackPanel dirBufferOption = _optionMaker.NewOption("Buffer de direcciones");

            TextBlock bodyColorDesc = new()
            {
                Text = "Cambia el color del cuerpo de la vibora",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock strokeColorDesc = new()
            {
                Text = "Cambia el color del borde de la vibora",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock pfDesc = new()
            {
                Text = "Cambiar tamaño",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock tickrateDesc = new()
            {
                Text = "Cambia la velocidad del jugador, menos es mas rapido",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock raiseDesc = new()
            {
                Text = "Aumenta la velocidad del jugador a medida que consigue mas puntos",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock inputDesc = new()
            {
                Text = "Cambia los controles del jugador actual",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock colisionDesc = new()
            {
                Text = "Elige si quieres que el jugador choque y pierda con ciertas cosas",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock bufferDesc = new()
            {
                Text = "Permite 'Stakear' multiples entradas de movimientos en un corto tiempo, util para cuando la vibora alcanza velocidades demasiado altas haciendo que doblar se vuelva complicado, desactivalo si prefieres jugar de una manera mas dificil y 'cruda' por asi decirlo",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };
            CustomComboBox ccb1 = new();

            CustomButton cb1 = new();
            CustomButton cb2 = new();
            CustomButton cb3 = new();
            CustomButton cb4 = new();

            CustomSlider slider1 = new();
            CustomSlider slider2 = new();

            CustomCheckBox check1 = new();
            CustomCheckBox check2 = new();
            CustomCheckBox check3 = new();
            CustomCheckBox check4 = new();

            StackPanel bodyColorButton = cb1.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Seleccionar Color" });
            StackPanel inputButton = cb2.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Asignar controles" });
            StackPanel pfCustomizationButton = cb3.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Cambiar fondo" });
            StackPanel strokeColorButton = cb4.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Seleccionar Color" });

            Slider tickrateSlider = slider1.NewSlider();
            Slider pfSizeSlider = slider2.NewSlider();

            StackPanel raiseCheck = check1.NewCheckBox("Activar aceleracion");
            StackPanel wallCheck = check2.NewCheckBox("Colision con paredes");
            StackPanel selfCheck = check3.NewCheckBox("Colision propia");
            StackPanel bufferCheck = check4.NewCheckBox("Activar buffer");

            us.Margin = new Thickness(7, 0, 7, 0);
            bodyColorButton.Margin = new Thickness(0, 7, 0, 0);
            strokeColorButton.Margin = new Thickness(0, 7, 0, 0);

            bodyColorOption.Children.Add(bodyColorDesc);
            bodyColorOption.Children.Add(bodyColorButton);

            strokeColorOption.Children.Add(strokeColorDesc);
            strokeColorOption.Children.Add(strokeColorButton);

            tickrateOption.Children.Add(tickrateDesc);
            tickrateOption.Children.Add(tickrateSlider);

            raiseSpeedOption.Children.Add(raiseDesc);
            raiseSpeedOption.Children.Add(raiseCheck);

            inputOption.Children.Add(inputDesc);
            inputOption.Children.Add(inputButton);

            colOption.Children.Add(colisionDesc);
            colOption.Children.Add(wallCheck);
            colOption.Children.Add(selfCheck);

            dirBufferOption.Children.Add(bufferDesc);
            dirBufferOption.Children.Add(bufferCheck);

            playfieldOption.Children.Add(pfDesc);
            playfieldOption.Children.Add(pfSizeSlider);
            playfieldOption.Children.Add(pfCustomizationButton);

            section.Children.Add(us);
            section.Children.Add(bodyColorOption);
            section.Children.Add(strokeColorOption);
            section.Children.Add(tickrateOption);
            section.Children.Add(raiseSpeedOption);
            section.Children.Add(inputOption);
            section.Children.Add(colOption);
            section.Children.Add(dirBufferOption);
            section.Children.Add(playfieldOption);

            _sections.Add(section);
        }

        private void SetupGameplaysOptions()
        {
            StackPanel section = _sectionMaker.NewMainSection("Gameplay");

            StackPanel gameSessionSizeOption = _optionMaker.NewOption("Tamaño de gameplay");

            StackPanel HUDSizeOption = _optionMaker.NewOption("Tamaño de HUD");

            StackPanel bgOption = _optionMaker.NewOption("Fondo de pantalla");

            TextBlock gssoDesc = new()
            {
                Text = "Cambia el zoom de la 'ventana' que muestra todos los jugadores",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock hudDesc = new()
            {
                Text = "Cambia el tamaño de la interfaz que muestra los puntos, el tiempo, etc",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock bgDesc = new()
            {
                Text = "Elige una imagen o un color solido para tener de fondo",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock bgPathDesc = new()
            {
                Text = "No has elegido ninguna imagen",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = new FontFamily("Calibri"),
                TextWrapping = TEXT_WRAPPING,
                FontStyle = FontStyles.Italic,
                Margin = MARGIN
            };

            CustomButton cb1 = new();

            StackPanel bgButton = cb1.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Seleccionar imagen o color de fondo" });

            CustomSlider slider1 = new();
            CustomSlider slider2 = new();

            Slider gmSizeSlider = slider1.NewSlider();
            Slider hudSizeSlider = slider2.NewSlider();

            bgButton.Margin = new Thickness(0, 7, 0, 0);

            gameSessionSizeOption.Children.Add(gssoDesc);
            gameSessionSizeOption.Children.Add(gmSizeSlider);

            HUDSizeOption.Children.Add(hudDesc);
            HUDSizeOption.Children.Add(hudSizeSlider);

            bgOption.Children.Add(bgDesc);
            bgOption.Children.Add(bgButton);
            bgOption.Children.Add(bgPathDesc);

            section.Children.Add(gameSessionSizeOption);
            section.Children.Add(HUDSizeOption);
            section.Children.Add(bgOption);

            _sections.Add(section);
        }

        private void SetupAudioOptions()
        {
            StackPanel section = _sectionMaker.NewMainSection("Sonido");

            StackPanel musicVolumeOption = _optionMaker.NewOption("Volumen de musica");

            StackPanel sfxVolumeOption = _optionMaker.NewOption("Volumen de efectos");

            StackPanel bgMusicOption = _optionMaker.NewOption("Musica de fondo");

            TextBlock volumeBGDesc = new()
            {
                Text = "Musica",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock volumeSFXDesc = new()
            {
                Text = "Efectos de sonido",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock bgMusicDesc = new()
            {
                Text = "Pon tu propia musica mientras juegas el juego",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock bgMusicPathDesc = new()
            {
                Text = "No has elegido ninguna cancion",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = new FontFamily("Calibri"),
                TextWrapping = TEXT_WRAPPING,
                FontStyle = FontStyles.Italic,
                Margin = MARGIN
            };

            CustomButton cb1 = new();

            StackPanel bgMusicButton = cb1.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Seleccionar cancion" });

            CustomSlider slider1 = new();
            CustomSlider slider2 = new();

            Slider bgSlider = slider1.NewSlider();
            Slider sfxSlider = slider2.NewSlider();

            musicVolumeOption.Children.Add(volumeBGDesc);
            musicVolumeOption.Children.Add(bgSlider);

            sfxVolumeOption.Children.Add(volumeSFXDesc);
            sfxVolumeOption.Children.Add(sfxSlider);

            bgMusicOption.Children.Add(bgMusicDesc);
            bgMusicOption.Children.Add(bgMusicButton);
            bgMusicOption.Children.Add(bgMusicPathDesc);

            section.Children.Add(musicVolumeOption);
            section.Children.Add(sfxVolumeOption);
            section.Children.Add(bgMusicOption);

            _sections.Add(section);
        }

        private void SetupOverallOptions()
        {
            StackPanel section = _sectionMaker.NewMainSection("Juego");

            StackPanel animOption = _optionMaker.NewOption("Animaciones");

            TextBlock animDesc = new()
            {
                Text = "Aveces, tener muchos movimientos en pantalla puede ser mareante o confuso, aqui puedes desactivar las animaciones si tienes ganas",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            CustomCheckBox check1 = new();
            StackPanel animCheck = check1.NewCheckBox("Desactivar animaciones");

            animOption.Children.Add(animDesc);
            animOption.Children.Add(animCheck);

            section.Children.Add(animOption);

            _sections.Add(section);
        }

        private void SetupExtraOptions()
        {
            StackPanel section = _sectionMaker.NewMainSection("????");

            StackPanel catOption = _optionMaker.NewOption("The cat");

            StackPanel rotateOption = _optionMaker.NewOption("Rotate");

            TextBlock catDesc = new()
            {
                Text = "Life or bath for dry cat",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            TextBlock rotateDesc = new()
            {
                Text = "Do a barrel roll",
                VerticalAlignment = VERTICAL_ALIGNMENT,
                HorizontalAlignment = HORIZONTAL_ALIGNMENT,
                Background = BACKGROUND_BRUSH,
                Foreground = FOREGROUND_BRUSH,
                FontSize = FONT_SIZE,
                FontWeight = FONT_WEIGHT,
                FontFamily = FONT_FAMILY,
                TextWrapping = TEXT_WRAPPING,
                Margin = MARGIN
            };

            CustomButton cb1 = new();
            CustomButton cb2 = new();
            CustomButton cb3 = new();

            StackPanel catButton1 = cb1.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "life :3" });
            StackPanel catButton2 = cb2.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "bath!!! >:3" });
            StackPanel rotateButton = cb3.NewButton(new TextBlock() { Foreground = new SolidColorBrush(Colors.White), Text = "Do the thing" });

            catButton2.Margin = new Thickness(0, 7, 0, 0);

            catOption.Children.Add(catDesc);
            catOption.Children.Add(catButton1);
            catOption.Children.Add(catButton2);

            rotateOption.Children.Add(rotateDesc);
            rotateOption.Children.Add(rotateButton);

            section.Children.Add(catOption);
            section.Children.Add(rotateOption);

            _sections.Add(section);
        }

        public void End()
        {
            _settingsSV.Content = null;
            _settingsSP.Children.Clear();
            _settingsGrid.Children.Clear();
            _horStack.Children.Clear();
        }

        public void SettingsToggle()
        {
            if (_switch)
            {
                ShowSettings();
                StateChanged?.Invoke(this, new SettingsPanelStateChangedEventArgs(true));
            }
            else
            {
                HideSettings();
                StateChanged?.Invoke(this, new SettingsPanelStateChangedEventArgs(false));
            }

            _switch = !_switch;
        }

        private void ShowSettings()
        {
            _outside.PreviewMouseDown += _outside_PreviewMouseDown;
            _animate.Position(_settingsGrid, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 300, 0);
            _animate.Opacity(_settingsGrid, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
            _animate.Opacity(_coolLine, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 700, 0, 1);
            _horStack.IsHitTestVisible = true;
            _outside.Visibility = Visibility.Visible;
            _isShowingSettings = true;
        }

        private async void HideSettings()
        {
            _outside.PreviewMouseDown -= _outside_PreviewMouseDown;
            _animate.Position(_settingsGrid, new TranslateTransform(-PANEL_SIZE, 0), new ExponentialEase() { EasingMode = EasingMode.EaseIn }, 300, 0);
            _animate.Opacity(_settingsGrid, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 500, 0);
            _horStack.IsHitTestVisible = false;
            _outside.Visibility = Visibility.Collapsed;
            _isShowingSettings = false;
        }
    }
}
