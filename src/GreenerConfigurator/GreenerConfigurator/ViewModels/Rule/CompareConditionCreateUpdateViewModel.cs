using Greener.Web.Definitions.API.DeviceState;
using Greener.Web.Definitions.Enums;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Models.Rule;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services.Rule;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using MuiLanguage = global::GreenerConfigurator.ClientCore.Utilities.MUI.Language;
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

namespace GreenerConfigurator.ViewModels.Rule
{
    public class CompareConditionCreateUpdateViewModel : ViewModelBase
    {

        #region [ Construction(s) ]

        private readonly CompareConditionService _compareConditionService;
        private readonly LoraWanDataRowsService _loraWanDataRowsService;
        private readonly LocationDetailService _locationDetailService;

        public CompareConditionCreateUpdateViewModel(CompareConditionEditModel compareConditionEditModel, 
                                                     Guid locationId,
                                                     PageStatus pageStatus)
        {
            _compareConditionService = App.ServiceProvider.GetRequiredService<CompareConditionService>();
            _loraWanDataRowsService = App.ServiceProvider.GetRequiredService<LoraWanDataRowsService>();
            _locationDetailService = App.ServiceProvider.GetRequiredService<LocationDetailService>();
            if (compareConditionEditModel.IsBelongToFirstCompareCondition)
                ExteraHeaderText = "First";
            else
                ExteraHeaderText = "Second";

            this.locationId = locationId;
            CompareConditionEdit = compareConditionEditModel;
            PageStatus = pageStatus;

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);

            OnDeviceLookupCommand = new AsyncRelayCommand(DeviceLookupCommandAsync);
            OnCancelLookupDeviceCommand = new AsyncRelayCommand(CancelLookupDeviceCommandAsync);
            OnSelectLookupDeviceCommand = new AsyncRelayCommand(SelectLookupDeviceCommandAsync);
            OnApplyFilterCommand = new AsyncRelayCommand(ApplyFilterCommandAsync);
            OnResetFilterCommand = new AsyncRelayCommand(ResetFilterCommandAsync);

            var values = Enum.GetValues(typeof(UnitOfMeasurement)).Cast<UnitOfMeasurement>();
            UnitMeasurmentItemList = new List<EnumModel<UnitOfMeasurement>>();

            EnumModel<UnitOfMeasurement> temp = null;

            foreach (var item in values)
            {
                temp = new EnumModel<UnitOfMeasurement>();
                UnitMeasurmentItemList.Add(new EnumModel<UnitOfMeasurement>()
                {
                    EnumItem = item
                });
            }

        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnOKCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnDeviceLookupCommand { get; private set; }

        public ICommand OnCancelLookupDeviceCommand { get; private set; }

        public ICommand OnSelectLookupDeviceCommand { get; private set; }

        public ICommand OnResetFilterCommand { get; private set; }

        public ICommand OnApplyFilterCommand { get; private set; }


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

        public string ExteraHeaderText
        {
            get => _ExteraHeaderText;
            set
            {
                _ExteraHeaderText = value;
                OnPropertyChanged(nameof(ExteraHeaderText));
            }
        }

        public CompareConditionEditModel CompareConditionEdit
        {
            get => _CompareConditionEdit;
            set
            {
                _CompareConditionEdit = value;
                OnPropertyChanged(nameof(CompareConditionEdit));
            }
        }

        public Visibility LookupDeviceVisibility
        {
            get => _LookupDeviceVisibility;
            set
            {
                _LookupDeviceVisibility = value;
                OnPropertyChanged(nameof(LookupDeviceVisibility));
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
                ManageFilterDeviceListAsync();
            }
        }

        public ObservableCollection<LoraWanDataRowsModel> DeviceList
        {
            get => _DeviceList;
            set
            {
                _DeviceList = value;
                OnPropertyChanged(nameof(DeviceList));
                if (_DeviceList != null)
                    SelectedDevice = _DeviceList.FirstOrDefault();
            }
        }

        public LoraWanDataRowsModel SelectedDevice
        {
            get => _SelectedDevice;
            set
            {
                _SelectedDevice = value;
                OnPropertyChanged(nameof(SelectedDevice));
            }
        }

        public EnumModel<UnitOfMeasurement> SelectedUnitMeasurment
        {
            get => _SelectedUnitMeasurment;
            set
            {
                _SelectedUnitMeasurment = value;
                OnPropertyChanged(nameof(SelectedUnitMeasurment));
                if (_SelectedUnitMeasurment != null)
                    ManageFilterDeviceListAsync();
            }
        }

