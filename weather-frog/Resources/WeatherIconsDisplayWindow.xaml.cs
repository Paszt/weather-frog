using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace weatherfrog.Resources
{
    /// <summary>
    /// Interaction logic for WeatherIconsDisplayWindow.xaml
    /// </summary>
    public partial class WeatherIconsDisplayWindow : Window
    {
        private static WeatherIconsDisplayWindow instance;
        public static WeatherIconsDisplayWindow Instance
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

        public WeatherIconsDisplayWindow() => InitializeComponent();
    }
}
