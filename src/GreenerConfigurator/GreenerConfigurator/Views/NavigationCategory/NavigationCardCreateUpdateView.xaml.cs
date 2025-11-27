using GreenerConfigurator.ViewModels.NavigationCategory;
using System.Windows.Controls;

namespace GreenerConfigurator.Views.NavigationCategory
{
    /// <summary>
    /// Interaction logic for NavigationCardCreateUpdateView.xaml
    /// </summary>
    public partial class NavigationCardCreateUpdateView : UserControl
    {
        public NavigationCardCreateUpdateView()
        {
            InitializeComponent();
        }

        private void LogicalDeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as NavigationCardCreateUpdateViewModel)?.OnSelectLookupDeviceCommand.Execute(e.AddedItems);
        }

        private  void lvLogicalDeviceNavigationM2M_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as NavigationCardCreateUpdateViewModel)?.OnLogicalDeviceNavigationM2MSelectionChangedCommand.Execute(null);
        }

        private void btnChangeDetailNumber_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.DataContext as NavigationCardCreateUpdateViewModel)?.OnExchangeDetailNumberCommand.Execute(null);
            lvLogicalDeviceNavigationM2M.Items.Refresh();
        }

        //private void SelectItemClick(object sender, SelectionChangedEventArgs e)
        //{
        //    (this.DataContext as NavigationCardCreateUpdateViewModel)?.OnSelectDeviceCommand.Execute(e.AddedItems);
        //}
    }
}
