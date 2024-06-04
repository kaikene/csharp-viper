using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Viper.Game.Animations;
using Viper.Game.Events;

namespace Viper.Game.Elements.UI
{
    public class CustomComboBox
    {
        private Animate _animate = new();

        public EventHandler<CustomComboBoxSelectionChangedEventArgs>? SelectionChanged;
        public EventHandler<CustomComboBoxElementAmountChangedEventArgs>? ElementAmountChanged; 

        private TextBlock _selectedElementText = new()
        {
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Margin = new System.Windows.Thickness(6, 4, 4, 6),
            TextWrapping = System.Windows.TextWrapping.WrapWithOverflow,
        };

        private StackPanel _elements = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
        };

        private StackPanel _comboBox = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
        };

        private StackPanel _selector = new()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 20, 20, 20)),
        };

        private ScrollViewer _elementsSV = new()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
            Background = new SolidColorBrush(Color.FromArgb(120, 20, 20, 20)),
            Height = 0,
        };

        private int _elementAmount = 0;

        public int ElementAmount
        {
            get
            {
                return _elementAmount;
            }
        }

        private string _selectedElementInfo = "";

        public string SelectedElementName
        {
            get
            {
                return _selectedElementInfo;
            }
        }

        private int _selectedElementIndex = -1;

        public int SelectedElementIndex
        {
            get
            {
                return _selectedElementIndex;
            }
        }

        bool _canOpen = false;

        private List<string> _elementsText = new();

        private bool _isSomethingSelected = false, _isMouseOverSelector = false;

        public StackPanel NewComboBox()
        {
            SetNothingToSelect();

            _selector.PreviewMouseLeftButtonUp += Selector_PreviewMouseLeftButtonUp;
            _selector.MouseEnter += OnMouseEnter;
            _selector.MouseLeave += OnMouseLeave;

            void Selector_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                OpenCloseToggle();
            }

            void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                _isMouseOverSelector = true;

                if (!_canOpen)
                {
                    _animate.Color(_selector, Animate.ColorProperty.Background, Color.FromArgb(255, 40, 40, 40), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);
                }
            }

            void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                _isMouseOverSelector = false;

                if (!_canOpen)
                {
                    _animate.Color(_selector, Animate.ColorProperty.Background, Color.FromArgb(255, 20, 20, 20), new QuadraticEase() { EasingMode = EasingMode.EaseIn }, 200, 0);
                }
            }

            _elementsSV.Content = _elements;

            _selector.Children.Add(_selectedElementText);

            _comboBox.Children.Add(_selector);
            _comboBox.Children.Add(_elementsSV);

            return _comboBox;
        }

        private void OpenCloseToggle()
        {
            _canOpen = !_canOpen;

            if (_canOpen)
            {
                _elementsSV.ScrollToTop();
                _animate.Height(_elementsSV, 100, new ElasticEase() { EasingMode = EasingMode.EaseOut, Springiness = 13 }, 1000, 0);
                _animate.Color(_selector, Animate.ColorProperty.Background, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
                _animate.Color(_selectedElementText, Animate.ColorProperty.Foreground, Colors.Black, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }
            else
            {
                _animate.Height(_elementsSV, 0, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 200, 0);

                if (_isMouseOverSelector)
                {
                    _animate.Color(_selector, Animate.ColorProperty.Background, Color.FromArgb(255, 40, 40, 40), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
                }
                else
                {
                    _animate.Color(_selector, Animate.ColorProperty.Background, Color.FromArgb(255, 20, 20, 20), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
                }

                _animate.Color(_selectedElementText, Animate.ColorProperty.Foreground, Colors.White, new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }
        }

        public void AddElement(string newElement)
        {
            _elementAmount++;

            int currentIndex = _elementAmount - 1;

            if (!_isSomethingSelected)
            {
                AskSelectSomething();
            }

            StackPanel elementBox = new()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            };

            TextBlock elementText = new()
            {
                Text = newElement,
                Foreground = new SolidColorBrush(Colors.White),
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Margin = new System.Windows.Thickness(6, 4, 4, 6),
                TextWrapping = System.Windows.TextWrapping.WrapWithOverflow,
            };

            _elementsText.Add(elementText.Text);

            elementBox.MouseEnter += OnMouseEnter;
            elementBox.MouseLeave += OnMouseLeave;
            elementBox.PreviewMouseLeftButtonUp += ElementBox_PreviewMouseLeftButtonUp;

            void ElementBox_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                SetSelectionToShow(currentIndex);

                OpenCloseToggle();

                _selectedElementInfo = newElement;
                _selectedElementIndex = currentIndex;

                SelectionChanged?.Invoke(this, new CustomComboBoxSelectionChangedEventArgs(newElement, currentIndex));
            }

            void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                _animate.Color(elementBox, Animate.ColorProperty.Background, Color.FromArgb(60, 255, 255, 255), new QuadraticEase() { EasingMode = EasingMode.EaseOut }, 100, 0);
            }

            void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                _animate.Color(elementBox, Animate.ColorProperty.Background, Color.FromArgb(0, 0, 0, 0), new QuadraticEase() { EasingMode = EasingMode.EaseIn }, 100, 0);
            }

            elementBox.Children.Add(elementText);

            _elements.Children.Add(elementBox);
        }

        public void RemoveAt(int elementIndex)
        {
            if (((_elements.Children[elementIndex] as StackPanel).Children[0] as TextBlock).Text == _selectedElementText.Text)
            {
                AskSelectSomething();
            }

            _elements.Children.RemoveAt(elementIndex);
            _elementAmount--;

            if (_elementAmount == 0)
            {
                SetNothingToSelect();
            }

            ElementAmountChanged?.Invoke(this, new CustomComboBoxElementAmountChangedEventArgs(_elementAmount));
        }

        private void SetNothingToSelect()
        {
            _selectedElementText.Text = "Theres nothing to select!";
            _selectedElementText.Opacity = 0.5;
            _selectedElementText.FontStyle = FontStyles.Italic;
            _isSomethingSelected = false;
            SelectionChanged?.Invoke(this, new CustomComboBoxSelectionChangedEventArgs("", -1));
        }

        public void SetSelectionToShow(int selectionIndex)
        {
            _selectedElementText.Text = _elementsText[selectionIndex];
            _selectedElementText.Opacity = 1;
            _selectedElementText.FontStyle = FontStyles.Normal;
            _isSomethingSelected = true;
        }

        private void AskSelectSomething()
        {
            _selectedElementText.Text = "Nothing is selected!";
            _selectedElementText.Opacity = 0.7;
            _selectedElementText.FontStyle = FontStyles.Italic;
            _isSomethingSelected = false;
            SelectionChanged?.Invoke(this, new CustomComboBoxSelectionChangedEventArgs("", -1));
        }
    }
}
