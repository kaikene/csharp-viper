using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Viper.Game.Animations;
using Viper.Game.Elements;
using Viper.Game.Events;
using Viper.Game.Interfaces;
using Viper.Game.Managers;
using Viper.Game.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Viper.Game.Screens
{
    /// <summary>
    /// The gameplay screen.
    /// </summary>
    public class GameplayScreen : IScreenStates
    {
        private Animate _animate = new();

        public EventHandler<GSPlayfieldAmountChangedEventArgs>? PlayfieldAmountChanged;

        public EventHandler<GSScaleChangedEventArgs>? ScaleChanged;

        private int _totalPlayfieldAmount = 0;

        private bool _isHidden = false;

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool IsHidden { get { return _isHidden; } }

        private Grid _gameplay = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsHitTestVisible = false,
            Visibility = Visibility.Hidden

        };

        /// <summary>
        /// The main container handling all gameplay elements.
        /// </summary>
        public Grid Container
        {
            get
            {
                return _gameplay;
            }
        }

        private List<GameplaySession> _gameplayManagers = new();

        public List<GameplaySession> GameplayManagers
        {
            get
            {
                return _gameplayManagers;
            }
        }

        private List<StackPanel> _rows = new();

        private StackPanel _rowManager = new();

        public const double DEFAULT_GM_SCALE = 100;

        private Viewbox _rowManagerVB = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        public double ManagerScale
        {
            get
            {
                return _rowManagerVB.Margin.Right;
            }
        }


        private StackPanel _GMAddRemoveButtons = new()
        {
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 250,
            Margin = new Thickness(0,0,5,0)
        };

        private Button _addGM = new()
        {
            Content = "Añadir Jugador",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2,2,2,2),
            Margin = new Thickness(0, 5, 0, 0)
        };

        private Button _removeGM = new()
        {
            Content = "Quitar Jugador",
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
            Foreground = new SolidColorBrush(Colors.White),
            BorderThickness = new Thickness(2, 2, 2, 2),
            HorizontalContentAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 5, 0, 0)
        };

        private TextBlock _notice = new()
        {
            Text = "No hay jugadores cargados!",
            Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255)),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        private int _playfieldsInRow = 0;

        public void ChangeGameplayScaling(double newScale)
        {
            SettingsHandler.SaveGameplayScale(newScale);
            _rowManagerVB.Margin = new Thickness(newScale, newScale, newScale, newScale);
            ScaleChanged?.Invoke(this, new GSScaleChangedEventArgs(newScale));
        }

        /// <summary>
        /// Loads and shows the gameplay screen.
        /// </summary>
        public void Show()
        {
            if (!_isInitialized)
            {
                double scale = SettingsHandler.GetGameplayScale();
                _rowManagerVB.Margin = new Thickness(scale, scale, scale, scale);

                _isInitialized = true;
                _gameplay.IsHitTestVisible = true;
                _gameplay.Visibility = Visibility.Visible;

                _rowManagerVB.Child = _rowManager;

                _GMAddRemoveButtons.Children.Add(_addGM);
                _GMAddRemoveButtons.Children.Add(_removeGM);

                _addGM.Click += _addGM_Click;
                _removeGM.Click += _removeGM_Click;

                _addGM.MouseEnter += Element_MouseEnter;
                _addGM.MouseLeave += Element_MouseLeave;

                _removeGM.MouseEnter += Element_MouseEnter;
                _removeGM.MouseLeave += Element_MouseLeave;

                _gameplay.Children.Add(_rowManagerVB);
                _gameplay.Children.Add(_GMAddRemoveButtons);

                void ButtonApearAnimation()
                {
                    _animate.Position(_GMAddRemoveButtons, new TranslateTransform(0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, new TranslateTransform(0, 40));
                    _animate.Opacity(_GMAddRemoveButtons, 1, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0, 0);
                }

                if (_totalPlayfieldAmount == 0)
                {
                    _gameplay.Children.Add(_notice);
                }

                ButtonApearAnimation();
            }
            else if (_isHidden)
            {
                _gameplay.Visibility = Visibility.Visible;
                _gameplay.IsHitTestVisible = true;
                _isHidden = false;
            }
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            _animate.Color((Button)sender, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            _animate.Color((Button)sender, Animate.ColorProperty.Foreground, Colors.Black, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
        }

        private void _addGM_Click(object sender, RoutedEventArgs e)
        {
            AddPlayfield();
        }

        private void _removeGM_Click(object sender, RoutedEventArgs e)
        {
            RemovePlayfield();
        }

        public void SetPlayerInputs(int player, Key up, Key down, Key left, Key right)
        {
            if (player <= _gameplayManagers.Count - 1) // If the player beign managed doesnt even exist, then dont attempt to set inputs.
            {
                _gameplayManagers[player].Player.InputUp = up;
                _gameplayManagers[player].Player.InputDown = down;
                _gameplayManagers[player].Player.InputLeft = left;
                _gameplayManagers[player].Player.InputRight = right;

                if (_gameplayManagers[player].Player.InputUp == Key.None && _gameplayManagers[player].Player.InputDown == Key.None && _gameplayManagers[player].Player.InputLeft == Key.None && _gameplayManagers[player].Player.InputRight == Key.None)
                {
                    _inputNotices[player].Text = "Este jugador no tiene controles!";
                }
                else if (_gameplayManagers[player].Player.InputUp != Key.None && _gameplayManagers[player].Player.InputDown != Key.None && _gameplayManagers[player].Player.InputLeft != Key.None && _gameplayManagers[player].Player.InputRight != Key.None)
                {
                    _inputNotices[player].Text = "";
                }
                else
                {
                    _inputNotices[player].Text = "A este jugador le faltan algunas teclas!";
                }
            }

            // TODO: Save inputs anyways to a .ini file, so when the player is loaded, those inputs get set at spawn
        }

        private List<TextBlock> _inputNotices = new();

        public void AddPlayfield()
        {
            if (_isInitialized)
            {
                TextBlock inputNotice = new()
                {
                    Text = "",
                    Foreground = new SolidColorBrush(Color.FromArgb(160, 255, 255, 255)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 6,
                };

                _inputNotices.Add(inputNotice);

                _totalPlayfieldAmount++;

                _notice.Visibility = Visibility.Hidden;

                PlayfieldAmountChanged?.Invoke(this, new GSPlayfieldAmountChangedEventArgs(_totalPlayfieldAmount));

                GameplaySession gm = new();

                gm.Displayer.Margin = new Thickness(5, 5, 5, 5);

                _gameplayManagers.Add(gm);

                // TODO: Go through a list of saved inputs from a .ini file and set it to the intended player loaded.

                if (_gameplayManagers[_gameplayManagers.Count -1].Player.InputUp == Key.None && _gameplayManagers[_gameplayManagers.Count - 1].Player.InputDown == Key.None && _gameplayManagers[_gameplayManagers.Count - 1].Player.InputLeft == Key.None && _gameplayManagers[_gameplayManagers.Count - 1].Player.InputRight == Key.None)
                {
                    inputNotice.Text = "Este jugador no tiene controles!";

                    _gameplayManagers[_gameplayManagers.Count - 1].Displayer.Children.Add(inputNotice);
                }
                else if (_gameplayManagers[_gameplayManagers.Count - 1].Player.InputUp != Key.None && _gameplayManagers[_gameplayManagers.Count - 1].Player.InputDown != Key.None && _gameplayManagers[_gameplayManagers.Count - 1].Player.InputLeft != Key.None && _gameplayManagers[_gameplayManagers.Count - 1].Player.InputRight != Key.None)
                {
                    inputNotice.Text = "A este jugador le faltan algunas teclas!";

                    _gameplayManagers[_gameplayManagers.Count - 1].Displayer.Children.Add(inputNotice);
                }

                if (_playfieldsInRow == 5)
                {
                    _playfieldsInRow = 0;
                }

                if (_playfieldsInRow == 0)
                {
                    StackPanel row = new()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Orientation = Orientation.Horizontal,
                    };

                    _rows.Add(row);

                    _rowManager.Children.Add(_rows[_rows.Count - 1]);
                }

                _playfieldsInRow++;

                _rows[_rows.Count - 1].Children.Add(gm.Displayer);

                gm.LoadElements();
            }
        }

        public void RemovePlayfield()
        {
            if (_isInitialized && _rowManager.Children.Count > 0)
            {
                _inputNotices.RemoveAt(_inputNotices.Count - 1);

                _totalPlayfieldAmount--;

                PlayfieldAmountChanged?.Invoke(this, new GSPlayfieldAmountChangedEventArgs(_totalPlayfieldAmount));

                _rows[_rows.Count - 1].Children.Remove(_gameplayManagers[_gameplayManagers.Count - 1].Displayer);

                _gameplayManagers[_gameplayManagers.Count - 1].End();

                _gameplayManagers.Remove(_gameplayManagers[_gameplayManagers.Count - 1]);

                _playfieldsInRow--;

                if (_playfieldsInRow == 0)
                {
                    _playfieldsInRow = 5;

                    _rowManager.Children.Remove(_rows[_rows.Count - 1]);

                    _rows.Remove(_rows[_rows.Count - 1]);

                    if (_rowManager.Children.Count == 0)
                    {
                        _playfieldsInRow = 0;

                        _notice.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        public void Hide()
        {
            if (_isInitialized)
            {
                _gameplay.Visibility = Visibility.Hidden;
                _gameplay.IsHitTestVisible = false;
                _isHidden = true;
            }
        }

        public void Clear()
        {
            _gameplay.Visibility = Visibility.Hidden;
            _isInitialized = false;
            _isHidden = false;
            _gameplay.Children.Clear();
            _playfieldsInRow = 0;
            _totalPlayfieldAmount = 0;
            PlayfieldAmountChanged?.Invoke(this, new GSPlayfieldAmountChangedEventArgs(_totalPlayfieldAmount));

            _addGM.Click -= _addGM_Click;
            _removeGM.Click -= _removeGM_Click;

            _addGM.MouseEnter -= Element_MouseEnter;
            _addGM.MouseLeave -= Element_MouseLeave;

            _removeGM.MouseEnter -= Element_MouseEnter;
            _removeGM.MouseLeave -= Element_MouseLeave;

            _GMAddRemoveButtons.Children.Clear();

            foreach (GameplaySession gm in _gameplayManagers)
            {
                gm.End();
            }

            _gameplayManagers.Clear();
            _rows.Clear();
            _rowManager.Children.Clear();
            _gameplay.IsHitTestVisible = false;
            _notice.Visibility = Visibility.Visible;
        }
    }
}
