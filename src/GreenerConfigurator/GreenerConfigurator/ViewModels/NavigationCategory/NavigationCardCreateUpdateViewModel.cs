using Greener.Web.Definitions.API.Navigation;
using GreenerConfigurator.Controls.Lookup;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.NavigationCategory
{
    public class NavigationCardCreateUpdateViewModel : ViewModelBase
    {

        #region [ Constructor(s) ]

        private readonly NavigationCardService _navigationCardService;
        private readonly DeviceStateService _deviceStateService;
        private readonly LogicalDeviceService _logicalDeviceService;
        private readonly LocationService _locationService;

        public NavigationCardCreateUpdateViewModel(NavigationCardModel navigationCard, PageStatus pageStatus)
        {
            _navigationCardService = App.ServiceProvider.GetRequiredService<NavigationCardService>();
            _deviceStateService = App.ServiceProvider.GetRequiredService<DeviceStateService>();
            _logicalDeviceService = App.ServiceProvider.GetRequiredService<LogicalDeviceService>();
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();
            NavigationCard = navigationCard;
            PageStatus = pageStatus;

            OnOkCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new RelayCommand(CancelCommand);

            OnLocationLookupCommand = new RelayCommand(LocationLookupCommand);
            OnSensorListCommand = new RelayCommand(SensorListCommand);

            OnAddSensorNavigationCardCommand = new AsyncRelayCommand(AddSensorNavigationCardCommandAsync);
            OnRemoveSensorNavigationCardCommand = new AsyncRelayCommand(RemoveSensorNavigationCardCommandAsync);

            OnMoveSensorDownCommand = new AsyncRelayCommand(MoveSensorDownCommand);
            OnMoveSensorUpCommand = new AsyncRelayCommand(MoveSensorUpCommand);
            OnSaveSortnumberChanges = new AsyncRelayCommand(SaveSortnumberChanges);
            OnExchangeDetailNumberCommand = new AsyncRelayCommand(ExchangeDetailNumberAsync);
            OnLogicalDeviceNavigationM2MSelectionChangedCommand = new AsyncRelayCommand(LogicalDeviceNavigationM2MSelectionChangedAsync);

            OnSelectLookupDeviceCommand = new AsyncRelayCommand(SelectLookupDeviceCommandAsync);
            OnCancelLookupDeviceCommand = new RelayCommand(CancelLookupDeviceCommand);

            LocationLookup = new LookupViewModel();
            LocationLookup.OnSelectedData += LocationLookup_OnSelectedData;
            LocationLookup.LookupVisibility = Visibility.Collapsed;

            LogicalDeviceLookup = new LookupViewModel();
            //LogicalDeviceLookup.OnSelectedData += LogicalDeviceLookup_OnSelectedData;
        }

        #endregion

        #region [ Public Property(s) ]

        #region [ Commands ]

        public ICommand OnOkCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnLocationLookupCommand { get; private set; }

        public ICommand OnSensorListCommand { get; private set; }

        public ICommand OnDeleteSensorCommand { get; private set; }

        public ICommand OnAddSensorNavigationCardCommand { get; private set; }

        public ICommand OnRemoveSensorNavigationCardCommand { get; private set; }

        public ICommand OnSelectLookupDeviceCommand { get; private set; }

        public ICommand OnCancelLookupDeviceCommand { get; private set; }

        public ICommand OnMoveSensorUpCommand { get; private set; }

        public ICommand OnMoveSensorDownCommand { get; private set; }

        public ICommand OnSaveSortnumberChanges { get; private set; }

        public ICommand OnExchangeDetailNumberCommand { get; private set; }

        public ICommand OnLogicalDeviceNavigationM2MSelectionChangedCommand { get; private set; }

        #endregion

        public NavigationCardModel NavigationCard
        {
            get => _NavigationCard;
            set
            {
                _NavigationCard = value;
                OnPropertyChanged(nameof(NavigationCard));
            }
        }

        public Visibility SensorDataVisibility
        {
            get => _SensorDataVisibility;
            set
            {
                _SensorDataVisibility = value;
                OnPropertyChanged(nameof(SensorDataVisibility));
            }
        }

        public PageStatus PageStatus
        {
            get => _PageStatus;
            set
            {
                _PageStatus = value;
                ManagePageStatusAsync();
                OnPropertyChanged(nameof(PageStatus));
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

        public string ButtonOkText
        {
            get => _ButtonOkText;
            set
            {
                _ButtonOkText = value;
                OnPropertyChanged(nameof(ButtonOkText));
            }
        }

        public bool IsLookupDeactivated
        {
            get => _IsLookupDeactivated;
            set
            {
                _IsLookupDeactivated = value;
                OnPropertyChanged(nameof(IsLookupDeactivated));
            }
        }

        public bool IsNavigationCardLocationAvailble
        {
            get => _IsNavigationCardLocationAvailble;
            set
            {
                _IsNavigationCardLocationAvailble = value;

                OnPropertyChanged(nameof(IsNavigationCardLocationAvailble));
            }
        }

        public LookupViewModel LocationLookup { get; set; }

        public LookupViewModel LogicalDeviceLookup { get; set; }

        public NavigationM2MModel SelectedNavigationM2M
        {
            get => _SelectedNavigationM2M;
            set
            {
                _SelectedNavigationM2M = value;
                OnPropertyChanged(nameof(SelectedNavigationM2M));
            }
        }

        public bool IsAssginedSensorAvailable
        {
            get => _IsAssginedSensorAvailable;
            set
            {
                _IsAssginedSensorAvailable = value;
                OnPropertyChanged(nameof(IsAssginedSensorAvailable));
            }
        }

        public ObservableCollection<NavigationM2MModel> NavigationCardM2MList
        {
            get => _NavigationCardM2MList;
            set
            {
                _NavigationCardM2MList = value;
                OnPropertyChanged(nameof(NavigationCardM2MList));
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

        public NavigationM2MModel SelectedDeviceNavigationLookup
        {
            get => _SelectedDeviceNavigationLookup;
            set
            {
                _SelectedDeviceNavigationLookup = value;
                OnPropertyChanged(nameof(SelectedDeviceNavigationLookup));
            }
        }

        public string NewNavigationM2MName
        {
            get => _NewNavigationM2MName;
            set
            {
                _NewNavigationM2MName = value;
                OnPropertyChanged(nameof(NewNavigationM2MName));
            }
        }

        public ObservableCollection<NavigationM2MModel> AvailableDeviceListLookup
        {
            get => _AvailableDeviceListLookup;
            set
            {
                _AvailableDeviceListLookup = value;
                OnPropertyChanged(nameof(AvailableDeviceListLookup));
            }
        }

        public bool IsExchangeButtonEnable
        {
            get => _IsExchangeButtonEnable;
            set
            {
                _IsExchangeButtonEnable = value;
                OnPropertyChanged(nameof(IsExchangeButtonEnable));
            }
        }
        #endregion

        #region [ Private Field(s) ]

        private NavigationCardModel _NavigationCard = null;
        private PageStatus _PageStatus;
        private bool _IsEditMode;
        private string _ButtonOkText = string.Empty;
        private bool _IsLookupDeactivated = true;
        private NavigationM2MModel _SelectedNavigationM2M = null;
        private Visibility _SensorDataVisibility = Visibility.Hidden;
        private bool _IsAssginedSensorAvailable = false;

        private List<NavigationM2MModel> navigationCardM2MList = null;
        private ObservableCollection<NavigationM2MModel> _NavigationCardM2MList = null;
        private Visibility _LookupDeviceVisibility = Visibility.Hidden;

        private NavigationM2MModel _SelectedDeviceNavigationLookup = null;
        private string _NewNavigationM2MName = string.Empty;
        private ObservableCollection<NavigationM2MModel> _AvailableDeviceListLookup;
        private List<NavigationM2MModel> availableDeviceListLookup;
        private bool _IsNavigationCardLocationAvailble = true;
        private bool _IsExchangeButtonEnable = false;

        #endregion

        #region [ Private Method(s) ]

        private async Task ManagePageStatusAsync()
        {
            switch (_PageStatus)
            {
                case PageStatus.Add:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                        IsAssginedSensorAvailable = false;
                    }
                    break;
                case PageStatus.Edit:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;

                        await LoadNavigationCardM2MAsync();
                        IsAssginedSensorAvailable = true;
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = Language.Delete;
                        IsEditMode = false;
                        await LoadNavigationCardM2MAsync();
                    }
                    break;
            }
        }

        private async Task OkCommandAsync()
        {
            try
            {
                if (IsEditMode)
                {
                    if (ValidateData())
                    {
                        NavigationCardModel tempResult = null;
                        if (PageStatus == PageStatus.Add)
                        {
                            NavigationCard.LogicalDeviceNavigationCardId = Guid.NewGuid();
                            tempResult = await _navigationCardService.AddNavigationCardAsync(NavigationCard);
                            PageStatus = PageStatus.Edit;
                        }
                        else
                        {
                            tempResult = await _navigationCardService.EditNavigationCardAsync(NavigationCard);

                            var temp = Application.Current;
                            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NavigationCategoryManagementViewModel(NavigationCard.LogicalDeviceNavigationCategoryId);
                        }
                    }
                }
                else
                {
                    //Manage Remove
                }
            }
            catch (Exception exp)
            {
                // TODO need to manage elog
                LogHelper.LogError(exp.ToString());
            }
        }

        private bool ValidateData()
        {
            bool result = true;

            if (string.IsNullOrEmpty(NavigationCard.CardName))
                result = false;
            if (NavigationCard.LocationId == Guid.Empty || string.IsNullOrEmpty(NavigationCard.LocationName))
                result = false;
            if (NavigationCard.LogicalDeviceNavigationCategoryId == Guid.Empty)
                result = false;

            return result;
        }

        private void CancelCommand()
        {
            var temp = Application.Current;
            if (NavigationCard != null)
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NavigationCategoryManagementViewModel(NavigationCard.LogicalDeviceNavigationCategoryId);
            else
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NavigationCategoryManagementViewModel();
        }

        private async Task AddSensorNavigationCardCommandAsync()
        {
            IsAssginedSensorAvailable = false;
            LookupDeviceVisibility = Visibility.Visible;
            IsLookupDeactivated = false;

            availableDeviceListLookup = await _navigationCardService.GetAvailableDevicesByNavigationCardIdAsync(NavigationCard.LogicalDeviceNavigationCardId);

            if (availableDeviceListLookup == null)
                availableDeviceListLookup = new List<NavigationM2MModel>();
            else
                await LoadLatestValueAsync(availableDeviceListLookup);

            AvailableDeviceListLookup = new ObservableCollection<NavigationM2MModel>(availableDeviceListLookup);

            SelectedDeviceNavigationLookup = availableDeviceListLookup.FirstOrDefault();
        }

        private async Task LoadLatestValueAsync(List<NavigationM2MModel> navigationM2MList)
        {
            var tempDeviceStates = await _deviceStateService.GetDeviceStateByLocationIdAsync(NavigationCard.LocationId);

            foreach (var item in tempDeviceStates)
            {
                foreach (var deviceStateDetail in item.DeviceStateDetailViewDtos)
                {
                    var temp = navigationM2MList.Where(w => w.LogicalDeviceId == item.LogicalDeviceId && deviceStateDetail.DataDetailNumber == w.DataDetailNumber).FirstOrDefault();
                    if (temp != null)
                    {
                        temp.LatestValue = deviceStateDetail.Value;
                    }
                }
            }
        }

        private async Task RemoveSensorNavigationCardCommandAsync()
        {
            if (SelectedNavigationM2M != null)
            {
                await _navigationCardService.RemoveLogicalDeviceNavigationM2MAsync(SelectedNavigationM2M.LogicalDeviceNavigationM2mId);
                await LoadNavigationCardM2MAsync();
            }

        }

        private async void SensorListCommand()
        {
            var locationList = await _logicalDeviceService.GetAllLogicalDevice(NavigationCard.LogicalDeviceNavigationCategoryId.ToString());

            List<LookupDataModel> tempLookupList = new List<LookupDataModel>();
            foreach (var item in locationList)
            {
                tempLookupList.Add(
                    new LookupDataModel()
                    {
                        Id = item.LogicalDeviceId.ToString(),
                        Name = item.LogicalDeviceName,
                        ExtraField = item
                    }
                );
            }
            LogicalDeviceLookup.SetDataList(tempLookupList);

            IsLookupDeactivated = false;
            SensorDataVisibility = Visibility.Hidden;
            LocationLookup.LookupVisibility = Visibility.Hidden;
            LogicalDeviceLookup.LookupVisibility = Visibility.Visible;
        }

        private async Task LoadNavigationCardM2MAsync()
        {
            if (NavigationCard?.LocationId == null)
                return;

            navigationCardM2MList = await _navigationCardService.GetLogicalDeviceNavigationCardM2MByNavigationCardIdAsync(NavigationCard.LogicalDeviceNavigationCardId);

            if (navigationCardM2MList == null)
                navigationCardM2MList = new List<NavigationM2MModel>();

            await LoadLatestValueAsync(navigationCardM2MList);

            navigationCardM2MList = navigationCardM2MList.OrderBy(o => o.SortNumber).ToList();

            NavigationCardM2MList = new ObservableCollection<NavigationM2MModel>(navigationCardM2MList);

            SelectedNavigationM2M = navigationCardM2MList.FirstOrDefault();
            if (navigationCardM2MList.Count > 0)
                IsNavigationCardLocationAvailble = false;
            else
                IsNavigationCardLocationAvailble = true;
        }

        private async void LocationLookupCommand()
        {
            var locationList = await _locationService.GetAllLocation();

            List<LookupDataModel> tempLookupList = new List<LookupDataModel>();

            foreach (var item in locationList)
            {
                tempLookupList.Add(
                    new LookupDataModel()
                    {
                        Id = item.LocationId.ToString(),
                        Name = item.LocationName
                    }
                );
            }

            LocationLookup.SetDataList(tempLookupList);

            IsLookupDeactivated = false;
            IsAssginedSensorAvailable = false;

            SensorDataVisibility = Visibility.Hidden;
            LocationLookup.LookupVisibility = Visibility.Visible;
            LogicalDeviceLookup.LookupVisibility = Visibility.Hidden;
        }

        private void LocationLookup_OnSelectedData(LookupDataModel selectedData)
        {
            if (selectedData != null)
            {
                NavigationCard.LocationId = new Guid(selectedData.Id);
                NavigationCard.LocationName = selectedData.Name;

                OnPropertyChanged(nameof(NavigationCard));
            }

            LocationLookup.LookupVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(LocationLookup));

            IsLookupDeactivated = true;
            CheckAssignedSensorSection();
        }

        private void CheckAssignedSensorSection()
        {
            if (PageStatus == PageStatus.Edit)
                IsAssginedSensorAvailable = true;
            else
                IsAssginedSensorAvailable = false;
        }

        private async void AddLogicalDeviceToCard(LookupDataModel lookupDataModel)
        {
            if (lookupDataModel.ExtraField == null)
                return;
            LogicalDeviceModel model = new LogicalDeviceModel();
            model = (LogicalDeviceModel)lookupDataModel.ExtraField;
            // var insertedModel = await _logicalDeviceService.AddLogicalDeviceToCard(model); //This method is not found in the API
		}

        private async Task SelectLookupDeviceCommandAsync()
        {
            if (!string.IsNullOrEmpty(_NewNavigationM2MName) && _SelectedDeviceNavigationLookup != null)
            {
                _SelectedDeviceNavigationLookup.Name = _NewNavigationM2MName;
                _SelectedDeviceNavigationLookup.LogicalDeviceNavigationCardId = NavigationCard.LogicalDeviceNavigationCardId;
                _SelectedDeviceNavigationLookup.LogicalDeviceNavigationM2mId = Guid.NewGuid();
                _SelectedDeviceNavigationLookup.SortNumber = NavigationCardM2MList.Count() + 1;

                await _navigationCardService.AddLogicalDeviceNavigationM2MAsync(_SelectedDeviceNavigationLookup);
                await LoadNavigationCardM2MAsync();

                IsAssginedSensorAvailable = true;
                LookupDeviceVisibility = Visibility.Hidden;
                IsLookupDeactivated = true;

                NewNavigationM2MName = string.Empty;
                SelectedDeviceNavigationLookup = null;
            }
        }

        private void CancelLookupDeviceCommand()
        {
            IsAssginedSensorAvailable = true;
            LookupDeviceVisibility = Visibility.Hidden;
            IsLookupDeactivated = true;

            NewNavigationM2MName = string.Empty;
            SelectedDeviceNavigationLookup = null;
        }

        private async Task MoveSensorUpCommand()
        {
            if (_SelectedNavigationM2M != null)
            {
                var tempSelectedItemIndex = NavigationCardM2MList.ToList().FindIndex(f => f.LogicalDeviceNavigationM2mId == _SelectedNavigationM2M.LogicalDeviceNavigationM2mId);
                var tempRecord = NavigationCardM2MList[tempSelectedItemIndex];
                if (tempSelectedItemIndex > 0)
                {
                    NavigationCardM2MList.RemoveAt(tempSelectedItemIndex);
                    //_SelectedNavigationM2M.SortNumber = tempSelectedItemIndex;
                    NavigationCardM2MList.Insert(tempSelectedItemIndex - 1, tempRecord);
                    //NavigationCardM2MList = new ObservableCollection<NavigationM2MModel>(navigationCardM2MList);
                    OnPropertyChanged(nameof(NavigationCardM2MList));
                    SelectedNavigationM2M = tempRecord;
                }
            }
        }

        private async Task MoveSensorDownCommand()
        {
            if (_SelectedNavigationM2M != null)
            {
                var tempSelectedItemIndex = NavigationCardM2MList.ToList().FindIndex(f => f.LogicalDeviceNavigationM2mId == _SelectedNavigationM2M.LogicalDeviceNavigationM2mId);
                var tempRecord = NavigationCardM2MList[tempSelectedItemIndex];
                if (tempSelectedItemIndex < NavigationCardM2MList.Count - 1)
                {
                    NavigationCardM2MList.RemoveAt(tempSelectedItemIndex);
                    //_SelectedNavigationM2M.SortNumber = tempSelectedItemIndex;
                    if (tempSelectedItemIndex + 1 < NavigationCardM2MList.Count)
                        NavigationCardM2MList.Insert(tempSelectedItemIndex + 1, tempRecord);
                    else
                        NavigationCardM2MList.Add(tempRecord);

                    OnPropertyChanged(nameof(NavigationCardM2MList));
                    SelectedNavigationM2M = tempRecord;
                }
            }
        }

        private async Task SaveSortnumberChanges()
        {
            var tempList = NavigationCardM2MList.ToList();
            for (int i = 0; i < tempList.Count(); i++)
            {
                tempList[i].SortNumber = i + 1;
            }

            //NavigationCardM2MList = new ObservableCollection<NavigationM2MModel>(tempList);

            await _navigationCardService.UpdateLogicalDeviceNavigationM2MListAsync(_NavigationCardM2MList.ToList());

            await LoadNavigationCardM2MAsync();
        }

        private async Task ExchangeDetailNumberAsync()
        {
            if (SelectedNavigationM2M.DataDetailNumber == 31 || SelectedNavigationM2M.DataDetailNumber == 30)
            {
                var tempItems = NavigationCardM2MList.Where(w => w.LogicalDeviceId == SelectedNavigationM2M.LogicalDeviceId).ToList();

                if (tempItems.Count() == 2)
                {
                    foreach (var tempItem in tempItems)
                    {

                        if (tempItem.DataDetailNumber != 31)
                        {
                            tempItem.DataDetailNumber = 31;
                        }
                        else
                            tempItem.DataDetailNumber = 30;
                    }
                }

                await _navigationCardService.UpdateLogicalDeviceNavigationM2MListAsync(tempItems);

                await LoadNavigationCardM2MAsync();
            }
        }

        public async Task LogicalDeviceNavigationM2MSelectionChangedAsync()
        {
            if (SelectedNavigationM2M != null)
            {
                if (SelectedNavigationM2M.DataDetailNumber == 31 || SelectedNavigationM2M.DataDetailNumber == 30)
                {
                    IsExchangeButtonEnable = true;
                }
                else
                {
                    IsExchangeButtonEnable = false;
                }
            }
        }

        #endregion

    }
}
