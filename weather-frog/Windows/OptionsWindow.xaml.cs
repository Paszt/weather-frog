using System.Windows;
using weatherfrog.Extensions;

namespace weatherfrog
{
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetPlacement(My.Settings.OptionsWindowPlacement);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            My.Settings.OptionsWindowPlacement = this.GetPlacement();
            My.Settings.Save();
        }
    }
}
