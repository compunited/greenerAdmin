using Greener.Web.Definitions.Enums.Networks;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Models.Network;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
//using System.Windows.Forms;

namespace GreenerConfigurator.ViewModels.NetworkDevice
{
    public class NetworkDeviceManagementViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly LocationService _locationService;
        private readonly LocationDetailService _locationDetailService;
        private readonly NetworkDeviceService _networkDeviceService;

        public NetworkDeviceManagementViewModel()
        {
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();
            _locationDetailService = App.ServiceProvider.GetRequiredService<LocationDetailService>();
            _networkDeviceService = App.ServiceProvider.GetRequiredService<NetworkDeviceService>();
            OnAddNetworkDeviceWithNoLocationCommand = new AsyncRelayCommand(AddNetworkDeviceWithNoLocationCommandAsync);
            OnAddNetworkDeviceCommand = new AsyncRelayCommand(AddNetworkDeviceCommandAsync);
            OnEditNetworkDeviceCommand = new AsyncRelayCommand(EditNetworkDeviceCommandAsync);
            OnDeleteNetworkDeviceCommand = new AsyncRelayCommand(DeleteNetworkDeviceCommandAsync);
            OnInfoNetworkDeviceCommand = new AsyncRelayCommand(InfoNetworkDeviceCommandAsync);
            OnManageUnassignedNetworkDeviceCommand = new AsyncRelayCommand(ManageUnassignedNetworkDeviceCommandAsync);

            GetAllLocationsAsync();
        }

