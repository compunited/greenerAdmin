using GreenerConfigurator.ClientCore.Models.Network;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.NetworkDevice
{
    public class NetworkDeviceUsageInfoViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        public NetworkDeviceUsageInfoViewModel(NetworkDeviceViewModel networkDeviceModel)
        {
            NetworkDeviceModel = networkDeviceModel;

            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnCancelCommand { get;private set; }

        public NetworkDeviceViewModel NetworkDeviceModel
        {
            get => _NetworkDeviceModel;
            set
            {
                _NetworkDeviceModel = value;
                OnPropertyChanged(nameof(NetworkDeviceModel));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private NetworkDeviceViewModel _NetworkDeviceModel = null;


        #endregion

        #region [ Private Method(s) ]

        private async Task CancelCommandAsync()
        {
            
        }

        #endregion
    }
}
