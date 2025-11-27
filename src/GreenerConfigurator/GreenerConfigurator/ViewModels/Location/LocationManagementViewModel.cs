using Greener.Web.Definitions.Enums;
using GreenerConfigurator.Controls.Lookup;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Services.Location;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Language = GreenerConfigurator.ClientCore.Utilities.MUI.Language;

namespace GreenerConfigurator.ViewModels.Location
{
    public class LocationManagementViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly LocationService _locationService;
        private readonly LocationDetailService _locationDetailService;

        public LocationManagementViewModel()
        {
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();
            _locationDetailService = App.ServiceProvider.GetRequiredService<LocationDetailService>();
            LocationsLookup = new LookupViewModel();
            LocationsLookup.OnSelectedData += LocationLookup_OnSelectedData;

            OnAddLocationCommand = new AsyncRelayCommand(AddLocationCommandAsync);
            OnEditLocationCommand = new AsyncRelayCommand(EditLocationCommandAsync);
            OnDeleteLocationCommand = new AsyncRelayCommand(DeleteLocationCommandAsync);

            OnSaveLocationCommand = new AsyncRelayCommand(SaveLocationCommandAsync);
            OnCancelLocationCommand = new AsyncRelayCommand(CancelLocationCommandAsync);

            OnAddLocationDetailCommand = new AsyncRelayCommand(AddLocationDetailCommandAsync);
            OnEditLocationDetailCommand = new AsyncRelayCommand(EditLocationDetailCommandASync);
            OnDeleteLocationDetailCommand = new AsyncRelayCommand(DeleteLocationDetailCommandASync);

            OnSaveLocationDetailCommand = new AsyncRelayCommand(SaveLocationDetailCommandAsync);
            OnCancelLocationDetailCommand = new AsyncRelayCommand(CancelLocationDetailCommandAsync);

            LocationPageStatus = PageStatus.Idle;
            //ManagePageState();

            GetAllLocations();

        }

        #endregion

        #region [ Public Property(s) ]

        public LookupViewModel LocationsLookup { get; set; }

        public ICommand OnAddLocationCommand { get; private set; }

        public ICommand OnEditLocationCommand { get; private set; }

        public ICommand OnDeleteLocationCommand { get; private set; }

        public ICommand OnSaveLocationCommand { get; private set; }

        public ICommand OnCancelLocationCommand { get; private set; }

        public ICommand OnAddLocationDetailCommand { get; private set; }

        public ICommand OnEditLocationDetailCommand { get; private set; }

        public ICommand OnDeleteLocationDetailCommand { get; private set; }

        public ICommand OnSaveLocationDetailCommand { get; private set; }

        public ICommand OnCancelLocationDetailCommand { get; private set; }

        public ObservableCollection<LocationModel> LocationList
        {
            get => _LocationList;
            set
            {
                _LocationList = value;
                OnPropertyChanged(nameof(LocationList));
            }
        }

