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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace weatherfrog.Resources
{
    /// <summary>
    /// Interaction logic for TaskbarBalloon.xaml
    /// </summary>
    public partial class TaskbarBalloon : UserControl
    {
        private readonly TranslateTransform GraphTranslateTransform;

        public TaskbarBalloon()
        {
            InitializeComponent();
            GraphTranslateTransform = new() { Y = 150 };
            HourlyGraph.RenderTransform = GraphTranslateTransform;
            HourlyGraph.Opacity = 0.0;
        }

        private void RootGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double ToYValue = 0.0;
            double ToOpacityValue = 1.0;
            if (GraphTranslateTransform.Y < 80)
            {
                ToYValue = 150.0;
                ToOpacityValue = 0.0;
            }
            DoubleAnimation GraphSlideAnimation = new(ToYValue, TimeSpan.FromSeconds(0.4))
            { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn } };
            DoubleAnimation GraphOpacityAnimation = new(ToOpacityValue, TimeSpan.FromSeconds(0.4))
            { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn } };
            GraphTranslateTransform.BeginAnimation(TranslateTransform.YProperty, GraphSlideAnimation);
            HourlyGraph.BeginAnimation(OpacityProperty, GraphOpacityAnimation);
        }
    }
}
