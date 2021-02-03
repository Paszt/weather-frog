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
            { EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut } };
            DoubleAnimation GraphOpacityAnimation = new(ToOpacityValue, TimeSpan.FromSeconds(0.4))
            { EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut } };
            GraphTranslateTransform.BeginAnimation(TranslateTransform.YProperty, GraphSlideAnimation);
            HourlyGraph.BeginAnimation(OpacityProperty, GraphOpacityAnimation);
        }

        private Point startDrag;
        private bool IsTrackingYDelta;

        private void HourlyGraph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startDrag = e.GetPosition(this);
            IsTrackingYDelta = true;
        }

        private void HourlyGraph_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => IsTrackingYDelta = false;

        private void HourlyGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsTrackingYDelta)
            {
                Vector v = startDrag - e.GetPosition(this);
                // If the user drags the graph more than 20 down, but not more than 20 left or right, close the graph.
                if (Math.Abs(v.X) < 20 && v.Y < -20) { RootGrid_MouseLeftButtonUp(null, null); IsTrackingYDelta = false; }
                // A drag to the left or right more than 20 means the user is sliding the graph left/right, 
                // so stop tracking Y change.
                if (Math.Abs(v.X) > 20) { IsTrackingYDelta = false; }
            }
        }
    }
}
