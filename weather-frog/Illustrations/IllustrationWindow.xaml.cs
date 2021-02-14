using System.Windows;
using weatherfrog.Extensions;

namespace weatherfrog.Illustrations
{
    /// <summary>
    /// Interaction logic for IllustrationWindow.xaml
    /// </summary>
    public partial class IllustrationWindow : Window
    {
        public IllustrationWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e) => this.SetPlacement(My.Settings.IllustrationWindowPlacement);

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is IllustrationViewModel ivm)
            {
                if (!await ivm.HandleIsDirty()) e.Cancel = true;
            }
            My.Settings.IllustrationWindowPlacement = this.GetPlacement();
            My.Settings.Save();
        }
    }
}
