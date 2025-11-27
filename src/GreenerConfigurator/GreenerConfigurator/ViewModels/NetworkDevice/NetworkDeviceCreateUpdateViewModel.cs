using Greener.Web.Definitions.API.Network.BusConnection;
using Greener.Web.Definitions.API.Network.BusConnection.Mqtt;
using Greener.Web.Definitions.Enums.Networks;
using GreenerConfigurator.ClientCore.Models.Network;
using GreenerConfigurator.ClientCore.Models.Network.BusConnection;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using GreenerConfigurator.ViewModels.NetworkDevice.BusConnection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.NetworkDevice
{
    public class NetworkDeviceCreateUpdateViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly NetworkDeviceService _networkDeviceService;

        public NetworkDeviceCreateUpdateViewModel(NetworkDeviceEditModel networkDeviceModel, PageStatus pageStatus)
        {
            _networkDeviceService = App.ServiceProvider.GetRequiredService<NetworkDeviceService>();
            NetworkDevice = networkDeviceModel;
            PageStatus = pageStatus;

            OnOKCommand = new AsyncRelayCommand(OKCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);

            OnAddBusConnectionCommand = new AsyncRelayCommand(AddBusConnectionAsync);
            OnEditBusConnectionCommand = new AsyncRelayCommand(EditBusConnectionAsync);
            OnDeleteBusConnectionCommand = new AsyncRelayCommand(DeleteBusConnectionAsync);

        }

        #endregion

        #region [ Poublic Method(s) ]

        public ICommand OnOKCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnAddBusConnectionCommand { get; set; }

        public ICommand OnEditBusConnectionCommand { get; set; }

        public ICommand OnDeleteBusConnectionCommand { get; set; }


        public NetworkDeviceEditModel NetworkDevice
        {
            get => _NetworkDevice; set
            {
                _NetworkDevice = value;
                OnPropertyChanged(nameof(NetworkDevice));
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

        public Visibility BusConnectionListVisibility
        {
            get => _BusConnectionListVisibility;
            set
            {
                _BusConnectionListVisibility = value;
                OnPropertyChanged(nameof(BusConnectionListVisibility));
                if (_BusConnectionListVisibility == Visibility.Collapsed)
                    SelectBusConnectionTypeVisibility = Visibility.Visible;
                else
                    SelectBusConnectionTypeVisibility = Visibility.Collapsed;
            }
        }

        public Visibility SelectBusConnectionTypeVisibility
        {
            get => _SelectBusConnectionTypeVisibility;
            set
            {
                _SelectBusConnectionTypeVisibility = value;
                OnPropertyChanged(nameof(SelectBusConnectionTypeVisibility));
            }
        }

        public ObservableCollection<BusConnectionEditModel> BusConnectionList
        {
            get => _BusConnectionList;
            set
            {
                _BusConnectionList = value;
                OnPropertyChanged(nameof(BusConnectionList));
                if (_BusConnectionList != null)
                    SelectedBusConnection = _BusConnectionList.FirstOrDefault();
            }
        }

        public BusConnectionEditModel SelectedBusConnection
        {
            get => _SelectedBusConnection;
            set
            {
                _SelectedBusConnection = value;
                OnPropertyChanged(nameof(SelectedBusConnection));
            }
        }

        public BusInterfaceConnectionType SelectedBusInterfaceConnectionType
        {
            get => _SelectedBusInterfaceConnectionType;
            set
            {
                _SelectedBusInterfaceConnectionType = value;
                // ManageAddBusConnectionAsync();
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private NetworkDeviceEditModel _NetworkDevice = null;
        private PageStatus _PageStatus;
        private string _ButtonOkText = string.Empty;
        private bool _IsEditMode = false;
        private Visibility _BusConnectionListVisibility = Visibility.Visible;
        private Visibility _SelectBusConnectionTypeVisibility = Visibility.Collapsed;
        private ObservableCollection<BusConnectionEditModel> _BusConnectionList = null;
        private BusConnectionEditModel _SelectedBusConnection = null;
        private BusInterfaceConnectionType _SelectedBusInterfaceConnectionType;

        #endregion

        #region [ Private Method(s) ]

        private async Task LoadNetworkDeviceData()
        {
            NetworkDevice = await _networkDeviceService.GetNetworkDeviceEditModelByNetworkDevideIdAsync(_NetworkDevice.Id);
            //ManageBusConnectionDataAsync();
        }

        //private async Task ManageBusConnectionDataAsync()
        //{
        //    List<BusConnectionEditModel> tempBusConnectionList = null;

        //    if (_NetworkDevice.BusConnections != null)
        //    {
        //        var query = from busConnection in _NetworkDevice.BusConnections
        //                    select new BusConnectionEditModel(busConnection.BusInterfaceConnectionType)
        //                    {
        //                        Name = busConnection.Name,
        //                        BusInterfaceId = busConnection.BusInterfaceId
        //                    };

        //        tempBusConnectionList = query.ToList();
        //    }
        //    else
        //        tempBusConnectionList = new List<BusConnectionEditModel>();

        //    BusConnectionList = new ObservableCollection<BusConnectionEditModel>(tempBusConnectionList);
        //}

        private async Task ManagePageStatusAsync()
        {
            switch (_PageStatus)
            {
                case PageStatus.Add:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                    }
                    break;
                case PageStatus.Edit:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                        LoadNetworkDeviceData();
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = Language.Delete;
                        IsEditMode = false;
                        LoadNetworkDeviceData();
                    }
                    break;
            }
        }

        private async Task CancelCommandAsync()
        {
            var temp = Application.Current;
            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NetworkDeviceManagementViewModel();
        }

        private async Task OKCommandAsync()
        {
            if (Validate())
            {
                NetworkDeviceEditModel tempResult = null;

                switch (PageStatus)
                {
                    case PageStatus.Add:
                        tempResult = await _networkDeviceService.AddNetworkDeviceAsync(_NetworkDevice);
                        break;
                    case PageStatus.Edit:
                        tempResult = await _networkDeviceService.EditNetworkDeviceAsync(_NetworkDevice);
                        break;
                    case PageStatus.Delete:
                        await _networkDeviceService.RemoveNetworkDeviceAsync(_NetworkDevice.Id);
                        break;
                    case PageStatus.Idle:
                    default:
                        break;
                }

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NetworkDeviceManagementViewModel();
            }
        }

        private async Task AddBusConnectionAsync()
        {
            BusConnectionListVisibility = Visibility.Collapsed;
        }

        private async Task EditBusConnectionAsync()
        {
            //TODO: should be updated with the correct model

            //if (_SelectedBusConnection != null)
            //{
            //    var tempItem = _NetworkDevice.BusConnections.Where(w => w.BusInterfaceId == _SelectedBusConnection.BusInterfaceId).SingleOrDefault();
            //    if (tempItem != null)
            //    {
            //        ManageEditBusConnectionAsync(tempItem);
            //        //var tempModel = (MqttBusConnectionEditModel)busConnectionBaseEditDto;
            //    }
            //}
        }


        private async Task DeleteBusConnectionAsync()
        {

        }

        //private async Task ManageAddBusConnectionAsync()
        //{
        //    switch (_SelectedBusInterfaceConnectionType)
        //    {
        //        case BusInterfaceConnectionType.Bluetooth:
        //            break;
        //        case BusInterfaceConnectionType.WiFi:
        //            break;
        //        case BusInterfaceConnectionType.Lan:
        //            break;
        //        case BusInterfaceConnectionType.LTE:
        //            break;
        //        case BusInterfaceConnectionType.LoraWan:
        //            break;
        //        case BusInterfaceConnectionType.WMBus:
        //            break;
        //        case BusInterfaceConnectionType.ModBus:
        //            break;
        //        case BusInterfaceConnectionType.MBus:
        //            break;
        //        case BusInterfaceConnectionType.MQTT:
        //            {
        //                MqttBusConnectionEditModel model = new MqttBusConnectionEditModel();
        //                model.NetworkDeviceId = _NetworkDevice.Id;
        //                model.BusInterfaceId = Guid.NewGuid();

        //                MqttBusConnectionCreateUpdateViewModel mqttViewModel = new MqttBusConnectionCreateUpdateViewModel(
        //                                                                                PageStatus.Add, model);
        //                var temp = Application.Current;
        //                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = mqttViewModel;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}


        //private void ManageEditBusConnectionAsync(BusConnectionBaseEditDto busConnectionBaseEditDto)
        //{
        //    switch (busConnectionBaseEditDto.BusInterfaceConnectionType)
        //    {
        //        case BusInterfaceConnectionType.Bluetooth:
        //            break;
        //        case BusInterfaceConnectionType.WiFi:
        //            break;
        //        case BusInterfaceConnectionType.Lan:
        //            break;
        //        case BusInterfaceConnectionType.LTE:
        //            break;
        //        case BusInterfaceConnectionType.LoraWan:
        //            break;
        //        case BusInterfaceConnectionType.WMBus:
        //            break;
        //        case BusInterfaceConnectionType.ModBus:
        //            break;
        //        case BusInterfaceConnectionType.MBus:
        //            break;
        //        case BusInterfaceConnectionType.MQTT:
        //            {
        //                var tempModel = (MqttBusConnectionEditDto)busConnectionBaseEditDto;
        //                MqttBusConnectionCreateUpdateViewModel mqttViewModel = new MqttBusConnectionCreateUpdateViewModel(PageStatus.Edit, tempModel);

        //                var temp = Application.Current;
        //                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = mqttViewModel;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private bool Validate()
        {
            bool result = true;

            if (string.IsNullOrEmpty(NetworkDevice.Name))
                result = false;

            return result;
        }


        #region Should move to lte busconnection

        //private ObservableCollection<SimCardEditModel> _SimCardList = null;
        //private SimCardEditModel _SelectedSimCard = null;
        //private bool _IsAssginSimcardAvailable = false;
        //private Visibility _SimCardListVisibility = Visibility.Visible;
        //private Visibility _SimCardDataEntryVisibility = Visibility.Collapsed;
        //private SimCardEditModel _SimCardDataEntry = null;
        //private string _ButtonOkSimCardText = string.Empty;
        //private PageStatus simCardDataEntryPageStatus;
        //private bool _SimCardDataEntryEnalble = true;

        //public bool IsAssginSimcardAvailable
        //{
        //    get => _IsAssginSimcardAvailable;
        //    set
        //    {
        //        _IsAssginSimcardAvailable = value;
        //        OnPropertyChanged(nameof(IsAssginSimcardAvailable));
        //    }
        //}

        //public Visibility SimCardListVisibility
        //{
        //    get => _SimCardListVisibility;
        //    set
        //    {
        //        _SimCardListVisibility = value;
        //        OnPropertyChanged(nameof(SimCardListVisibility));

        //        if (_SimCardListVisibility == Visibility.Collapsed)
        //            SimCardDataEntryVisibility = Visibility.Visible;
        //    }
        //}

        //public Visibility SimCardDataEntryVisibility
        //{
        //    get => _SimCardDataEntryVisibility;
        //    set
        //    {
        //        _SimCardDataEntryVisibility = value;
        //        OnPropertyChanged(nameof(SimCardDataEntryVisibility));

        //        if (_SimCardDataEntryVisibility == Visibility.Collapsed)
        //            SimCardListVisibility = Visibility.Visible;
        //    }
        //}

        //public string ButtonOkSimCardText
        //{
        //    get => _ButtonOkSimCardText;
        //    set
        //    {
        //        _ButtonOkSimCardText = value;
        //        OnPropertyChanged(nameof(ButtonOkSimCardText));
        //    }
        //}

        //public bool SimCardDataEntryEnalble
        //{
        //    get => _SimCardDataEntryEnalble;
        //    set
        //    {
        //        _SimCardDataEntryEnalble = value;
        //        OnPropertyChanged(nameof(SimCardDataEntryEnalble));
        //    }
        //}

        //public ObservableCollection<SimCardEditModel> SimCardList
        //{
        //    get => _SimCardList;
        //    set
        //    {
        //        _SimCardList = value;
        //        OnPropertyChanged(nameof(SimCardList));
        //        if (_SimCardList != null)
        //            SelectedSimCard = _SimCardList.FirstOrDefault();
        //    }
        //}

        //public SimCardEditModel SelectedSimCard
        //{
        //    get => _SelectedSimCard;
        //    set
        //    {
        //        _SelectedSimCard = value;
        //        OnPropertyChanged(nameof(SelectedSimCard));
        //    }
        //}

        //public SimCardEditModel SimCardDataEntry
        //{
        //    get => _SimCardDataEntry;
        //    set
        //    {
        //        _SimCardDataEntry = value;
        //        OnPropertyChanged(nameof(SimCardDataEntry));
        //    }
        //}

        //private async Task AddSimCardCommandAsync()
        //{
        //    ButtonOkSimCardText = Language.Save;
        //    simCardDataEntryPageStatus = PageStatus.Add;
        //    SimCardDataEntryEnalble = true;
        //    SimCardListVisibility = Visibility.Collapsed;

        //    SimCardDataEntry = new SimCardEditModel();
        //    SimCardDataEntry.SimCardId = Guid.NewGuid();
        //    SimCardDataEntry.NetworkDeviceId = _NetworkDevice.Id;

        //    OnPropertyChanged(nameof(SimCardDataEntry));
        //}

        //private async Task EditSimCardCommandAsync()
        //{
        //    if (_SelectedSimCard != null)
        //    {
        //        ButtonOkSimCardText = Language.Save;
        //        simCardDataEntryPageStatus = PageStatus.Edit;
        //        SimCardListVisibility = Visibility.Collapsed;
        //        SimCardDataEntryEnalble = true;

        //        MapModel();
        //        OnPropertyChanged(nameof(SimCardDataEntry));
        //    }
        //}

        //private async Task DeleteSimCardCommandAsync()
        //{
        //    if (_SelectedSimCard != null)
        //    {
        //        ButtonOkSimCardText = Language.Delete;
        //        simCardDataEntryPageStatus = PageStatus.Delete;
        //        SimCardListVisibility = Visibility.Collapsed;
        //        SimCardDataEntryEnalble = false;

        //        MapModel();
        //        OnPropertyChanged(nameof(SimCardDataEntry));
        //    }
        //}

        //private async Task OKSimCardCommandAsync()
        //{
        //    bool changeIsDone = false;
        //    //if (simCardDataEntryPageStatus != PageStatus.Delete)
        //    //{
        //    //    SimCardEditModel tempResult = null;
        //    //    if (_SimCardDataEntry.IsValid)
        //    //    {
        //    //        if (simCardDataEntryPageStatus == PageStatus.Add)
        //    //            tempResult = await SimCardService.AddSimCardAsync(_SimCardDataEntry);
        //    //        else if (simCardDataEntryPageStatus == PageStatus.Edit)
        //    //            tempResult = await SimCardService.EditSimCardAsync(_SimCardDataEntry);

        //    //        if (tempResult != null)
        //    //            changeIsDone = true;
        //    //    }
        //    //}
        //    //else if (simCardDataEntryPageStatus == PageStatus.Delete)
        //    //{
        //    //    await SimCardService.RemoveSimCardAsync(_SimCardDataEntry);
        //    //    changeIsDone = true;
        //    //}

        //    //if (changeIsDone)
        //    //{
        //    //    SimCardDataEntryVisibility = Visibility.Collapsed;
        //    //    LoadNetworkDeviceData();
        //    //}
        //}

        //private async Task CancelSimCardCommandAsync()
        //{
        //    SimCardDataEntryVisibility = Visibility.Collapsed;
        //}

        //private void MapModel()
        //{
        //    SimCardDataEntry = new SimCardEditModel();
        //    SimCardDataEntry.SimCardId = _SelectedSimCard.SimCardId;
        //    SimCardDataEntry.NetworkDeviceId = _NetworkDevice.Id;
        //    SimCardDataEntry.ICCDID = _SelectedSimCard.ICCDID;
        //    SimCardDataEntry.IP = _SelectedSimCard.IP;
        //    SimCardDataEntry.SimCardDescription = _SelectedSimCard.SimCardDescription;
        //    SimCardDataEntry.SimCardNumber = _SelectedSimCard.SimCardNumber;
        //    SimCardDataEntry.SimCardProvider = _SelectedSimCard.SimCardProvider;
        //    SimCardDataEntry.SimCardState = _SelectedSimCard.SimCardState;
        //    SimCardDataEntry.ValidToDate = _SelectedSimCard.ValidToDate;
        //    SimCardDataEntry.VersionId = _SelectedSimCard.VersionId;
        //    SimCardDataEntry.UtcCreateDateTime = _SelectedSimCard.UtcCreateDateTime;
        //    SimCardDataEntry.UtcEditDateTime = _SelectedSimCard.UtcEditDateTime;
        //}

        #endregion

        #endregion

    }
}
