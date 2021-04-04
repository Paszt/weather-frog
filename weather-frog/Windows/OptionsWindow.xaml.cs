using System.Reflection;
using System.Windows;
using weatherfrog.Extensions;
using weatherfrog.Infrastructure;

namespace weatherfrog
{
    public partial class OptionsWindow : Window
    {
        private static OptionsWindow instance;
        public static OptionsWindow Instance
        {
            get
            {
                if (instance == null || (bool)typeof(Window).GetProperty("IsDisposed",
                    BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance))
                    return instance = new();

                if (instance.IsLoaded)
                {
                    instance.Activate();
                    return null;
                }
                return instance;
            }
        }

        public OptionsWindow() => InitializeComponent();

        private void SaveButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;

        private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;

        private void Window_Loaded(object sender, RoutedEventArgs e) => this.SetPlacement(My.Settings.OptionsWindowPlacement);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            My.Settings.OptionsWindowPlacement = this.GetPlacement();
            My.Settings.Save();
        }
    }
}
