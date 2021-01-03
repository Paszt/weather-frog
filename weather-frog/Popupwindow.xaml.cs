using System.Windows;

namespace weatherfrog
{
    /// <summary>
    /// Interaction logic for Popupwindow.xaml
    /// </summary>
    public partial class Popupwindow : Window
    {
        public Popupwindow()
        {
            InitializeComponent();
            DataContext = Application.Current;
            Icon = (System.Windows.Media.ImageSource)Application.Current.FindResource("FrogHeadDrawingImage");
        }
    }
}
