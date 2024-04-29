using System.Windows;
using Viper.Game;

namespace ViperWindow
{
    public partial class MainWindow : Window
    {
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            ViperGame viperGame = new();

            MainGrid.Children.Add(viperGame.Start());
        }
    }
}