using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using Greener.Web.Definitions.Enums;

namespace GreenerConfigurator.Views.Location
{
  /// <summary>
  /// Interaction logic for LocationManagementView.xaml
  /// </summary>
  public partial class LocationManagementView : UserControl
    {
        public LocationManagementView()
        {
            InitializeComponent();
        }

        private void TexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Int32 selectionStart = textBox.SelectionStart;
            Int32 selectionLength = textBox.SelectionLength;
            String newText = String.Empty;
            int count = 0;
            foreach (Char c in textBox.Text.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count == 0))
                {
                    newText += c;
                    if (c == '.')
                        count += 1;
                }
            }
            textBox.Text = newText;
            textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
        }

        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = e.NewValue as TreeViewItem;
           
            if (tvi == null || e.Handled) return;

            ((ViewModels.Location.LocationManagementViewModel)DataContext).SelectedLocationDetail = (global::GreenerConfigurator.ClientCore.Models.LocationDetailModel)tvi.Tag;

            e.Handled = true;
        }
    }
}
