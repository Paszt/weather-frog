using System;
using System.Collections.Generic;
using System.Linq;
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
using weatherfrog.Extensions;

namespace weatherfrog.Illustrations
{
    /// <summary>
    /// Interaction logic for IllustrationWindow.xaml
    /// </summary>
    public partial class IllustrationWindow : Window
    {
        public IllustrationWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetPlacement(My.Settings.IllustrationWindowPlacement);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            My.Settings.IllustrationWindowPlacement = this.GetPlacement();
            My.Settings.Save();
        }
    }
}
