using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using GreenerConfigurator.ClientCore.Utilities.MUI;

namespace GreenerConfigurator.ViewModels
{
    public class LoginViewModel : ViewModelBase, IDataErrorInfo
    {

        #region [ Constructor(s) ]

        public LoginViewModel()
        {
            OnLoginCommand = new RelayCommand(LoginCommand);
            OnCancelCommand = new RelayCommand(CancelCommand);
        }

        #endregion

        #region [ Public Property(s) ]

        public delegate void LoginSuccessHandler();

        //public event LoginSuccessHandler OnLoginSuccess;

        public ICommand OnLoginCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public string Username
        {
            get => _Username; set
            {
                SetProperty(ref _Username, value);
            }
        }

        public string Password
        {
            get => _Password; set
            {
                SetProperty(ref _Password, value);
            }
        }

        public string Error { get { return _Error; } }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;

                switch (columnName)
                {
                    case nameof(Username):
                        if (string.IsNullOrEmpty(this.Username.Trim()))
                            result = Language.UsernameCouldNotBeEmpty;
                        break;
                }
                _Error = result;
                return result;
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private string _Error = string.Empty;

        private string _Username = "demo";

        private string _Password = string.Empty;

        #endregion

        #region [ Private Method(s) ]

        private void LoginCommand()
        {

        }

        private void CancelCommand()
        {
            App.Current.Shutdown();
        }

        #endregion

    }
}