        public List<EnumModel<UnitOfMeasurement>> UnitMeasurmentItemList
        {
            get => _UnitMeasurmentItemList;
            set
            {
                _UnitMeasurmentItemList = value;
                OnPropertyChanged(nameof(UnitMeasurmentItemList));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private PageStatus _PageStatus;
        private string _ButtonOkText = MuiLanguage.Save;
        private bool _IsEditMode = false;
        private CompareConditionEditModel _CompareConditionEdit;
        private string _ExteraHeaderText = string.Empty;
        private Visibility _LookupDeviceVisibility = Visibility.Collapsed;
        private Guid locationId = Guid.Empty;
        private ObservableCollection<TreeViewItem> _LocationDetailTreeList = null;
        private List<LocationDetailModel> locationDetailList = null;
        private LocationDetailModel _SelectedLocationDetail;
        private List<LoraWanDataRowsModel> loraDeviceList = null;
        private ObservableCollection<LoraWanDataRowsModel> _DeviceList = null;
        private EnumModel<UnitOfMeasurement> _SelectedUnitMeasurment = null;
        private List<EnumModel<UnitOfMeasurement>> _UnitMeasurmentItemList;
        private LoraWanDataRowsModel _SelectedDevice = null;

        #endregion

        #region [ Private Method(s) ]

        private async Task ManagePageStatusAsync()
        {
            switch (_PageStatus)
            {
                case PageStatus.Add:
                case PageStatus.Edit:
                    {
                        IsEditMode = true;
                        ButtonOkText = MuiLanguage.Save;
                        IsEditMode = true;
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = MuiLanguage.Delete;
                        ButtonOkText = MuiLanguage.Delete;
                    }
                    break;
            }
        }

        private async Task OkCommandAsync()
        {
            if (_CompareConditionEdit.IsValid)
            {
                CompareConditionEditModel result = null;
                switch (_PageStatus)
                {
                    case PageStatus.Add:
                        {
                            result = await _compareConditionService.AddCompareConditionAsync(_CompareConditionEdit);
                        }
                        break;
                    case PageStatus.Edit:
                        {
                            result = await _compareConditionService.EditCompareConditionAsync(_CompareConditionEdit);
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            await _compareConditionService.RemoveCompareConditionAsync(_CompareConditionEdit);
                            result = new CompareConditionEditModel();
                        }
                        break;
                }

                if (result != null)
                {
                    var tempViewModel = new RuleDetailCreateUpdateViewModel(_CompareConditionEdit.RuleId, _CompareConditionEdit.RuleDetailId);
                    App.CurrentView(tempViewModel);
                }
            }
        }

        private async Task CancelCommandAsync()
        {
            var tempViewModel = new RuleDetailCreateUpdateViewModel(_CompareConditionEdit.RuleId, _CompareConditionEdit.RuleDetailId);
            App.CurrentView(tempViewModel);
        }

        private async Task DeviceLookupCommandAsync()
        {
            LoadLocationDetailDataAsync();

            LoadDeviceDataAsync();

            LookupDeviceVisibility = Visibility.Visible;
            IsEditMode = false;
        }

        private async Task LoadDeviceDataAsync()
        {
            loraDeviceList = await _loraWanDataRowsService.GetLoraWanDataRowsLatestByLocationId(locationId);
            if (loraDeviceList != null)
            {
                var tempDeviceList = loraDeviceList.OrderBy(o => o.PhysicalDeviceName).ToList();
                DeviceList = new ObservableCollection<LoraWanDataRowsModel>(tempDeviceList);
            }
        }

        private async Task LoadLocationDetailDataAsync()
        {

            locationDetailList = await _locationDetailService.GetLocationDetailByLocationIdAsync(locationId);
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

        private async Task ApplyFilterCommandAsync()
        { }

        private async Task ResetFilterCommandAsync()
        {
            SelectedLocationDetail = null;
            SelectedUnitMeasurment = null;

            var tempDeviceList = loraDeviceList.OrderBy(o => o.PhysicalDeviceName).ToList();
            DeviceList = new ObservableCollection<LoraWanDataRowsModel>(tempDeviceList);
        }

        private async Task CancelLookupDeviceCommandAsync()
        {
            LookupDeviceVisibility = Visibility.Collapsed;
            IsEditMode = true;
        }

        private async Task SelectLookupDeviceCommandAsync()
        {
            if (_SelectedDevice != null)
            {
                CompareConditionEdit.PhysicalDeviceId = _SelectedDevice.PhysicalDeviceId;
                CompareConditionEdit.DeviceName = _SelectedDevice.LogicalDeviceName;
                CompareConditionEdit.DataDetailNumber = _SelectedDevice.DataDetailNumber;
                OnPropertyChanged(nameof(CompareConditionEdit));

                LookupDeviceVisibility = Visibility.Collapsed;
                IsEditMode = true;
            }
        }

        private async Task ManageFilterDeviceListAsync()
        {
            var tempList = loraDeviceList;

            if (_SelectedLocationDetail != null)
            {
                tempList = loraDeviceList.Where(w => w.LocationDetailId == _SelectedLocationDetail.LocationDetailId).ToList();
            }

            if (_SelectedUnitMeasurment != null)
            {
                tempList = tempList.Where(w => w.UnitOfMeasurement == _SelectedUnitMeasurment.EnumItem).ToList();
            }

            DeviceList = new ObservableCollection<LoraWanDataRowsModel>(tempList);
        }

        #endregion
    }
}
