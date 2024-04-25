using System.Windows;
using Viper.Game;

namespace Viper
{
    public partial class MainWindow : Window
    {
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViperGame viperGame = new(window);

            MainGrid.Children.Add(viperGame.Start());
        }
    }
}