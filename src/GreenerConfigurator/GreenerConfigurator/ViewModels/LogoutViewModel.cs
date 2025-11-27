using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels
{
    public class LogoutViewModel : ViewModelBase
    {
        #region [ Consstructor(s) ]

        public LogoutViewModel()
        {
            OnExitCommand = new RelayCommand(ExitCommand);
            OnLogoutCommand = new RelayCommand(LogoutCommand);
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnExitCommand { get; private set; }

        public ICommand OnLogoutCommand { get; private set; }

        #endregion

        #region [ Private Method(s) ]

        private void ExitCommand()
        { 
            Application.Current.Shutdown(99);
        }

        private async void LogoutCommand()
        {
           var result = await Utilities.AuthenticationHelper.Logout();
            if (result)
                ExitCommand();
        }

        #endregion

    }
}
