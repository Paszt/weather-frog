using System.Reflection;
using System.Windows;

namespace weatherfrog
{
    /// <summary>
    /// Interaction logic for Popupwindow.xaml
    /// </summary>
    public partial class Popupwindow : Window
    {
        private static Popupwindow instance;
        public static Popupwindow Instance
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

        public Popupwindow()
        {
            InitializeComponent();
            DataContext = Application.Current;
            Icon = (System.Windows.Media.ImageSource)Application.Current.FindResource("FrogHeadDrawingImage");
        }
    }
}
