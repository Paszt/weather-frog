using System.Windows;
using weatherfrog.Extensions;

namespace weatherfrog.Illustrations
{
    /// <summary>
    /// Interaction logic for FilenameEditorWindow.xaml
    /// </summary>
    public partial class FilenameEditorWindow : Window
    {
        public FilenameEditorWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e) => 
            this.SetPlacement(My.Settings.FileNameEditorWindowPlacement);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            My.Settings.FileNameEditorWindowPlacement = this.GetPlacement();
            My.Settings.Save();
        }

    }
}
