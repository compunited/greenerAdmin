using global::GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ViewModels.Navigator;
using System.Windows;
using System.Windows.Controls;

namespace GreenerConfigurator.Controls
{
    /// <summary>
    /// Interaction logic for SideBarNavigation.xaml
    /// </summary>
    public partial class SideBarNavigation : UserControl
    {

        #region [ Constructor(s) ]

        public SideBarNavigation()
        {
            InitializeComponent();
        }

        #endregion

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            (((sender as Button).Content as StackPanel).Children[0] as Image).ContextMenu.IsOpen = true;
        }

        private void ImportSensors_OnClick(object sender, RoutedEventArgs e)
        {
            var navigator = this.DataContext as INavigator;
            navigator.UpdateCurrentViewModelICommand.Execute(ViewModelType.SensorImport);
        }

        private void ConfigureManyally_OnClick(object sender, RoutedEventArgs e)
        {
            var navigator = this.DataContext as INavigator;
            navigator.UpdateCurrentViewModelICommand.Execute(ViewModelType.SensorImportManually);
        }

        private void ConfigureGateway_OnClick(object sender, RoutedEventArgs e)
        {
            var navigator = this.DataContext as INavigator;
            navigator.UpdateCurrentViewModelICommand.Execute(ViewModelType.ConfigureGateway);
        }

        private void ConnectionView_OnClick(object sender, RoutedEventArgs e)
        {
            var navigator = this.DataContext as INavigator;
            navigator.UpdateCurrentViewModelICommand.Execute(ViewModelType.ConnectionView);
        }
    }
}
