using GreenerConfigurator.ViewModels.PlanView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenerConfigurator.Views.PlanView
{
    /// <summary>
    /// Interaction logic for PlaceElementView.xaml
    /// </summary>
    public partial class PlanViewView : UserControl
    {
        public PlanViewView()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var tempCoursePoint = e.GetPosition(testImage);
            
            ((PlanViewViewModel)DataContext).ManageMouseClickOnImage((float)tempCoursePoint.X,(float)tempCoursePoint.Y);
        }
    }
}