        public NetworkDeviceManagementViewModel(Guid locationId, Guid locationDetailId)
        {
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();
            _locationDetailService = App.ServiceProvider.GetRequiredService<LocationDetailService>();
            _networkDeviceService = App.ServiceProvider.GetRequiredService<NetworkDeviceService>();
            OnAddNetworkDeviceWithNoLocationCommand = new AsyncRelayCommand(AddNetworkDeviceWithNoLocationCommandAsync);
            OnAddNetworkDeviceCommand = new AsyncRelayCommand(AddNetworkDeviceCommandAsync);
            OnEditNetworkDeviceCommand = new AsyncRelayCommand(EditNetworkDeviceCommandAsync);
            OnDeleteNetworkDeviceCommand = new AsyncRelayCommand(DeleteNetworkDeviceCommandAsync);
            OnInfoNetworkDeviceCommand = new AsyncRelayCommand(InfoNetworkDeviceCommandAsync);
            OnManageUnassignedNetworkDeviceCommand = new AsyncRelayCommand(ManageUnassignedNetworkDeviceCommandAsync);

            ManageLoadDataByLocationInfoAsync(locationId, locationDetailId);
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnAddNetworkDeviceWithNoLocationCommand { get; private set; }

        public ICommand OnAddNetworkDeviceCommand { get; private set; }

        public ICommand OnEditNetworkDeviceCommand { get; private set; }

        public ICommand OnDeleteNetworkDeviceCommand { get; private set; }

        public ICommand OnInfoNetworkDeviceCommand { get; private set; }

        public ICommand OnManageUnassignedNetworkDeviceCommand { get; private set; }

        public bool IsNetworkDeviceListEnable
        {
            get => _IsNetworkDeviceListEnable;
            set
            {
                _IsNetworkDeviceListEnable = value;
                OnPropertyChanged(nameof(IsNetworkDeviceListEnable));
            }
        }

        public ObservableCollection<NetworkDeviceViewModel> NetworkDeviceList
        {
            get => _NetworkDeviceList;
            set
            {
                _NetworkDeviceList = value;
                OnPropertyChanged(nameof(NetworkDeviceList));
                if (_NetworkDeviceList != null)
                    SelectedNetworkDevice = _NetworkDeviceList.FirstOrDefault();
            }
        }

        public NetworkDeviceViewModel SelectedNetworkDevice
        {
            get => _SelectedNetworkDevice;
            set
            {
                _SelectedNetworkDevice = value;
                OnPropertyChanged(nameof(SelectedNetworkDevice));
            }
        }

        public ObservableCollection<LocationModel> LocationList
        {
            get => _LocationList;
            set
            {
                _LocationList = value;
                OnPropertyChanged(nameof(LocationList));
                if (_LocationList != null)
                    SelectedLocation = _LocationList.FirstOrDefault();
            }
        }

        public LocationModel SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                _SelectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                if (_SelectedLocation != null)
                {
                    LoadNetworkDeviceListAsync();
                    ManageLoadLocationDetailAsync();
                }
            }
        }

        public ObservableCollection<TreeViewItem> LocationDetailTreeList
        {
            get => _LocationDetailTreeList;
            set
            {
                _LocationDetailTreeList = value;
                OnPropertyChanged(nameof(LocationDetailTreeList));
            }
        }

        public LocationDetailModel SelectedLocationDetail
        {
            get => _SelectedLocationDetail;
            set
            {
                _SelectedLocationDetail = value;
                OnPropertyChanged(nameof(SelectedLocationDetail));
                if (_SelectedLocationDetail != null)
                    LoadNetworkDeviceListAsync();
            }
        }

        public bool FilterByLocationDetail
        {
            get => _FilterByLocationDetail;
            set
            {
                _FilterByLocationDetail = value;
                OnPropertyChanged(nameof(FilterByLocationDetail));
                LoadNetworkDeviceListAsync();
            }
        }

        public bool FilterByLocation
        {
            get => _FilterByLocation;
            set
            {
                _FilterByLocation = value;
                OnPropertyChanged(nameof(FilterByLocation));
                LoadNetworkDeviceListAsync();
            }
        }

        public bool FilterShowAll
        {
            get => _FilterShowAll;
            set
            {
                _FilterShowAll = value;
                OnPropertyChanged(nameof(FilterShowAll));
            }
        }

        public bool FilterByGatewayDevice
        {
            get => _FilterByGatewayDevice;
            set
            {
                _FilterByGatewayDevice = value;
                OnPropertyChanged(nameof(FilterByGatewayDevice));
            }
        }

        public bool FilterByBridgeDevice
        {
            get => _FilterByBridgeDevice;
            set
            {
                _FilterByBridgeDevice = value;
                OnPropertyChanged(nameof(FilterByBridgeDevice));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private bool _IsNetworkDeviceListEnable = true;

        private ObservableCollection<NetworkDeviceViewModel> _NetworkDeviceList = null;
        private NetworkDeviceViewModel _SelectedNetworkDevice = null;

        private ObservableCollection<LocationModel> _LocationList = null;
        private LocationModel _SelectedLocation = null;

        private ObservableCollection<TreeViewItem> _LocationDetailTreeList = null;
        private LocationDetailModel _SelectedLocationDetail = null;
        private List<LocationDetailModel> locationDetailList = null;

        private List<NetworkDeviceViewModel> networkDeviceList = null;
        private bool _FilterByLocationDetail = false;
        private bool _FilterByLocation = false;
        private bool _FilterShowAll = true;
        private bool _FilterByGatewayDevice = false;
        private bool _FilterByBridgeDevice = false;

        #endregion

        #region [ Private Method(s) ]

        private async Task ManageLoadDataByLocationInfoAsync(Guid? locationId, Guid? locationDetailId)
        {
            await GetAllLocationsAsync();
            if (locationId.HasValue)
            {
                SelectedLocation = LocationList.Where(w => w.LocationId == locationId.Value).SingleOrDefault();
                if (locationDetailId.HasValue)
                {
                    FilterByLocationDetail = true;
                }
                else
                    FilterByLocation = true;

            }
        }

        private async Task GetAllLocationsAsync()
        {
            var tempLocations = await _locationService.GetAllLocation();

            if (tempLocations == null)
                tempLocations = new List<LocationModel>();

            LocationList = new ObservableCollection<LocationModel>(tempLocations);
        }

        private async Task ManageLoadLocationDetailAsync()
        {
            if (_SelectedLocation != null)
            {
                locationDetailList = await _locationDetailService.GetLocationDetailByLocationIdAsync(SelectedLocation.LocationId);

                if (locationDetailList == null)
                    locationDetailList = new List<LocationDetailModel>();

                CreateTreeList();
            }
        }

        private void CreateTreeList()
        {
            List<TreeViewItem> tempTreeList = new List<TreeViewItem>();

            var tempRootItems = locationDetailList.Where(w => !w.LocationDetailParentId.HasValue
                                                            || w.LocationDetailParentId == w.LocationDetailId)
                                                  .OrderBy(o => o.LocationDetailName)
                                                  .ToList();
            foreach (var item in tempRootItems)
            {
                var tempNode = CreateTreeItem(item);
                CreateTreeChildItem(tempNode, item.LocationDetailId);

                tempTreeList.Add(tempNode);
            }

            LocationDetailTreeList = new ObservableCollection<TreeViewItem>(tempTreeList);
        }

        private void CreateTreeChildItem(TreeViewItem treeViewItem, Guid locationDetailId)
        {
            var tempChildItems = locationDetailList.Where(w => w.LocationDetailParentId == locationDetailId
                                                          && w.LocationDetailId != locationDetailId)
                                                   .OrderBy(o => o.LocationDetailName)
                                                   .ToList();

            foreach (var item in tempChildItems)
            {
                var tempNode = CreateTreeItem(item);
                CreateTreeChildItem(tempNode, item.LocationDetailId);

                treeViewItem.Items.Add(tempNode);
            }
        }

        private TreeViewItem CreateTreeItem(LocationDetailModel locationDetail)
        {
            TreeViewItem result = new TreeViewItem();

            result.Header = $"{locationDetail.LocationDetailName} - {locationDetail.LocationDetailTypeName}";
            //result.Name = locationDetail.LocationDetailId.ToString();
            result.Tag = locationDetail;

            return result;
        }

        private async Task LoadNetworkDeviceListAsync()
        {
            if (_FilterByLocation)
            {
                if (_FilterByLocationDetail && _SelectedLocationDetail != null)
                    networkDeviceList = await _networkDeviceService.GetNetworkDevicesForLocationDetailIdAsync(_SelectedLocationDetail.LocationDetailId);
                else if (_SelectedLocation != null)
                    networkDeviceList = await _networkDeviceService.GetNetworkDevicesForLocationIdAsync(_SelectedLocation.LocationId);
                else
                    networkDeviceList = await _networkDeviceService.GetAllNetworkDevicesAsync();
            }
            else
            {
                //TODO: should call to load data with no location information
                networkDeviceList = await _networkDeviceService.GetAllNetworkDevicesAsync();
                networkDeviceList = networkDeviceList.Where(w => !w.LocationId.HasValue).ToList();
            }

            ManageTheDeviceFilteringAsync();
        }

        private async Task ManageTheDeviceFilteringAsync()
        {
            List<NetworkDeviceViewModel> temp = networkDeviceList;

            if (_FilterByGatewayDevice)
                temp = temp.Where(w => w.NetworkDeviceType == NetworkDeviceType.Gateway).ToList();
            else if (_FilterByBridgeDevice)
                temp = temp.Where(w => w.NetworkDeviceType == NetworkDeviceType.Bridge).ToList();

            NetworkDeviceList = new ObservableCollection<NetworkDeviceViewModel>(temp);
        }

        private async Task AddNetworkDeviceWithNoLocationCommandAsync()
        {
            NetworkDeviceEditModel tempDeviceModel = new NetworkDeviceEditModel();
            tempDeviceModel.Id = Guid.NewGuid();

            var tempNetworkDeviceViewModel = new NetworkDeviceCreateUpdateViewModel(tempDeviceModel, PageStatus.Add);

            var temp = Application.Current;
            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempNetworkDeviceViewModel;
        }

        private async Task AddNetworkDeviceCommandAsync()
        {
            NetworkDeviceEditModel tempDeviceModel = new NetworkDeviceEditModel();
            tempDeviceModel.Id = Guid.NewGuid();

            // if FilterByLocation be false, user can add networkDevice with no location info
            if (_FilterByLocation && _SelectedLocation != null)
            {
                tempDeviceModel.LocationId = _SelectedLocation.LocationId;
                tempDeviceModel.LocationName = _SelectedLocation.LocationName;

                if (_FilterByLocationDetail && _SelectedLocationDetail != null)
                {
                    //tempDeviceModel.LocationId = _SelectedLocationDetail.LocationId;
                    //tempDeviceModel.LocationName = _SelectedLocation.LocationName;

                    tempDeviceModel.LocationDetailId = _SelectedLocationDetail.LocationDetailId;
                    tempDeviceModel.LocationDetailName = _SelectedLocationDetail.LocationDetailName;
                }
            }

            var tempNetworkDeviceViewModel = new NetworkDeviceCreateUpdateViewModel(tempDeviceModel, PageStatus.Add);

            var temp = Application.Current;
            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempNetworkDeviceViewModel;
        }

        private async Task EditNetworkDeviceCommandAsync()
        {
            if (_SelectedNetworkDevice != null)
            {
                //var tempEditModel = await NetworkDeviceServices.GetNetworkDeviceEditModelByNetworkDevideIdAsync(_SelectedNetworkDevice.Id);

                var tempEditModel = new NetworkDeviceEditModel();
                tempEditModel.Id = _SelectedNetworkDevice.Id;

                var tempNetworkDeviceViewModel = new NetworkDeviceCreateUpdateViewModel(tempEditModel, PageStatus.Edit);

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempNetworkDeviceViewModel;
            }
        }

        private async Task DeleteNetworkDeviceCommandAsync()
        {
            if (_SelectedNetworkDevice != null)
            {
                //var tempEditModel = await NetworkDeviceServices.GetNetworkDeviceEditModelByNetworkDevideIdAsync(_SelectedNetworkDevice.Id);

                var tempEditModel = new NetworkDeviceEditModel();
                tempEditModel.Id = _SelectedNetworkDevice.Id;

                var tempNetworkDeviceViewModel = new NetworkDeviceCreateUpdateViewModel(tempEditModel, PageStatus.Delete);

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempNetworkDeviceViewModel;
            }
        }

        private async Task InfoNetworkDeviceCommandAsync()
        {
            if (_SelectedNetworkDevice != null)
            {

            }
        }

        private async Task ManageUnassignedNetworkDeviceCommandAsync()
        {
            var tempUnassingedNetworkDevice = new UnassignedNetworkDeviceManagementViewModel();

            var temp = Application.Current;
            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempUnassingedNetworkDevice;
        }

        #endregion
    }
}
