using GreenerConfigurator.ViewModels.Navigator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GreenerConfigurator.ViewModels
{
    public class MainWindowModel : ViewModelBase
    {
        #region [ Constructor(s) ] 

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowModel()
        {
            _Navigator = new Navigator.Navigator();
            IsEnable = false;
        }

        #endregion

        #region [ Public Property(s) ]

        public bool IsEnable
        {
            get => _IsEnable;
            set
            {
                _IsEnable = value;
                Navigator.IsEnable = value;
                if (_IsEnable)
                    IsVisible = Visibility.Visible;
                else
                    IsVisible = Visibility.Hidden;

                OnPropertyChanged(nameof(IsEnable));
            }
        }

        public INavigator Navigator
        {
            get => _Navigator;
            set
            {
                _Navigator = value;
            }
        }

        public string CurrentUserFullName
        {
            get => _CurrentUserFullName;
            set
            {
                _CurrentUserFullName = value;
                if (!string.IsNullOrEmpty(_CurrentUserFullName))
                    IsEnable = true;

                OnPropertyChanged(nameof(CurrentUserFullName));
            }
        }

        public Visibility IsVisible
        {
            get => _Isvisible;
            set
            {
                _Isvisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private INavigator _Navigator = null;

        private string _CurrentUserFullName = string.Empty;

        private bool _IsEnable = false;

        private Visibility _Isvisible = Visibility.Hidden;

        #endregion
    }
}
