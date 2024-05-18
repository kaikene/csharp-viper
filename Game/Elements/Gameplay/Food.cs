using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;
using System.Diagnostics;
using Viper.Game.Events;
using Viper.Game.Interfaces;

namespace Viper.Game.Elements.Gameplay
{
    /// <summary>
    /// Add, remove and manage a food element.
    /// </summary>
    public class Food : IGameplayElements
    {
        /// <summary>
        /// Triggers when the color of the food changed.
        /// </summary>
        public event EventHandler<FoodColorChangedEventArgs>? ColorChanged;

        /// <summary>
        /// Triggers when the position of one of the foods changes.
        /// </summary>
        public event EventHandler<FoodPositionChangedEventArgs>? PositionChanged;

        // Color of the food
        private Color _color = Color.FromArgb(255, 109, 255, 56);

        /// <summary>
        /// Current saved color for the food.
        /// </summary>
        public Color CurrentColor
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// Determines if value change events can be triggered, like if the amount of food changes or if a food changed position, etc.
        /// </summary>
        public bool CanTriggerValueEvents = false;

        // Posiiton of each food element.
        private TranslateTransform _foodPosition = new();

        public TranslateTransform Position
        {
            get
            {
                return _foodPosition;
            }
        }

        public Color Color { get => _color; set => _color = value; }

        // Food element.
        private Rectangle _food = new();

        private bool _showing = false;

        private Panel _parent;

        // Size of the food elements
        public const int SIZE = 10;

        public const byte DEFAULT_ALPHA = 255;

        public const byte DEFAULT_RED = 109;

        public const byte DEFAULT_GREEN = 255;

        public const byte DEFAULT_BLUE = 56;

        public const byte DEFAULT_STROKE_ALPHA = 0;

        public const byte DEFAULT_STROKE_RED = 0;

        public const byte DEFAULT_STROKE_GREEN = 0;

        public const byte DEFAULT_STROKE_BLUE = 0;

        /// <summary>
        /// Adds a food element to the panel.
        /// </summary>
        /// <returns></returns>
        public void Show(Panel panel)
        {
            // If the element tag matches, then that means an element was created and is being shown somewhere, so it cannot be shown again.
            if (_showing == false)
            {
                _showing = true;
                _parent = panel;

                Rectangle food = new()
                {
                    Fill = new SolidColorBrush(_color),
                    Height = SIZE,
                    Width = SIZE,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    RenderTransform = new TranslateTransform(-30, -30), // Appear out of bounds.
                    Tag = "Viper.Game.Elements.Food",
                };

                _food = food;
                _foodPosition = food.RenderTransform as TranslateTransform;

                panel.Children.Add(_food);

                Reset();
            }
        }

        /// <summary>
        /// Resets a selected food element to a random place.
        /// </summary>
        public void Reset()
        {
            // If the element tag matches, then that means an element was created and is being shown somewhere, so it can be reseted.
            if (_showing == true)
            {
                // Check how big is the space.
                int spaceH = Convert.ToInt32(_parent.Height), spaceW = Convert.ToInt32(_parent.Width);

                CheckMaxUsableSpace();

                void CheckMaxUsableSpace()
                {
                    if (spaceH % SIZE != 0)
                    {
                        spaceH--;
                        CheckMaxUsableSpace();
                    }

                    if (spaceW % SIZE != 0)
                    {
                        spaceW--;
                        CheckMaxUsableSpace();
                    }
                }

                Random rnd = new();

                int newX = rnd.Next(0, spaceW + 1);
                int newY = rnd.Next(0, spaceH + 1);

                AdjustNewPositions();

                void AdjustNewPositions()
                {
                    if (newX % SIZE != 0)
                    {
                        newX--;
                        AdjustNewPositions();
                    }

                    if (newY % SIZE != 0)
                    {
                        newY--;
                        AdjustNewPositions();
                    }
                }

                // Because of aligments, elements can still appear slightly out of bounds, this makes sure that it doesnt happend by avoiding the last possible bottom Y axis and right X axis.
                if (newY == spaceH)
                {
                    newY =- SIZE;
                }
                                        
                if (newX == spaceW)
                {
                    newX =- SIZE;
                }

                TranslateTransform newPos = new(newX, newY);

                _food.RenderTransform = new TranslateTransform(newPos.X, newPos.Y); // Apply new position.
                _foodPosition = newPos; // Add to the list.

                PositionChanged?.Invoke(this, new FoodPositionChangedEventArgs(newPos.X, newPos.Y));
            }
        }

        /// <summary>
        /// Change the color of all food elements.
        /// </summary>
        /// <param name="newColor"></param>
        public void ChangeFoodColor(Color newColor)
        {
            _color = newColor;

            _food.Fill = new SolidColorBrush(_color);

            ColorChanged?.Invoke(this, new FoodColorChangedEventArgs(_color));
        }

        public void Remove()
        {
            // If the element tag matches, then that means an element was created and is being shown somewhere, so we can remove it.
            if (_showing == true)
            {
                _parent.Children.Remove(_food); // Remove element from the panel

                // Restart instances.
                _food = new();
                _foodPosition = new();
                _showing = false;
            }
        }
    }
}
