using Greener.Web.Definitions.API.Navigation;
using GreenerConfigurator.Controls.Lookup;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.NavigationCategory
{
    public class NavigationCardGroupCreateUpdateViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly NavigationCardGroupService _navigationCardGroupService;
        private readonly LocationService _locationService;
        private readonly NavigationCategoryService _navigationCategoryService;
        private readonly NavigationCardService _navigationCardService;

        public NavigationCardGroupCreateUpdateViewModel(NavigationCardGroupModel navigationCardGroup, PageStatus pageStatus)
        {
            _navigationCardGroupService = App.ServiceProvider.GetRequiredService<NavigationCardGroupService>();
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();
            _navigationCategoryService = App.ServiceProvider.GetRequiredService<NavigationCategoryService>();
            _navigationCardService = App.ServiceProvider.GetRequiredService<NavigationCardService>();
            NavigationCardGroup = navigationCardGroup;
            PageStatus = pageStatus;

            OnSaveCommand = new AsyncRelayCommand(SaveCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);
            OnLocationLookupCommand = new AsyncRelayCommand(LocationLookupCommandAsync);

            OnAddNavigationCardToGroupCommand = new AsyncRelayCommand(AddNavigationCardToGroupAsync);
            OnDeleteNavigationCardToGroupCommand = new AsyncRelayCommand(DeleteNavigationCardToGroupAsync);

            OnManageCardsCommand = new AsyncRelayCommand(ManageCardsCommandAsync);
            OnManageCategoriesCommand = new AsyncRelayCommand(ManageCategoriesCommandAsync);
            OnAssignCategoryCommand = new AsyncRelayCommand(AssignCategoryCommandAsync);
            OnRemoveAssignedCategoryCommand = new AsyncRelayCommand(RemoveAssignedCategoryCommandAsync);

            LocationLookup = new LookupViewModel();
            LocationLookup.OnSelectedData += LocationLookup_OnSelectedData;

        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnSaveCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnLocationLookupCommand { get; private set; }

        public ICommand OnAddNavigationCardToGroupCommand { get; private set; }

        public ICommand OnDeleteNavigationCardToGroupCommand { get; private set; }

        public ICommand OnManageCardsCommand { get; private set; }

        public ICommand OnManageCategoriesCommand { get; private set; }

        public ICommand OnAssignCategoryCommand { get; private set; }

        public ICommand OnRemoveAssignedCategoryCommand { get; private set; }

        public NavigationCardGroupModel NavigationCardGroup
        {
            get => _NavigationCardGroup;
            set
            {
                _NavigationCardGroup = value;
                OnPropertyChanged(nameof(NavigationCardGroup));
            }
        }

        public ObservableCollection<NavigationCardModel> GroupedNavigationCardList
        {
            get => _GroupedNavigationCardList;
            set
            {
                _GroupedNavigationCardList = value;
                if (_GroupedNavigationCardList != null && _GroupedNavigationCardList.Count > 0)
                    IsNavigationGroupLocationAvailble = false;
                else
                    IsNavigationGroupLocationAvailble = true;

                OnPropertyChanged(nameof(GroupedNavigationCardList));
            }
        }

        public NavigationCardModel SelectedGroupedNavigationCard
        {
            get => _SelectedGroupedNavigationCard;
            set
            {
                _SelectedGroupedNavigationCard = value;
                OnPropertyChanged(nameof(SelectedGroupedNavigationCard));
            }
        }

        public ObservableCollection<NavigationCardModel> NotGroupedNavigationCardList
        {
            get => _NotGroupedNavigationCardList;
            set
            {
                _NotGroupedNavigationCardList = value;
                OnPropertyChanged(nameof(NotGroupedNavigationCardList));
            }
        }

        public NavigationCardModel SelectedNotGroupedNavigationCard
        {
            get => _SelectedNotGroupedNavigationCard;
            set
            {
                _SelectedNotGroupedNavigationCard = value;
                OnPropertyChanged(nameof(SelectedNotGroupedNavigationCard));
            }
        }

        public ObservableCollection<NavigationCategoryModel> AvailableCategoriesList
        {
            get => _AvailableCategoriesList;
            set
            {
                _AvailableCategoriesList = value;
                OnPropertyChanged(nameof(AvailableCategoriesList));
                if (_AvailableCategoriesList != null)
                    SelectedAvailableCategory = _AvailableCategoriesList.FirstOrDefault();
            }
        }

        public NavigationCategoryModel SelectedAvailableCategory
        {
            get => _SelectedAvailableCategory;
            set
            {
                _SelectedAvailableCategory = value;
                OnPropertyChanged(nameof(SelectedAvailableCategory));
            }
        }

        public ObservableCollection<NavigationCategoryModel> AssignedCategoriesList
        {
            get => _AssignedCategoriesList;
            set
            {
                _AssignedCategoriesList = value;
                OnPropertyChanged(nameof(AssignedCategoriesList));
                if (_AssignedCategoriesList != null)
                    SelectedAssignedCategory = _AssignedCategoriesList.FirstOrDefault();
            }
        }

        public NavigationCategoryModel SelectedAssignedCategory
        {
            get => _SelectedAssignedCategory;
            set
            {
                _SelectedAssignedCategory = value;
                OnPropertyChanged(nameof(SelectedAssignedCategory));
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

        public PageStatus PageStatus
        {
            get => _PageStatus;
            set
            {
                _PageStatus = value;
                OnPropertyChanged(nameof(PageStatus));

                ManagePageStatus();
            }
        }

        public LookupViewModel LocationLookup { get; set; }

        public bool IsLookupDeactivated
        {
            get => _IsLookupDeactivated;
            set
            {
                _IsLookupDeactivated = value;
                OnPropertyChanged(nameof(IsLookupDeactivated));
            }
        }

        public bool IsNavigationGroupLocationAvailble
        {
            get => _IsNavigationGroupLocationAvailble;
            set
            {
                _IsNavigationGroupLocationAvailble = value;
                OnPropertyChanged(nameof(IsNavigationGroupLocationAvailble));
            }
        }

        public bool IsAssginedAvailable
        {
            get => _IsAssginedAvailable;
            set
            {
                _IsAssginedAvailable = value;
                OnPropertyChanged(nameof(IsAssginedAvailable));
            }
        }

        public Visibility ManageCardVisiblity
        {
            get => _ManageCardVisiblity;
            set
            {
                _ManageCardVisiblity = value;
                OnPropertyChanged(nameof(ManageCardVisiblity));
                if (_ManageCardVisiblity == Visibility.Visible)
                    ManageCategoriesVisiblity = Visibility.Collapsed;
            }
        }

        public Visibility ManageCategoriesVisiblity
        {
            get => _ManageCategoriesVisiblity;
            set
            {
                _ManageCategoriesVisiblity = value;
                OnPropertyChanged(nameof(ManageCategoriesVisiblity));
                if (_ManageCategoriesVisiblity == Visibility.Visible)
                    ManageCardVisiblity = Visibility.Collapsed;
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private NavigationCardGroupModel _NavigationCardGroup = null;
        private PageStatus _PageStatus;
        private string _ButtonOkText = string.Empty;
        private bool _IsEditMode = false;
        private bool _IsLookupDeactivated = true;
        private ObservableCollection<NavigationCardModel> _GroupedNavigationCardList = null;
        private NavigationCardModel _SelectedGroupedNavigationCard = null;

        private ObservableCollection<NavigationCardModel> _NotGroupedNavigationCardList = null;
        private NavigationCardModel _SelectedNotGroupedNavigationCard = null;
        private bool _IsNavigationGroupLocationAvailble = true;
        private bool _IsAssginedAvailable = false;
        private Visibility _ManageCardVisiblity = Visibility.Collapsed;
        private Visibility _ManageCategoriesVisiblity = Visibility.Collapsed;
        private ObservableCollection<NavigationCategoryModel> _AvailableCategoriesList = null;
        private NavigationCategoryModel _SelectedAvailableCategory;
        private ObservableCollection<NavigationCategoryModel> _AssignedCategoriesList;
        private NavigationCategoryModel _SelectedAssignedCategory;

        #endregion

        #region [ Private Method(s) ]

        private async Task ManagePageStatus()
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
                        IsNavigationGroupLocationAvailble = false;
                        IsAssginedAvailable = true;
                        ManageCardsCommandAsync();
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = Language.Delete;
                        IsEditMode = false;
                        IsNavigationGroupLocationAvailble = false;
                        //TODO: need to been checked
                        ManageCardVisiblity = Visibility.Visible;
                    }
                    break;
            }
        }

        private async Task SaveCommandAsync()
        {
            if (IsEditMode)
            {
                if (ValidateData())
                {
                    if (PageStatus == PageStatus.Add)
                    {
                        _NavigationCardGroup.LogicalDeviceNavigationCardGroupId = Guid.NewGuid();
                        await _navigationCardGroupService.AddNavigationCardGroupAsync(_NavigationCardGroup);
                    }
                    else
                    {
                        await _navigationCardGroupService.EditNavigationCardGroupAsync(_NavigationCardGroup);
                    }

                    var temp = Application.Current;
                    ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel =
                                new NavigationCategoryManagementViewModel(_NavigationCardGroup.LogicalDeviceNavigationCategoryId);
                }
            }
            else
            {
                //Delete Mode
                await _navigationCardGroupService.RemoveNavigationCardGroupAsync(_NavigationCardGroup.LogicalDeviceNavigationCardGroupId);

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel =
                            new NavigationCategoryManagementViewModel(_NavigationCardGroup.LogicalDeviceNavigationCategoryId);
            }
        }

        private async Task CancelCommandAsync()
        {
            var temp = Application.Current;
            if (NavigationCardGroup != null)
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NavigationCategoryManagementViewModel(NavigationCardGroup.LogicalDeviceNavigationCategoryId);
            else
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new NavigationCategoryManagementViewModel();
        }

        private async Task LocationLookupCommandAsync()
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
            LocationLookup.LookupVisibility = Visibility.Visible;
        }

        private async Task ManageCategoriesCommandAsync()
        {
            ManageCategoriesVisiblity = Visibility.Visible;
            ManageLoadCategoriesDataAsync();
        }

        private async Task ManageLoadCategoriesDataAsync()
        {
            var tempCategoryList = await _navigationCategoryService.GetNavigationCategoryByNavigationCardGroupIdAsync(_NavigationCardGroup.LogicalDeviceNavigationCardGroupId);

            if (tempCategoryList == null)
                tempCategoryList = new List<NavigationCategoryModel>();

            AssignedCategoriesList = new ObservableCollection<NavigationCategoryModel>(tempCategoryList);

            var tempAvailableCategoryList = await _navigationCategoryService.GetAvailableNavigationCategoryByNavigationCardGroupIdAsync(_NavigationCardGroup.LogicalDeviceNavigationCardGroupId);
            if (tempAvailableCategoryList == null)
                tempAvailableCategoryList = new List<NavigationCategoryModel>();

            AvailableCategoriesList = new ObservableCollection<NavigationCategoryModel>(tempAvailableCategoryList);

        }

        private async Task ManageCardsCommandAsync()
        {
            ManageCardVisiblity = Visibility.Visible;
            ManageLoadNavigationCardData();
        }

        private void LocationLookup_OnSelectedData(LookupDataModel selectedData)
        {
            if (selectedData != null)
            {
                NavigationCardGroup.LocationId = new Guid(selectedData.Id);
                NavigationCardGroup.LocationName = selectedData.Name;

                OnPropertyChanged(nameof(NavigationCardGroup));

                //ManageLoadNavigationCardData();
            }

            IsLookupDeactivated = true;
        }

        private bool ValidateData()
        {
            bool result = true;

            if (string.IsNullOrEmpty(_NavigationCardGroup.Name))
                result = false;
            if (_NavigationCardGroup.LocationId == Guid.Empty || string.IsNullOrEmpty(_NavigationCardGroup.LocationName))
                result = false;
            if (_NavigationCardGroup.LogicalDeviceNavigationCategoryId == Guid.Empty)
                result = false;

            return result;
        }

        private async Task AddNavigationCardToGroupAsync()
        {
            if (SelectedNotGroupedNavigationCard != null)
            {
                SelectedNotGroupedNavigationCard.LogicalDeviceNavigationCardGroupId = NavigationCardGroup.LogicalDeviceNavigationCardGroupId;
                var temp = await _navigationCardService.SetLogicalDeviceNavigationCardToGroupAsync(SelectedNotGroupedNavigationCard);
                if (temp != null)
                {
                    await ManageLoadNavigationCardData();
                }
            }
        }

        private async Task DeleteNavigationCardToGroupAsync()
        {
            if (SelectedGroupedNavigationCard != null)
            {
                try
                {
                    var tempResult = await _navigationCardService.RemoveLogicalDeviceNavigationCardFromGroupAsync(SelectedGroupedNavigationCard.LogicalDeviceNavigationCardId);

                    if (tempResult != null && !tempResult.LogicalDeviceNavigationCardGroupId.HasValue)
                    {
                        await ManageLoadNavigationCardData();
                    }
                }
                catch (Exception exp)
                {
                    LogHelper.LogError(exp.ToString());
                }
            }
        }

        private async Task ManageLoadNavigationCardData()
        {

            var tempGroupedCardList = await _navigationCardService.GetLogicalDeviceNavigationCardByNavigationCardGroupoIdAsync(
                                                                        _NavigationCardGroup.LogicalDeviceNavigationCardGroupId);
            if (tempGroupedCardList == null)
                tempGroupedCardList = new List<NavigationCardModel>();

            SelectedGroupedNavigationCard = tempGroupedCardList.FirstOrDefault();
            GroupedNavigationCardList = new ObservableCollection<NavigationCardModel>(tempGroupedCardList);

            var notGroupedCardList = await _navigationCardService.GetLogicalDeviceNavigationCardByLoationIdAsync(NavigationCardGroup.LocationId);
            if (notGroupedCardList != null)
                notGroupedCardList = notGroupedCardList.Where(w => !w.LogicalDeviceNavigationCardGroupId.HasValue
                                                              && w.LogicalDeviceNavigationCategoryId == NavigationCardGroup.LogicalDeviceNavigationCategoryId)
                                                       .ToList();
            else
                notGroupedCardList = new List<NavigationCardModel>();

            SelectedNotGroupedNavigationCard = notGroupedCardList.FirstOrDefault();
            NotGroupedNavigationCardList = new ObservableCollection<NavigationCardModel>(notGroupedCardList);
        }

        private async Task AssignCategoryCommandAsync()
        {
            if (_SelectedAvailableCategory != null)
            {
                LogicalDeviceNavigationCardGroupCategoryM2mDto cardGroupCategoryM2m = new LogicalDeviceNavigationCardGroupCategoryM2mDto();
                cardGroupCategoryM2m.LogicalDeviceNavigationCardGroupId = _NavigationCardGroup.LogicalDeviceNavigationCardGroupId;
                cardGroupCategoryM2m.LogicalDeviceNavigationCategoryId = _SelectedAvailableCategory.LogicalDeviceNavigationCategoryId;
                cardGroupCategoryM2m.LogicalDeviceNavigationCardGroupCategoryM2mId = Guid.NewGuid();

                await _navigationCardGroupService.AddNavigationCardGroupCategoryM2mAsync(cardGroupCategoryM2m);

                //TODO: would be better handle direcly the record instead of reload again 

                ManageLoadCategoriesDataAsync();


                //AssignedCategoriesList.Add(_SelectedAvailableCategory);
                //AvailableCategoriesList.Remove(_SelectedAvailableCategory);
                //OnPropertyChanged(nameof(AvailableCategoriesList));
                //OnPropertyChanged(nameof(AssignedCategoriesList));
            }
        }

        private async Task RemoveAssignedCategoryCommandAsync()
        {
            if (_SelectedAssignedCategory != null)
            {
                LogicalDeviceNavigationCardGroupCategoryM2mDto cardGroupCategoryM2m = new LogicalDeviceNavigationCardGroupCategoryM2mDto();
                cardGroupCategoryM2m.LogicalDeviceNavigationCardGroupId = _NavigationCardGroup.LogicalDeviceNavigationCardGroupId;
                cardGroupCategoryM2m.LogicalDeviceNavigationCategoryId = _SelectedAssignedCategory.LogicalDeviceNavigationCategoryId;

                await _navigationCardGroupService.DeleteNavigationCardGroupCategoryM2mAsync(cardGroupCategoryM2m);

                //TODO: would be better handle direcly the record instead of reload again 

                ManageLoadCategoriesDataAsync();
                //AvailableCategoriesList.Add(_SelectedAssignedCategory);
                //AssignedCategoriesList.Remove(_SelectedAssignedCategory);

                //OnPropertyChanged(nameof(AvailableCategoriesList));
                //OnPropertyChanged(nameof(AssignedCategoriesList));
            }
        }

        #endregion
    }
}
