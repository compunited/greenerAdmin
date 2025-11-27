using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using Greener.Web.Definitions.Enums;
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
    public class NavigationCategoryManagementViewModel : ViewModelBase
    {

        #region [ Constructor(s) ]

        private readonly NavigationCategoryService _navigationCategoryService;
        private readonly NavigationCardGroupService _navigationCardGroupService;
        private readonly NavigationCardService _navigationCardService;

        public NavigationCategoryManagementViewModel()
        {
            _navigationCategoryService = App.ServiceProvider.GetRequiredService<NavigationCategoryService>();
            _navigationCardGroupService = App.ServiceProvider.GetRequiredService<NavigationCardGroupService>();
            _navigationCardService = App.ServiceProvider.GetRequiredService<NavigationCardService>();
            LoadNavigationCategoryList();
            OnAddNavigationCardCommand = new RelayCommand(AddNavigationCardCommand);
            OnEditNaviagtionCardCommand = new RelayCommand(EditNavigationCardCommand);
            OnRemoveNavigationCardCommand = new RelayCommand(RemoveNavigationCardCommand);

            OnAddNavigationCardGroupCommand = new RelayCommand(AddNavigationCardGroupCommand);
            OnEditNaviagtionCardGroupCommand = new RelayCommand(EditNaviagtionCardGroupCommand);
            OnRemoveNavigationCardGroupCommand = new RelayCommand(RemoveNavigationCardGroupCommand);
        }

        public NavigationCategoryManagementViewModel(Guid selectedLogicalDeviceNavigationCategoryId)
        {
            _navigationCategoryService = App.ServiceProvider.GetRequiredService<NavigationCategoryService>();
            _navigationCardGroupService = App.ServiceProvider.GetRequiredService<NavigationCardGroupService>();
            _navigationCardService = App.ServiceProvider.GetRequiredService<NavigationCardService>();
            LoadNavigationCategoryList();

            OnAddNavigationCardCommand = new RelayCommand(AddNavigationCardCommand);
            OnEditNaviagtionCardCommand = new RelayCommand(EditNavigationCardCommand);
            OnRemoveNavigationCardCommand = new RelayCommand(RemoveNavigationCardCommand);

            OnAddNavigationCardGroupCommand = new RelayCommand(AddNavigationCardGroupCommand);
            OnEditNaviagtionCardGroupCommand = new RelayCommand(EditNaviagtionCardGroupCommand);
            OnRemoveNavigationCardGroupCommand = new RelayCommand(RemoveNavigationCardGroupCommand);

            this.selectedLogicalDeviceNavigationCategoryId = selectedLogicalDeviceNavigationCategoryId;
        }

        #endregion

        #region  [ Public Property(s) ]

        #region [ Commands ]

        public ICommand OnAddNavigationCardCommand { get; private set; }

        public ICommand OnEditNaviagtionCardCommand { get; private set; }

        public ICommand OnRemoveNavigationCardCommand { get; private set; }

        public ICommand OnAddNavigationCardGroupCommand { get; private set; }

        public ICommand OnEditNaviagtionCardGroupCommand { get; private set; }

        public ICommand OnRemoveNavigationCardGroupCommand { get; private set; }

        #endregion

        public ObservableCollection<NavigationCategoryModel> NavigationCategoryList
        {
            get => _NavigationCategoryList;
            set
            {
                _NavigationCategoryList = value;
                OnPropertyChanged(nameof(NavigationCategoryList));
            }
        }

        public ObservableCollection<NavigationCardModel> NavigationCardList
        {
            get => _NavigationCardList;
            set
            {
                _NavigationCardList = value;
                OnPropertyChanged(nameof(NavigationCardList));
                if (_NavigationCardList != null)
                    SelectedNavigationCard = _NavigationCardList.FirstOrDefault();
            }
        }

        public NavigationCategoryModel SelectedNavigationCategory
        {
            get => _SelectedNavigationCategory;
            set
            {
                _SelectedNavigationCategory = value;
                OnPropertyChanged(nameof(SelectedNavigationCategory));

                ManageLoadNavigationCardInfoAsync();
            }
        }

        public NavigationCardModel SelectedNavigationCard
        {
            get => _SelectedNavigationCard; set
            {
                _SelectedNavigationCard = value;
                OnPropertyChanged(nameof(SelectedNavigationCard));

                //Load sensor data
            }
        }

        public NavigationCardGroupModel SelectedNavigationCardGroup
        {
            get => _SelectedNavigationCardGroup;
            set
            {
                _SelectedNavigationCardGroup = value;
                OnPropertyChanged(nameof(SelectedNavigationCardGroup));
                LoadNavigationCardAsync();
            }
        }

        public ObservableCollection<NavigationCardGroupModel> NavigationCardGroupList
        {
            get => _NavigationCardGroupList;
            set
            {
                _NavigationCardGroupList = value;
                OnPropertyChanged(nameof(NavigationCardGroupList));
                if (_NavigationCardGroupList != null)
                    SelectedNavigationCardGroup = NavigationCardGroupList.FirstOrDefault();
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private ObservableCollection<NavigationCategoryModel> _NavigationCategoryList = null;

        private NavigationCategoryModel _SelectedNavigationCategory = null;

        private ObservableCollection<NavigationCardModel> _NavigationCardList = null;

        private NavigationCardModel _SelectedNavigationCard = null;

        private Guid selectedLogicalDeviceNavigationCategoryId = Guid.Empty;

        private NavigationCardGroupModel _SelectedNavigationCardGroup = null;

        private ObservableCollection<NavigationCardGroupModel> _NavigationCardGroupList = null;

        private List<NavigationCardModel> navigationCardList = null;

        #endregion

        #region [ Private Method(s) ]

        private async void LoadNavigationCategoryList()
        {
            var tempNavigationCategoryList = await _navigationCategoryService.GetAllNavigationCategoryAsync();
            if (tempNavigationCategoryList != null)
            {
                NavigationCategoryList = new ObservableCollection<NavigationCategoryModel>(tempNavigationCategoryList);
                if (selectedLogicalDeviceNavigationCategoryId == Guid.Empty)
                    SelectedNavigationCategory = NavigationCategoryList.FirstOrDefault();
                else
                    SelectedNavigationCategory = NavigationCategoryList.Where(w => w.LogicalDeviceNavigationCategoryId == selectedLogicalDeviceNavigationCategoryId).FirstOrDefault();

                //ManageLoadNavigationCardInfoAsync();
            }
        }

        private async Task ManageLoadNavigationCardInfoAsync()
        {
            await LoadNavigationCardGroupAsync();
            //LoadNavigationCardAsync();
        }

        private async Task LoadNavigationCardGroupAsync()
        {
            if (SelectedNavigationCategory == null)
                return;

            var navigationCardGroup = await _navigationCardGroupService.GetNavigationCardGroupByNavigationCategoryIdAsync
                                                                        (_SelectedNavigationCategory.LogicalDeviceNavigationCategoryId);
            if (navigationCardGroup == null)
                navigationCardGroup = new List<NavigationCardGroupModel>();

            navigationCardGroup.Insert(0, PrepareNotGroupedReord());
            NavigationCardGroupList = new ObservableCollection<NavigationCardGroupModel>(navigationCardGroup);
        }

        private async Task LoadNavigationCardAsync()
        {
            if (SelectedNavigationCategory == null)
                return;

            if (navigationCardList == null ||
                navigationCardList.Where(w =>
                            w.LogicalDeviceNavigationCategoryId == _SelectedNavigationCategory.LogicalDeviceNavigationCategoryId).Count() == 0)
                navigationCardList = await _navigationCardService.GetLogicalDeviceNavigationCardByNavigationCategoryIdAsync(
                                                                          _SelectedNavigationCategory.LogicalDeviceNavigationCategoryId);

            if (navigationCardList != null)
            {
                var tempList = navigationCardList;

                if (_SelectedNavigationCardGroup != null)
                {
                    if (_SelectedNavigationCardGroup.LogicalDeviceNavigationCardGroupId == Guid.Empty)
                        tempList = navigationCardList.Where(w =>
                                                    !w.LogicalDeviceNavigationCardGroupId.HasValue).ToList();
                    else
                        tempList = navigationCardList.Where(w =>
                                                        w.LogicalDeviceNavigationCardGroupId == _SelectedNavigationCardGroup.LogicalDeviceNavigationCardGroupId).ToList();
                }

                NavigationCardList = new ObservableCollection<NavigationCardModel>(tempList);

                //if (_SelectedNavigationCardGroup == null)
                //    SelectedNavigationCard = NavigationCardList.FirstOrDefault();

            }
        }

        private void AddNavigationCardCommand()
        {
            if (SelectedNavigationCategory != null)
            {
                var tempNavigationCard = new NavigationCardModel();
                tempNavigationCard.LogicalDeviceNavigationCategoryId = SelectedNavigationCategory.LogicalDeviceNavigationCategoryId;

                var tempPaintCreateUpdate = new NavigationCardCreateUpdateViewModel(tempNavigationCard, PageStatus.Add);

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempPaintCreateUpdate;
            }
        }

        private void EditNavigationCardCommand()
        {
            if (SelectedNavigationCard != null)
            {
                var tempPaintCreateUpdate = new NavigationCardCreateUpdateViewModel(SelectedNavigationCard, PageStatus.Edit);

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = tempPaintCreateUpdate;
            }
        }

        private async void RemoveNavigationCardCommand()
        {
            if (SelectedNavigationCard != null)
            {
                await _navigationCardService.RemoveLogicalDeviceNavigationCardAsync(SelectedNavigationCard.LogicalDeviceNavigationCardId);
                SelectedNavigationCard = null;
                LoadNavigationCategoryList();
            }
        }

        private void AddNavigationCardGroupCommand()
        {
            if (SelectedNavigationCategory != null)
            {
                var tempCarGroup = new NavigationCardGroupModel();
                tempCarGroup.LogicalDeviceNavigationCategoryId = SelectedNavigationCategory.LogicalDeviceNavigationCategoryId;
                tempCarGroup.NavigationCategory = SelectedNavigationCategory.NavigationCategory;

                var cardGroupViewModel = new NavigationCardGroupCreateUpdateViewModel(tempCarGroup, PageStatus.Add);
                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = cardGroupViewModel;
            }
        }

        private void EditNaviagtionCardGroupCommand()
        {
            if (SelectedNavigationCardGroup != null && SelectedNavigationCardGroup.LogicalDeviceNavigationCardGroupId != Guid.Empty)
            {
                var cardGroupViewModel = new NavigationCardGroupCreateUpdateViewModel(SelectedNavigationCardGroup, PageStatus.Edit);

                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = cardGroupViewModel;
            }
        }

        private void RemoveNavigationCardGroupCommand()
        {
            if (SelectedNavigationCardGroup != null && SelectedNavigationCardGroup.LogicalDeviceNavigationCardGroupId != Guid.Empty)
            {
                var cardGroupViewModel = new NavigationCardGroupCreateUpdateViewModel(SelectedNavigationCardGroup, PageStatus.Delete);
                var temp = Application.Current;
                ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = cardGroupViewModel;
            }
        }

        private NavigationCardGroupModel PrepareNotGroupedReord()
        {
            NavigationCardGroupModel tempNotGrouped = new NavigationCardGroupModel()
            {
                LocationId = Guid.Empty,
                LocationName = string.Empty,
                LogicalDeviceNavigationCardGroupId = Guid.Empty,
                Name = "With No Goroup",
            };

            return tempNotGrouped;
        }

        #endregion

    }
}
