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

namespace GreenerConfigurator.Views.NetworkDevice
{
    /// <summary>
    /// Interaction logic for NetworkDeviceManagementView.xaml
    /// </summary>
    public partial class NetworkDeviceManagementView : UserControl
    {
        #region [ Constructor(s) ]

        public NetworkDeviceManagementView()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Private Method(s) ]
        
        private void trvLocationDetail_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = e.NewValue as TreeViewItem;

            if (tvi == null || e.Handled) return;

            ((ViewModels.NetworkDevice.NetworkDeviceManagementViewModel)DataContext).SelectedLocationDetail = (global::GreenerConfigurator.ClientCore.Models.LocationDetailModel)tvi.Tag;

            e.Handled = true;
        }

        #endregion
    }
}
