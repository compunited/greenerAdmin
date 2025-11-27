using ClientCoreLanguage = global::GreenerConfigurator.ClientCore.Utilities.MUI.Language;
using GreenerConfigurator.Utilities;
using GreenerConfigurator.ViewModels;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GreenerConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region [ Constructor(s) ]

        public MainWindow()
        {
            InitializeComponent();
            MainWindowModel = new MainWindowModel();
            this.DataContext = MainWindowModel;

            this.Title = ClientCoreLanguage.GreenerConfigurator;
        }

        #endregion

        #region [ Private Field(s) ]

        public MainWindowModel MainWindowModel { get; set; }

        #endregion

        #region [ Private Method(s) ]

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AuthenticationHelper.Init(this);

            var tempToken = await AuthenticationHelper.GetToken();
            if (!string.IsNullOrWhiteSpace(tempToken))
            {
                MainWindowModel.CurrentUserFullName = AuthenticationHelper.CurerntUser.Name;
            }
            else
                Application.Current.Shutdown(99);
        }

        #endregion
    }
}
