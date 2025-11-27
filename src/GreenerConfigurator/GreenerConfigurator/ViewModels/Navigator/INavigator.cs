using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.Navigator
{
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }

        ICommand UpdateCurrentViewModelICommand { get; }

        bool IsEnable { get; set; }

    }
}
