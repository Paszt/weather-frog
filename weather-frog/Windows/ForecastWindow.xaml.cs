using System.Reflection;
using System.Windows;
using weatherfrog.Extensions;

namespace weatherfrog
{
    /// <summary>
    /// Interaction logic for ForecastWindow.xaml
    /// </summary>
    public partial class ForecastWindow : Window
    {
        private static ForecastWindow instance;
        public static ForecastWindow Instance
        {
            get
            {
                if (instance == null || (bool)typeof(Window).GetProperty("IsDisposed", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance))
                    return instance = new();
                if (instance.IsLoaded)
                {
                    instance.Activate();
                    return null;
                }
                return instance;
            }
        }

        public ForecastWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e) => this.SetPlacement(My.Settings.ForecastWindowPlacement);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            My.Settings.ForecastWindowPlacement = this.GetPlacement();
            My.Settings.Save();
        }
    }
}
