using global::GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ViewModels;
using GreenerConfigurator.ViewModels.Location;
using GreenerConfigurator.ViewModels.NavigationCategory;
using GreenerConfigurator.ViewModels.Navigator;
using GreenerConfigurator.ViewModels.RuleSetting;
using GreenerConfigurator.ViewModels.NetworkDevice;
using System;
using System.Windows.Input;
using GreenerConfigurator.ViewModels.Rule;
using GreenerConfigurator.ViewModels.Sensor;
using GreenerConfigurator.ViewModels.PlanView;

namespace GreenerConfigurator.Utilities.Commands
{
    class UpdateCurrentViewModelICommand : ICommand
    {
        #region [ Constructor(s) ]

        public UpdateCurrentViewModelICommand(INavigator Navigator)
        {
            _Navigator = Navigator;
        }

        #endregion

        #region [ Public Method(s) ]

        #region [ Event(s) ]

        public event EventHandler CanExecuteChanged;

        #endregion

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewModelType)
            {
                var tempViewModelType = (ViewModelType)parameter;
                switch (tempViewModelType)
                {
                    case ViewModelType.Dashboard:
                        break;
                    case ViewModelType.Login:
                        break;
                    case ViewModelType.Location:
                        _Navigator.CurrentViewModel = new LocationManagementViewModel();
                        break;
                    case ViewModelType.Gateway:
                        _Navigator.CurrentViewModel = new NetworkDeviceManagementViewModel();
                        break;
                    case ViewModelType.Sensor:
                        break;
                    case ViewModelType.SensorImport:
                        _Navigator.CurrentViewModel = new SensorImportViewModel();
                        break;
                    case ViewModelType.SensorImportManually:
                        break;
                    case ViewModelType.Setting:
                        _Navigator.CurrentViewModel = new NewRuleSettingViewModel();
                        break;
                    case ViewModelType.SettingHeader:
                        _Navigator.CurrentViewModel = new RuleManagementViewModel();
                        break;
                    case ViewModelType.NavigationCategory:
                        _Navigator.CurrentViewModel = new NavigationCategoryManagementViewModel();
                        break;
                    case ViewModelType.PlaceElement:
                        _Navigator.CurrentViewModel = new PlanViewManagementViewModel();
                        break;
                    case ViewModelType.Logout:
                        _Navigator.CurrentViewModel = new LogoutViewModel();
                        break;
                }
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private INavigator _Navigator;

        #endregion

    }
}
