using GreenerConfigurator.ViewModels.Rule;
using System;
using System.Collections.Generic;
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

namespace GreenerConfigurator.Views.Rule
{
    /// <summary>
    /// Interaction logic for CompareConditionCreateUpdateView.xaml
    /// </summary>
    public partial class CompareConditionCreateUpdateView : UserControl
    {
        public CompareConditionCreateUpdateView()
        {
            InitializeComponent();
        }

        private void trvLoationDetail_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = e.NewValue as TreeViewItem;

            if (tvi == null || e.Handled) return;

            ((CompareConditionCreateUpdateViewModel)DataContext).SelectedLocationDetail = (global::GreenerConfigurator.ClientCore.Models.LocationDetailModel)tvi.Tag;

            e.Handled = true;
        }
    }
}
