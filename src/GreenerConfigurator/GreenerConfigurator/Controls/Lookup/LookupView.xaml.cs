using System.Windows.Controls;

namespace GreenerConfigurator.Controls.Lookup
{
    /// <summary>
    /// Interaction logic for LookupView.xaml
    /// </summary>
    public partial class LookupView : UserControl
    {
        public LookupView()
        {
            InitializeComponent();
        }

        public void SetViewModel(LookupViewModel lookupViewModel)
        {
            this.DataContext = lookupViewModel;
        }

        private void SelectItemClick(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as LookupViewModel)?.OnSelectCommand.Execute(e.AddedItems);
        }
    }
}
