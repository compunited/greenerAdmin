using GreenerConfigurator.Utilities.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.Navigator
{
    public class Navigator : ViewModelBase, INavigator
    {
        #region [ Public Property(s) ]

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _CurrentViewModel;
            }
            set
            {
                _CurrentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public bool IsEnable
        {
            get => _IsEnable;
            set
            {
                _IsEnable = value;
                if (_IsEnable)
                    Isvisible = Visibility.Visible;
                else
                    Isvisible = Visibility.Hidden;

                OnPropertyChanged(nameof(IsEnable));
            }
        }

        public Visibility Isvisible
        {
            get => _Isvisible;
            set
            {
                _Isvisible = value;
                OnPropertyChanged(nameof(Isvisible));
            }
        }

        #endregion

        #region [ Public Method(s) ]

        public ICommand UpdateCurrentViewModelICommand => new UpdateCurrentViewModelICommand(this);

        #endregion

        #region  [ Private Field(s) ]

        private ViewModelBase _CurrentViewModel = null;

        private bool _IsEnable = false;
        private Visibility _Isvisible;

        #endregion
    }
}
