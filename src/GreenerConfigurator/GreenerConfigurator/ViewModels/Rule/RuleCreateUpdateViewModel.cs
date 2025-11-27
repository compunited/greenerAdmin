using Greener.Web.Definitions.API.Rule;
using GreenerConfigurator.ClientCore.Models.Rule;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using global::GreenerConfigurator.ClientCore.Services;
using global::GreenerConfigurator.ClientCore.Services.Rule;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.Rule
{
    public class RuleCreateUpdateViewModel : ViewModelBase
    {

        #region [ Constructor(s) ]

        private readonly RuleService _ruleService;

        public RuleCreateUpdateViewModel(RuleEditModel ruleEditModel, PageStatus pageStatus)
        {
            _ruleService = App.ServiceProvider.GetRequiredService<RuleService>();
            RuleModel = ruleEditModel;
            PageStatus = pageStatus;

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);

            OnAddRuleDetailCommand = new AsyncRelayCommand(AddRuleDetailCommandAsync);
            OnEditRuleDetailCommand = new AsyncRelayCommand(EditRuleDetailCommandAsync);
            OnDeleteRuleDetailCommand = new AsyncRelayCommand(DeleteRuleDetailCommandAsync);

            OnAddNotificationGroupCommand = new AsyncRelayCommand(AddNotificationGroupCommandAsync);
            OnEditNotificationGroupCommand = new AsyncRelayCommand(EditNotificationGroupCommandAsync);
            OnDeleteNotificationGroupCommand = new AsyncRelayCommand(DeleteNotificationGroupCommandAsync);
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnOKCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnAddRuleDetailCommand { get; private set; }

        public ICommand OnEditRuleDetailCommand { get; private set; }

        public ICommand OnDeleteRuleDetailCommand { get; private set; }

        public ICommand OnAddNotificationGroupCommand { get; private set; }

        public ICommand OnEditNotificationGroupCommand { get; private set; }

        public ICommand OnDeleteNotificationGroupCommand { get; private set; }

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

        public RuleEditModel RuleModel
        {
            get => _RuleModel;
            set
            {
                _RuleModel = value;
                OnPropertyChanged(nameof(RuleModel));
                PauseRule = _RuleModel.PauseUntil.HasValue;
            }
        }

        public ObservableCollection<RuleDetailEditModel> RuleDetailList
        {
            get => _RuleDetailList;
            set
            {
                _RuleDetailList = value;
                OnPropertyChanged(nameof(RuleDetailList));
                if (_RuleDetailList != null)
                    SelectedRuleDetail = _RuleDetailList.FirstOrDefault();
            }
        }

        public RuleDetailEditModel SelectedRuleDetail
        {
            get => _SelectedRuleDetail;
            set
            {
                _SelectedRuleDetail = value;
                OnPropertyChanged(nameof(SelectedRuleDetail));
            }
        }

        public ObservableCollection<NotificationGroupDataEditModel> NotificationGroupDataList
        {
            get => _NotificationGroupDataList;
            set
            {
                _NotificationGroupDataList = value;
                OnPropertyChanged(nameof(NotificationGroupDataList));
                if (_NotificationGroupDataList != null)
                    SelectedNotificationGroupData = _NotificationGroupDataList.FirstOrDefault();
            }
        }

        public NotificationGroupDataEditModel SelectedNotificationGroupData
        {
            get => _SelectedNotificationGroupData;
            set
            {
                _SelectedNotificationGroupData = value;
                OnPropertyChanged(nameof(SelectedNotificationGroupData));
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

        public bool PauseRule
        {
            get => _PauseRule;
            set
            {
                _PauseRule = value;
                RuleModel.PauseRule = value;
                OnPropertyChanged(nameof(PauseRule));
                OnPropertyChanged(nameof(RuleModel));

                //if (!_PauseRule)
                //{
                //    _RuleModel.PauseUntil = null;
                //    OnPropertyChanged(nameof(RuleModel));
                //}
            }
        }

        public bool IsAddRuleDetailEnable
        {
            get => _IsAddRuleDetailEnable;
            set
            {
                _IsAddRuleDetailEnable = value;
                OnPropertyChanged(nameof(IsAddRuleDetailEnable));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private PageStatus _PageStatus;
        private ObservableCollection<RuleDetailEditModel> _RuleDetailList = null;
        private RuleDetailEditModel _SelectedRuleDetail = null;
        private string _ButtonOkText = Language.Save;
        private bool _IsEditMode = true;
        private RuleEditModel _RuleModel = null;
        private bool _PauseRule = false;
        private bool _IsAddRuleDetailEnable = false;
        private ObservableCollection<NotificationGroupDataEditModel> _NotificationGroupDataList;
        private NotificationGroupDataEditModel _SelectedNotificationGroupData;

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
                        IsAddRuleDetailEnable = false;
                    }
                    break;
                case PageStatus.Edit:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                        LoadRuleDataAsync();
                        IsAddRuleDetailEnable = true;
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = Language.Delete;
                        IsEditMode = false;
                        IsAddRuleDetailEnable = false;
                        LoadRuleDataAsync();
                    }
                    break;
            }
        }

        private async Task LoadRuleDataAsync()
        {
            RuleModel = await _ruleService.GetRuleByRuleIdAsync(_RuleModel.Id);
            if (_RuleModel != null)
            {
                //PauseRule = _RuleModel.PauseUntil.HasValue;
                ManageRuleDetailDataAsync();
            }
        }

        private async Task ManageRuleDetailDataAsync()
        {
            if (_RuleModel != null)
            {
                if (_RuleModel.RuleDetailList != null)
                {
                    var tempJson = JsonConvert.SerializeObject(_RuleModel.RuleDetailList);
                    List<RuleDetailEditModel> tempRuleDetailList = JsonConvert.DeserializeObject<List<RuleDetailEditModel>>(tempJson);
                    RuleDetailList = new ObservableCollection<RuleDetailEditModel>(tempRuleDetailList);

                    tempJson = JsonConvert.SerializeObject(_RuleModel.NotificationGroupDataList);
                    List<NotificationGroupDataEditModel> tempNotificationGroup = JsonConvert.DeserializeObject<List<NotificationGroupDataEditModel>>(tempJson);
                    NotificationGroupDataList = new ObservableCollection<NotificationGroupDataEditModel>(tempNotificationGroup);
                }
            }
        }

        private async Task OkCommandAsync()
        {
            if (_RuleModel.IsValid)
            {
                RuleEditModel tempResponse = null;
                switch (_PageStatus)
                {
                    case PageStatus.Add:
                        {
                            tempResponse = await _ruleService.AddRuleAsync(_RuleModel);
                        }
                        break;
                    case PageStatus.Edit:
                        {
                            tempResponse = await _ruleService.EditRuleAsync(_RuleModel);
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            await _ruleService.RemoveRuleAsync(_RuleModel);
                            tempResponse = new RuleEditModel();
                        }
                        break;
                }

                if (tempResponse != null)
                {
                    var tempViewModel = new RuleManagementViewModel();
                    App.CurrentView(tempViewModel);
                }
            }
        }

        private async Task CancelCommandAsync()
        {
            var tempViewModel = new RuleManagementViewModel();
            App.CurrentView(tempViewModel);
        }

        private async Task AddRuleDetailCommandAsync()
        {
            RuleDetailEditModel ruleDetailEditModel = new RuleDetailEditModel();
            ruleDetailEditModel.RuleDetailId = Guid.NewGuid();
            ruleDetailEditModel.VersionId = _RuleModel.VersionId;
            ruleDetailEditModel.RuleId = _RuleModel.Id;

            var tempModel = new RuleDetailCreateUpdateViewModel(ruleDetailEditModel, PageStatus.Add);
            App.CurrentView(tempModel);
        }

        private async Task EditRuleDetailCommandAsync()
        {
            if (_SelectedRuleDetail != null)
            {
                var tempModel = new RuleDetailCreateUpdateViewModel(_SelectedRuleDetail, PageStatus.Edit);
                App.CurrentView(tempModel);

                //RuleDetailEditModel ruleDetailEditModel = _SelectedRuleDetail;
                //var temp = Application.Current;
                //((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new RuleDetailCreateUpdateViewModel(ruleDetailEditModel, PageStatus.Edit);
            }
        }

        private async Task DeleteRuleDetailCommandAsync()
        {
            if (_SelectedRuleDetail != null)
            {

                var tempModel = new RuleDetailCreateUpdateViewModel(_SelectedRuleDetail, PageStatus.Delete);
                App.CurrentView(tempModel);

                //RuleDetailEditModel ruleDetailEditModel = _SelectedRuleDetail;
                //var temp = Application.Current;
                //((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new RuleDetailCreateUpdateViewModel(ruleDetailEditModel, PageStatus.Delete);
            }
        }

        private async Task AddNotificationGroupCommandAsync()
        {
            NotificationGroupDataEditModel notificationGroupDataEditModel = new NotificationGroupDataEditModel();
            notificationGroupDataEditModel.RuleId = _RuleModel.Id;
            notificationGroupDataEditModel.NotificationGroupDataId = Guid.NewGuid();
            notificationGroupDataEditModel.VersionId = _RuleModel.VersionId;

            var tempModel = new NotificationGroupDataCreateUpdateViewModel(PageStatus.Add, notificationGroupDataEditModel);
            App.CurrentView(tempModel);
        }

        private async Task EditNotificationGroupCommandAsync()
        {
            if(_SelectedNotificationGroupData!=null)
            {
                var tempModel = new NotificationGroupDataCreateUpdateViewModel(PageStatus.Edit, _SelectedNotificationGroupData);
                App.CurrentView(tempModel);
            }
        }

        private async Task DeleteNotificationGroupCommandAsync()
        {
            if (_SelectedNotificationGroupData != null)
            {
                var tempModel = new NotificationGroupDataCreateUpdateViewModel(PageStatus.Delete, _SelectedNotificationGroupData);
                App.CurrentView(tempModel);
            }
        }

        #endregion

    }
}