        public LocationModel SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                _SelectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                ManageLoadLocationDetailAsync();
            }
        }

        public LocationModel AddUpdateLocation
        {
            get => _AddUpdateLocation;
            set
            {
                _AddUpdateLocation = value;
                OnPropertyChanged(nameof(AddUpdateLocation));
            }
        }

        public bool IsLocationEditEnable
        {
            get => _IsLocationEditEnable;
            set
            {
                _IsLocationEditEnable = value;
                OnPropertyChanged(nameof(IsLocationEditEnable));
            }
        }

        public bool IsLocationListEnable
        {
            get => _IsLocationListEnable;
            set
            {
                _IsLocationListEnable = value;
                OnPropertyChanged(nameof(IsLocationListEnable));
            }
        }


        public PageStatus LocationPageStatus
        {
            get => _LocationPageStatus;
            set
            {
                _LocationPageStatus = value;
                switch (_LocationPageStatus)
                {
                    case PageStatus.Add:
                    case PageStatus.Edit:
                        {
                            ButtonOkLocationText = Language.Save;
                            IsLocationListEnable = false;
                            IsLocationDetailListEnable = false;
                            IsLocationEditEnable = true;
                            IsLocationDetailEditEnable = false;
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            IsLocationListEnable = false;
                            IsLocationDetailListEnable = false;
                            IsLocationEditEnable = true;
                            IsLocationDetailEditEnable = false;
                            ButtonOkLocationText = Language.Delete;
                        }
                        break;
                    case PageStatus.Idle:
                        {
                            IsLocationListEnable = true;
                            IsLocationDetailListEnable = true;
                            IsLocationEditEnable = false;
                            IsLocationDetailEditEnable = false;
                        }
                        break;
                }
                OnPropertyChanged(nameof(LocationPageStatus));
            }
        }

        public string ButtonOkLocationText
        {
            get => _ButtonOkLocationText;
            set
            {
                _ButtonOkLocationText = value;
                OnPropertyChanged(nameof(ButtonOkLocationText));
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
            }
        }

        public LocationDetailModel AddUpdateLocationDetail
        {
            get => _AddUpdateLocationDetail;
            set
            {
                _AddUpdateLocationDetail = value;
                OnPropertyChanged(nameof(AddUpdateLocationDetail));
            }
        }

        public bool IsLocationDetailListEnable
        {
            get => _IsLocationDetailListEnable;
            set
            {
                _IsLocationDetailListEnable = value;
                OnPropertyChanged(nameof(IsLocationDetailListEnable));
            }
        }

        public PageStatus LocationDetailPageStatus
        {
            get => _LocationDetailPageStatus;
            set
            {
                _LocationDetailPageStatus = value;
                switch (_LocationDetailPageStatus)
                {
                    case PageStatus.Add:
                    case PageStatus.Edit:
                        {
                            ButtonOkLocationDetailText = Language.Save;
                            IsLocationListEnable = false;
                            IsLocationDetailListEnable = false;
                            IsLocationEditEnable = false;
                            IsLocationDetailEditEnable = true;
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            ButtonOkLocationDetailText = Language.Delete;
                            IsLocationListEnable = false;
                            IsLocationDetailListEnable = false;
                            IsLocationEditEnable = false;
                            IsLocationDetailEditEnable = true;
                        }
                        break;
                    case PageStatus.Idle:
                        {
                            IsLocationListEnable = true;
                            IsLocationDetailListEnable = true;
                            IsLocationEditEnable = false;
                            IsLocationDetailEditEnable = false;
                        }
                        break;
                }


                OnPropertyChanged(nameof(LocationDetailPageStatus));
            }
        }

        public string ButtonOkLocationDetailText
        {
            get => _ButtonOkLocationDetailText;
            set
            {
                _ButtonOkLocationDetailText = value;
                OnPropertyChanged(nameof(ButtonOkLocationDetailText));
            }
        }

        public bool IsLocationDetailEditEnable
        {
            get => _IsLocationDetailEditEnable;
            set
            {
                _IsLocationDetailEditEnable = value;
                OnPropertyChanged(nameof(IsLocationDetailEditEnable));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private ObservableCollection<LocationModel> _LocationList = null;
        private ObservableCollection<TreeViewItem> _LocationDetailTreeList = null;

        private LocationModel _SelectedLocation = null;
        private LocationDetailModel _SelectedLocationDetail = null;

        private bool _IsLocationEditEnable = false;
        private PageStatus _LocationPageStatus = PageStatus.Idle;
        private PageStatus _LocationDetailPageStatus = PageStatus.Idle;
        private bool _IsLocationListEnable = true;
        private string _ButtonOkLocationText = Language.Save;
        private LocationModel _AddUpdateLocation = null;
        private bool _IsLocationDetailListEnable = true;
        private List<LocationDetailModel> locationDetailList = null;
        private string _ButtonOkLocationDetailText = Language.Save;
        private LocationDetailModel _AddUpdateLocationDetail = null;
        private bool _IsLocationDetailEditEnable = false;

        #endregion

        #region [ Private Method(s) ]

        private async Task AddLocationCommandAsync()
        {
            AddUpdateLocation = new LocationModel();
            AddUpdateLocation.LocationId = Guid.NewGuid();
            //AddUpdateLocation.LocationType = LocationType.UnspecifiedBuilding;
            LocationPageStatus = PageStatus.Add;

            //IsLocationEditEnable = true;
            //ManagePageState();
        }

        private async Task EditLocationCommandAsync()
        {
            AddUpdateLocation = SelectedLocation;
            LocationPageStatus = PageStatus.Edit;

            //IsLocationEditEnable = true;
            //ManagePageState();
        }

        private async Task DeleteLocationCommandAsync()
        {

        }

        private async Task SaveLocationCommandAsync()
        {
            if (AddUpdateLocation != null)
            {
                LocationModel temp = null;
                if (LocationPageStatus == PageStatus.Add)
                    temp = await _locationService.AddLocationAsync(AddUpdateLocation);
                else
                    temp = await _locationService.EditLocationAsync(AddUpdateLocation);

                if (temp != null)
                {
                    await GetAllLocations();

                    AddUpdateLocation = null;
                    LocationPageStatus = PageStatus.Idle;

                    //IsLocationEditEnable = false;
                    //ManagePageState();
                }
            }
        }

        private async Task CancelLocationCommandAsync()
        {
            AddUpdateLocation = null;
            LocationPageStatus = PageStatus.Idle;

            //IsLocationEditEnable = false;
            //ManagePageState();
        }

        private async Task GetAllLocations()
        {
            var tempLocations = await _locationService.GetAllLocation();

            if (tempLocations == null)
                tempLocations = new List<LocationModel>();

            LocationList = new ObservableCollection<LocationModel>(tempLocations);
            SelectedLocation = tempLocations.FirstOrDefault();
        }

        private void LocationLookup_OnSelectedData(LookupDataModel selectedData)
        {
            if (selectedData != null)
            {

            }
        }

        private void ManagePageState()
        {
            switch (LocationPageStatus)
            {
                case PageStatus.Idle:
                    {
                        IsLocationListEnable = true;
                        IsLocationDetailListEnable = true;
                        IsLocationEditEnable = false;
                        IsLocationDetailEditEnable = false;
                    }
                    break;
                case PageStatus.Add:
                case PageStatus.Edit:
                case PageStatus.Delete:
                    {
                        IsLocationListEnable = false;
                        IsLocationDetailListEnable = false;
                        IsLocationEditEnable = true;
                        IsLocationDetailEditEnable = false;
                    }
                    break;
            }

            switch (LocationDetailPageStatus)
            {
                case PageStatus.Idle:
                    {
                        IsLocationListEnable = true;
                        IsLocationDetailListEnable = true;
                        IsLocationEditEnable = false;
                        IsLocationDetailEditEnable = false;
                    }
                    break;
                case PageStatus.Add:
                case PageStatus.Edit:
                case PageStatus.Delete:
                    {
                        IsLocationListEnable = false;
                        IsLocationDetailListEnable = false;
                        IsLocationEditEnable = false;
                        IsLocationDetailEditEnable = true;
                    }
                    break;

            }
            //if (IsLocationEditEnable)
            //{
            //    IsLocationListEnable = false;
            //    IsLocationDetailListEnable = false;
            //    IsLocationDetailEditEnable = false;
            //}
            //else
            //{
            //    IsLocationListEnable = true;
            //    IsLocationDetailListEnable = true;
            //}

            //if (IsLocationListEnable)
            //{
            //    //IsLocationDetailListEnable = true;
            //    IsLocationEditEnable = false;
            //    //IsLocationDetailEditEnable = false;
            //}

            //if (IsLocationDetailListEnable)
            //{
            //    IsLocationDetailEditEnable = false;
            //}

            //if (IsLocationDetailEditEnable)
            //{
            //    IsLocationDetailListEnable = false;
            //    IsLocationEditEnable = false;
            //    IsLocationEditEnable = false;
            //}
        }

        private async Task ManageLoadLocationDetailAsync()
        {
            if (SelectedLocation != null)
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

        private async Task AddLocationDetailCommandAsync()
        {
            if (SelectedLocation != null)
            {
                AddUpdateLocationDetail = new LocationDetailModel();
                AddUpdateLocationDetail.LocationDetailId = Guid.NewGuid();
                AddUpdateLocationDetail.LocationId = SelectedLocation.LocationId;

                if (SelectedLocationDetail == null)
                    AddUpdateLocationDetail.AddAsRoot = true;
                LocationDetailPageStatus = PageStatus.Add;
            }
        }

        private async Task EditLocationDetailCommandASync()
        {
            if (SelectedLocationDetail != null)
            {
                AddUpdateLocationDetail = SelectedLocationDetail;

                LocationDetailPageStatus = PageStatus.Edit;
            }
        }

        private async Task DeleteLocationDetailCommandASync()
        { }

        private async Task SaveLocationDetailCommandAsync()
        {
            if (AddUpdateLocationDetail != null && IsLocationDetailEditEnable)
            {
                LocationDetailModel tempLocationDetail = null;

                if (LocationDetailPageStatus == PageStatus.Add)
                {
                    if (SelectedLocationDetail != null && AddUpdateLocationDetail.AddAsRoot)
                        AddUpdateLocationDetail.LocationDetailParentId = SelectedLocationDetail.LocationDetailId;
                        tempLocationDetail = await _locationDetailService.AddLocationDetail(AddUpdateLocationDetail);
                }
                else if (LocationDetailPageStatus == PageStatus.Edit)
                    tempLocationDetail = await _locationDetailService.EditLocationDetail(AddUpdateLocationDetail);

                if (tempLocationDetail != null)
                {
                    AddUpdateLocationDetail = null;
                    LocationDetailPageStatus = PageStatus.Idle;
                    ManageLoadLocationDetailAsync();
                }

            }
        }

        private async Task CancelLocationDetailCommandAsync()
        {
            AddUpdateLocationDetail = null;

            LocationDetailPageStatus = PageStatus.Idle;
        }

        #endregion
    }
}
