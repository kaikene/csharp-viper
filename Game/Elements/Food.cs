using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Windows.Media.Color;
using System.Diagnostics;
using Viper.Game.Events;

namespace Viper.Game.Elements
{
    /// <summary>
    /// Add, remove and manage food elements.
    /// </summary>
    public class Food
    {
        /// <summary>
        /// Triggers when the color of the food changed.
        /// </summary>
        public event EventHandler<FoodColorChangedEventArgs> ColorChanged;

        /// <summary>
        /// Triggers when the amount of food changes.
        /// </summary>
        public event EventHandler<FoodAmountChangedEventArgs> FoodAmountChanged;

        /// <summary>
        /// Triggers when the position of one of the foods changes.
        /// </summary>
        public event EventHandler<FoodPositionChangedEventArgs> FoodPositionsChanged;

        /// <summary>
        /// Used with "FoodAmountChanged" to determine which action hass been done.
        /// </summary>
        public enum FoodAction
        {
            Remove,
            Add,
            /// <summary>
            /// Used as a "nothing has been done" to call the event when no action have been performed, you use it to basically just check what's up :)
            /// </summary>
            Check,
        }

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
        private List<TranslateTransform> _foodPositions = new();

        // Amount of food counted.
        private int _foodCounter = 0;

        // A list for all food elements saved.
        private List<Rectangle> _foods = new();

        // Constant size of the food elements
        public const int SIZE = 10;

        /// <summary>
        /// Gets the current amount of food being shown.
        /// </summary>
        public int FoodAmount
        {
            get
            {
                return _foods.Count();
            }
        }

        /// <summary>
        /// Adds a food element to the panel.
        /// </summary>
        /// <returns></returns>
        public Rectangle AddFood()
        {
            // Takes the current counter value as the index for the food, mostly so the _foodCounter variable can be incremented or decreased without
            // affecting the index of the foodwe want to control
            int currentIndex = _foodCounter;

            Rectangle food = new()
            {
                Fill = new SolidColorBrush(_color),
                Height = SIZE,
                Width = SIZE,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                RenderTransform = new TranslateTransform(-30, -30), // Appear out of bounds.
            };

            _foods.Add(food);
            _foodPositions.Add(food.RenderTransform as TranslateTransform);

            food.Loaded += (s, e) =>
            {
                // When the food loads, check how big is the space.
                int spaceW = Convert.ToInt32((_foods[0].Parent as Panel).Height), spaceH = Convert.ToInt32((_foods[0].Parent as Panel).Width);

                // Move the food element to a visible position depending on the size of the panel we are in.
                // If the amount of spaces is equal to the amount of food being shown, then that means theres no space left.
                if ((spaceW / SIZE) * (spaceH / SIZE) != _foods.Count)
                {
                    _foodCounter += 1;

                    FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Add));

                    RePosition(currentIndex);
                }
                else // Remove the extra food element from the list and from the window
                {
                    _foods.Remove(food);
                    _foodPositions.Remove(food.RenderTransform as TranslateTransform);
                    (_foods[0].Parent as Panel).Children.Remove(food);
                }
            };

            return _foods[currentIndex];
        }

        /// <summary>
        /// Remoes the last food element from the panel.
        /// </summary>
        public void RemoveFood()
        {
            if (_foodPositions.Count > 0) // Only do it when we have more than 0 elements.
            {
                int currentIndex = _foodCounter - 1;

                (_foods[currentIndex].Parent as Panel).Children.Remove(_foods[currentIndex]); // Remove element from the panel

                // Remove from lists
                _foods.RemoveAt(currentIndex);
                _foodPositions.RemoveAt(currentIndex);

                _foodCounter -= 1;

                FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Remove));
            }
        }

        /// <summary>
        /// Repositions a selected food element to a random place.
        /// </summary>
        /// <param name="foodIndex"></param>
        public void RePosition(int foodIndex)
        {
            if (_foodPositions.Count > 0)
            {
                // Check how big is the space.
                int spaceH = Convert.ToInt32((_foods[0].Parent as Panel).Height), spaceW = Convert.ToInt32((_foods[0].Parent as Panel).Width);

                TranslateTransform newPos = new();

                VerifyPositioning();

                void VerifyPositioning()
                {
                    Random random = new Random();

                    // Select a new position depending on the size of the panel we are in.
                    newPos = new(SIZE * random.Next(0, spaceW / SIZE), SIZE * random.Next(0, spaceH / SIZE));

                    for (int i = 0; i < _foodPositions.Count; i++)
                    {
                        // Check if the new position is equal to a position from the food positions list
                        if (newPos.X == _foodPositions[i].X && newPos.Y == _foodPositions[i].Y)
                        {
                            // If the anwser is yes, then that means this postion is already in use, so search a new one.
                            VerifyPositioning();
                        }
                    }
                }
                _foods[foodIndex].RenderTransform = new TranslateTransform(newPos.X, newPos.Y); // Apply new position.
                _foodPositions[foodIndex] = newPos; // Add to the list.

                FoodPositionsChanged?.Invoke(this, new FoodPositionChangedEventArgs(foodIndex, newPos.X, newPos.Y));
            }
        }   

        /// <summary>
        /// Gets the position from a selected food element.
        /// </summary>
        /// <param name="foodIndex"></param>
        /// <returns></returns>
        public TranslateTransform GetFoodPosition(int foodIndex)
        {
            return _foodPositions[foodIndex];
        }

        /// <summary>
        /// Change the color of all food elements.
        /// </summary>
        /// <param name="newColor"></param>
        public void ChangeFoodColor(Color newColor)
        {
            _color = newColor;

            foreach (var food in _foods)
            {
                food.Fill = new SolidColorBrush(_color);
            }
            ColorChanged?.Invoke(this, new FoodColorChangedEventArgs(_color));
        }

        public void CleanUp()
        {
            try
            {
                Panel parent = _foods[0].Parent as Panel;

                foreach (Rectangle food in _foods)
                {
                    parent.Children.Remove(food);
                }
            }
            catch
            {
                // Food is not loaded, nothing to remove.
            }

            _foods.Clear();
            _foodPositions.Clear();
            _foodCounter = 0;

            FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Check));
        }

        private void RaiseAllEvents()
        {
            if (CanTriggerValueEvents)
            {
                FoodAmountChanged?.Invoke(this, new FoodAmountChangedEventArgs(_foodCounter, FoodAction.Check));
                ColorChanged?.Invoke(this, new FoodColorChangedEventArgs(_color));
                FoodPositionsChanged?.Invoke(this, new FoodPositionChangedEventArgs(0, 0, 0));
            }
        }
    }
}
