using Greener.Web.Definitions.API.Network.BusConnection.Mqtt;
using GreenerConfigurator.ClientCore.Models.Network;
using GreenerConfigurator.ClientCore.Models.Network.BusConnection;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.NetworkDevice.BusConnection
{
    public class MqttBusConnectionCreateUpdateViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly BusConnectionService _busConnectionService;

        public MqttBusConnectionCreateUpdateViewModel(PageStatus pageStatus, MqttBusConnectionEditModel mqttBusConnectionEditModel)
        {
            _busConnectionService = App.ServiceProvider.GetRequiredService<BusConnectionService>();
            PageStatus = pageStatus;
            MqttBusConnectionEditModel = mqttBusConnectionEditModel;

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);
        }

        public MqttBusConnectionCreateUpdateViewModel(PageStatus pageStatus, MqttBusConnectionEditDto mqttBusConnectionEditDto)
        {
            _busConnectionService = App.ServiceProvider.GetRequiredService<BusConnectionService>();
            PageStatus = pageStatus;
            MqttBusConnectionEditModel = ConvertDtoToModel(mqttBusConnectionEditDto);

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnOKCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public MqttBusConnectionEditModel MqttBusConnectionEditModel
        {
            get => _MqttBusConnectionEditModel;
            set
            {
                _MqttBusConnectionEditModel = value;
                OnPropertyChanged(nameof(MqttBusConnectionEditModel));
            }
        }

        public PageStatus PageStatus
        {
            get => _PageStatus;
            set
            {
                _PageStatus = value;
                OnPropertyChanged(nameof(PageStatus));
                ManagePageStatusAsync();
            }
        }

        public string ButtonOkText
        {
            get => _ButtonOkText;
            set
            {
                _ButtonOkText = value;
                OnPropertyChanged(nameof(ButtonOkText));
            }
        }

        public bool IsEditMode
        {
            get => _IsEditMode;
            set
            {
                _IsEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private MqttBusConnectionEditModel _MqttBusConnectionEditModel = null;
        private PageStatus _PageStatus;
        private string _ButtonOkText;
        private bool _IsEditMode = false;

        #endregion

        #region [ Private Method(s) ]

        private async Task ManagePageStatusAsync()
        {
            switch (_PageStatus)
            {
                case PageStatus.Add:
                case PageStatus.Edit:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = Language.Delete;
                        IsEditMode = false;
                    }
                    break;
            }
        }

        private async Task OkCommandAsync()
        {
            bool changeIsDone = false;
            if (PageStatus != PageStatus.Delete)
            {
                if (_MqttBusConnectionEditModel.IsValid)
                {
                    MqttBusConnectionEditModel tempResult = null;
                    if (PageStatus == PageStatus.Add)
                    {
                        tempResult = await _busConnectionService.AddBusConnectionAsync(_MqttBusConnectionEditModel);
                    }
                    else
                        tempResult = await _busConnectionService.EditBusConnectionAsync(_MqttBusConnectionEditModel);

                    if (tempResult != null)
                        changeIsDone = true;
                }
            }
            else
            {
                //await Services.BusConnectionService.RemoveBusConnectionAsync(_BusConnectionEditModel);
                changeIsDone = true;
            }

            if (changeIsDone)
            {
                NetworkDeviceEditModel tempNetworkDevice = new NetworkDeviceEditModel();
                tempNetworkDevice.Id = _MqttBusConnectionEditModel.NetworkDeviceId;

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NetworkDeviceCreateUpdateViewModel(tempNetworkDevice, PageStatus.Edit);
            }
        }

        private async Task CancelCommandAsync()
        {
            NetworkDeviceEditModel tempNetworkDevice = new NetworkDeviceEditModel();
            tempNetworkDevice.Id = _MqttBusConnectionEditModel.NetworkDeviceId;

            var temp = Application.Current;
            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NetworkDeviceCreateUpdateViewModel(tempNetworkDevice, PageStatus.Edit);
        }

        private MqttBusConnectionEditModel ConvertDtoToModel(MqttBusConnectionEditDto mqttBusConnectionEditdto)
        {
            MqttBusConnectionEditModel result = new MqttBusConnectionEditModel();

            result.Address = mqttBusConnectionEditdto.Address;
            result.BusInterfaceId = mqttBusConnectionEditdto.BusInterfaceId;
            result.CertificateId = mqttBusConnectionEditdto.CertificateId;
            result.ChangeStateDate = mqttBusConnectionEditdto.ChangeStateDate; ;
            result.ClientId = mqttBusConnectionEditdto.ClientId;
            result.Description = mqttBusConnectionEditdto.Description;
            result.Name = mqttBusConnectionEditdto.Name;
            result.Password = mqttBusConnectionEditdto.Password;
            result.Port = mqttBusConnectionEditdto.Port;
            result.State = mqttBusConnectionEditdto.State;
            result.Username = mqttBusConnectionEditdto.Username;
            result.UseSSL = mqttBusConnectionEditdto.UseSSL;
            result.VersionId = mqttBusConnectionEditdto.VersionId;
            result.NetworkDeviceId = mqttBusConnectionEditdto.NetworkDeviceId;

            return result;
        }

        #endregion

    }
}
