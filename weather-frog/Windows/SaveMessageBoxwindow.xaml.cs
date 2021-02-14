using System.Windows;

namespace weatherfrog.Windows
{
    /// <summary>
    /// Interaction logic for SaveMessageBox.xaml
    /// </summary>
    internal partial class SaveMessageBoxWindow : Window
    {
        public MessageBoxResult Result { get; set; }

        internal string FileName { set => TextBlock_Message.Text = "Do you want to save changes to " + value + "?"; }

        public SaveMessageBoxWindow(string fileName)
        {
            InitializeComponent();
            FileName = fileName;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void Button_DontSave_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
