using System.Windows;
using Viper.Game;

namespace ViperWindow
{
    public partial class MainWindow : Window
    {
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            ViperGame vg = new();

            MainGrid.Children.Add(vg.Game);
            vg.Start();
        }
    }
}