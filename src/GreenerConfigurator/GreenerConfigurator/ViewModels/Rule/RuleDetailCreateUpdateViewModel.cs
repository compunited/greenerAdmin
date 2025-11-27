using Greener.Web.Definitions.API.Rule;
using GreenerConfigurator.Controls.Lookup;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Models.Rule;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services.Rule;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
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
    public class RuleDetailCreateUpdateViewModel : ViewModelBase
    {

        #region [ Constructor(s) ]

        public RuleDetailCreateUpdateViewModel(RuleDetailEditModel ruleDetail, PageStatus pageStatus)
        {
            _ruleService = App.ServiceProvider.GetRequiredService<RuleService>();
            _ruleDetailService = App.ServiceProvider.GetRequiredService<RuleDetailService>();
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();

            RuleDetail = ruleDetail;
            PageStatus = pageStatus;

            Init();
        }

        public RuleDetailCreateUpdateViewModel(Guid ruleId, Guid ruleDetailId)
        {
            _ruleService = App.ServiceProvider.GetRequiredService<RuleService>();
            _ruleDetailService = App.ServiceProvider.GetRequiredService<RuleDetailService>();
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();

            Init();

            ManageLoadingRuleDetailAsync(ruleId, ruleDetailId);
        }

        #endregion

        #region [ Public Property(s) ]

        #region [ ICommand ]

        public ICommand OnOKCommand { get; private set; }
        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnAddActiveTimeCommand { get; private set; }
        public ICommand OnEditActiveTimeCommand { get; private set; }
        public ICommand OnDeleteActiveTimeCommand { get; private set; }

        public ICommand OnLocationLookupCommand { get; private set; }

        public ICommand OnAddFirstCompareConditionCommand { get; private set; }
        public ICommand OnEditFirstCompareConditionCommand { get; private set; }
        public ICommand OnDeleteFirstCompareConditionCommand { get; private set; }

        public ICommand OnAddSecondCompareConditionCommand { get; private set; }
        public ICommand OnEditSecondCompareConditionCommand { get; private set; }
        public ICommand OnDeleteSecondCompareConditionCommand { get; private set; }

        #endregion

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

        public RuleDetailEditModel RuleDetail
        {
            get => _RuleDetail;
            set
            {
                _RuleDetail = value;
                OnPropertyChanged(nameof(RuleDetail));
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

        public bool IsAddDetailEnabled
        {
            get => _IsAddDetailEnabled;
            set
            {
                _IsAddDetailEnabled = value;
                OnPropertyChanged(nameof(IsAddDetailEnabled));
            }
        }

        public ObservableCollection<CompareConditionEditModel> FirstCompareConditionList
        {
            get => _FirstCompareConditionList;
            set
            {
                _FirstCompareConditionList = value;
                OnPropertyChanged(nameof(FirstCompareConditionList));
                if (_FirstCompareConditionList != null)
                {
                    SelectedFirstCompareCondition = _FirstCompareConditionList.FirstOrDefault();
                    if (_FirstCompareConditionList.Count > 0)
                    {
                        IsSecondCompareConditionListEnabled = true;
                        IsSelectLocationEnable = false;
                    }
                    else
                    {
                        IsSecondCompareConditionListEnabled = false;
                        IsSelectLocationEnable = true;
                    }
                }
                else
                {
                    IsSelectLocationEnable = true;
                }
            }
        }

        public CompareConditionEditModel SelectedFirstCompareCondition
        {
            get => _SelectedFirstCompareCondition;
            set
            {
                _SelectedFirstCompareCondition = value;
                OnPropertyChanged(nameof(SelectedFirstCompareCondition));
            }
        }

        public ObservableCollection<CompareConditionEditModel> SecondCompareConditionList
        {
            get => _SecondCompareConditionList;
            set
            {
                _SecondCompareConditionList = value;
                OnPropertyChanged(nameof(SecondCompareConditionList));
                if (_SecondCompareConditionList != null)
                    SelectedSecondCompareCondition = _SecondCompareConditionList.FirstOrDefault();
            }
        }

        public CompareConditionEditModel SelectedSecondCompareCondition
        {
            get => _SelectedSecondCompareCondition;
            set
            {
                _SelectedSecondCompareCondition = value;
                OnPropertyChanged(nameof(SelectedSecondCompareCondition));
            }
        }

        public ObservableCollection<ActiveTimeEditModel> ActiveTimeList
        {
            get => _ActiveTimeList;
            set
            {
                _ActiveTimeList = value;
                OnPropertyChanged(nameof(ActiveTimeList));
                if (_ActiveTimeList != null)
                    SelectedActiveTime = _ActiveTimeList.FirstOrDefault();
            }
        }

        public ActiveTimeEditModel SelectedActiveTime
        {
            get => _SelectedActiveTime;
            set
            {
                _SelectedActiveTime = value;
                OnPropertyChanged(nameof(SelectedActiveTime));
            }
        }

        public bool IsSecondCompareConditionListEnabled
        {
            get => _IsSecondCompareConditionListEnabled;
            set
            {
                _IsSecondCompareConditionListEnabled = value;
                OnPropertyChanged(nameof(IsSecondCompareConditionListEnabled));
            }
        }

        public Visibility ComopareConditionVisibility
        {
            get => _ComopareConditionVisibility;
            set
            {
                _ComopareConditionVisibility = value;
                OnPropertyChanged(nameof(ComopareConditionVisibility));
            }
        }

        public Visibility ActiveTimeVisibility
        {
            get => _ActiveTimeVisibility;
            set
            {
                _ActiveTimeVisibility = value;
                OnPropertyChanged(nameof(ActiveTimeVisibility));
            }
        }

        public LookupViewModel LocationLookup { get; set; }

        public bool IsSelectLocationEnable
        {
            get => _IsSelectLocationEnable;
            set
            {
                _IsSelectLocationEnable = value;
                OnPropertyChanged(nameof(IsSelectLocationEnable));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private PageStatus _PageStatus;
        private bool _IsEditMode = false;
        private string _ButtonOkText = Language.Save;
        private RuleDetailEditModel _RuleDetail;
        private ObservableCollection<CompareConditionEditModel> _FirstCompareConditionList = null;
        private ObservableCollection<CompareConditionEditModel> _SecondCompareConditionList = null;
        private ObservableCollection<ActiveTimeEditModel> _ActiveTimeList = null;
        private CompareConditionEditModel _SelectedFirstCompareCondition = null;
        private CompareConditionEditModel _SelectedSecondCompareCondition = null;
        private ActiveTimeEditModel _SelectedActiveTime = null;
        private bool _IsAddDetailEnabled = false;
        private bool _IsSecondCompareConditionListEnabled = false;
        private Visibility _ComopareConditionVisibility = Visibility.Collapsed;
        private Visibility _ActiveTimeVisibility = Visibility.Visible;
        private bool _IsSelectLocationEnable = true;
        private LocationModel _SelectedLocation;
        private readonly RuleService _ruleService;
        private readonly RuleDetailService _ruleDetailService;
        private readonly LocationService _locationService;

        #endregion

        #region [ Private Method(s) ]

        private void Init()
        {

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);

            OnAddActiveTimeCommand = new AsyncRelayCommand(AddActiveTimeCommandAsync);
            OnEditActiveTimeCommand = new AsyncRelayCommand(EditActiveTimeCommandAsync);
            OnDeleteActiveTimeCommand = new AsyncRelayCommand(DeleteActiveTimeCommandAsync);

            OnLocationLookupCommand = new AsyncRelayCommand(LocationLookupCommandAsync);

            OnAddFirstCompareConditionCommand = new AsyncRelayCommand(AddFirstCompareConditionCommandAsync);
            OnEditFirstCompareConditionCommand = new AsyncRelayCommand(EditFirstCompareConditionCommandAsync);
            OnDeleteFirstCompareConditionCommand = new AsyncRelayCommand(DeleteFirstCompareConditionCommandAsync);

            OnAddSecondCompareConditionCommand = new AsyncRelayCommand(AddSecondCompareConditionCommandAsync);
            OnEditSecondCompareConditionCommand = new AsyncRelayCommand(EditSecondCompareConditionCommandAsync);
            OnDeleteSecondCompareConditionCommand = new AsyncRelayCommand(DeleteSecondCompareConditionCommandAsync);

            LocationLookup = new LookupViewModel();
            LocationLookup.OnSelectedData += LocationLookup_OnSelectedData;
        }

        private async Task ManageLoadingRuleDetailAsync(Guid ruleId, Guid ruleDetailId)
        {
            var tempRule = await _ruleService.GetRuleByRuleIdAsync(ruleId);
            if (tempRule == null)
            {
                App.CurrentView(new RuleManagementViewModel());
                return;
            }

            if (tempRule.RuleDetailList == null)
            {
                RuleCreateUpdateViewModel tempModel = new RuleCreateUpdateViewModel(tempRule, PageStatus.Edit);
                App.CurrentView(tempModel);
            }

            var tempRuleDetail = tempRule.RuleDetailList.Where(w => w.RuleDetailId == ruleDetailId).FirstOrDefault();
            if (tempRuleDetail == null)
            {
                RuleCreateUpdateViewModel tempModel = new RuleCreateUpdateViewModel(tempRule, PageStatus.Edit);
                App.CurrentView(tempModel);
            }

            var tempJson = JsonConvert.SerializeObject(tempRuleDetail);
            RuleDetail = JsonConvert.DeserializeObject<RuleDetailEditModel>(tempJson);
            PageStatus = PageStatus.Edit;
        }

        private async Task ManagePageStatusAsync()
        {
            switch (_PageStatus)
            {
                case PageStatus.Add:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                        IsAddDetailEnabled = false;
                    }
                    break;
                case PageStatus.Edit:
                    {
                        ButtonOkText = Language.Save;
                        IsEditMode = true;
                        if (RuleDetail.LocationId != Guid.Empty)
                            IsAddDetailEnabled = true;
                        ManageRulesDetailDataAsync();
                    }
                    break;
                case PageStatus.Delete:
                    {
                        ButtonOkText = Language.Delete;
                        IsEditMode = false;
                        IsAddDetailEnabled = false;
                        ManageRulesDetailDataAsync();
                    }
                    break;
            }
        }

        private async Task ManageRulesDetailDataAsync()
        {
            if (_RuleDetail != null)
            {
                var locationList = await _locationService.GetAllLocation();
                var tempLocation = locationList.Where(w => w.LocationId == _RuleDetail.LocationId).FirstOrDefault();
                if (tempLocation == null)
                {
                    //TODO: Maybe should do something!!
                }

                _RuleDetail.LocationName = tempLocation.LocationName;
                OnPropertyChanged(nameof(RuleDetail));

                if (_RuleDetail.FirstCompareConditionList != null)
                {
                    var tempJsonBodyContent = JsonConvert.SerializeObject(_RuleDetail.FirstCompareConditionList);
                    var tempCompareConditionList = JsonConvert.DeserializeObject<List<CompareConditionEditModel>>(tempJsonBodyContent);
                    if (tempCompareConditionList != null)
                        FirstCompareConditionList = new ObservableCollection<CompareConditionEditModel>(tempCompareConditionList);
                }

                if (_RuleDetail.SecondCompareConditionList != null)
                {
                    var tempJsonBodyContent = JsonConvert.SerializeObject(_RuleDetail.SecondCompareConditionList);
                    var tempCompareConditionList = JsonConvert.DeserializeObject<List<CompareConditionEditModel>>(tempJsonBodyContent);
                    if (tempCompareConditionList != null)
                        SecondCompareConditionList = new ObservableCollection<CompareConditionEditModel>(tempCompareConditionList);
                }

                if (_RuleDetail.ActiveTimeList != null)
                {
                    var tempJsonBodyContent = JsonConvert.SerializeObject(_RuleDetail.ActiveTimeList);
                    var tempActiveTimeList = JsonConvert.DeserializeObject<List<ActiveTimeEditModel>>(tempJsonBodyContent);
                    if (tempActiveTimeList != null)
                        ActiveTimeList = new ObservableCollection<ActiveTimeEditModel>(tempActiveTimeList);
                }
            }
        }

        private async Task OkCommandAsync()
        {
            if (RuleDetail.IsValid)
            {
                RuleDetailEditModel temp = null;

                switch (_PageStatus)
                {
                    case PageStatus.Add:
                        {
                            temp = await _ruleDetailService.AddRuleDetailAsync(_RuleDetail);
                        }
                        break;
                    case PageStatus.Edit:
                        {
                            temp = await _ruleDetailService.EditRuleDetailAsync(_RuleDetail);
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            await _ruleDetailService.RemoveRuleDetailAsync(_RuleDetail);
                            temp = new RuleDetailEditModel();
                        }
                        break;
                }

                if (temp != null)
                {
                    RuleEditModel tempModel = new RuleEditModel();
                    tempModel.Id = _RuleDetail.RuleId;

                    var tempViewModel = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
                    App.CurrentView(tempViewModel);
                }
            }
        }

        private async Task CancelCommandAsync()
        {
            RuleEditModel tempModel = new RuleEditModel();
            tempModel.Id = _RuleDetail.RuleId;

            var tempViewModel = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
            App.CurrentView(tempViewModel);
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

            ActiveTimeVisibility = Visibility.Collapsed;
            LocationLookup.SetDataList(tempLookupList);
            LocationLookup.LookupVisibility = Visibility.Visible;
        }

        private async Task AddActiveTimeCommandAsync()
        {
            ActiveTimeEditModel tempModel = new ActiveTimeEditModel();
            tempModel.RuleDetailId = _RuleDetail.RuleDetailId;
            tempModel.ActivetimeId = Guid.NewGuid();
            tempModel.RuleId = _RuleDetail.RuleId;
            tempModel.VersionId = _RuleDetail.VersionId;

            var tempViewModel = new ActiveTimeCreateUpdateViewModel(tempModel, PageStatus.Add);
            App.CurrentView(tempViewModel);
        }

        private async Task EditActiveTimeCommandAsync()
        {
            if (_SelectedActiveTime != null)
            {
                //_SelectedActiveTime.RuleId = _RuleDetail.RuleId;
                var tempViewModel = new ActiveTimeCreateUpdateViewModel(_SelectedActiveTime, PageStatus.Edit);
                App.CurrentView(tempViewModel);
            }
        }

        private async Task DeleteActiveTimeCommandAsync()
        {
            if (_SelectedActiveTime != null)
            {
                //_SelectedActiveTime.RuleId = _RuleDetail.RuleId;
                var tempViewModel = new ActiveTimeCreateUpdateViewModel(_SelectedActiveTime, PageStatus.Delete);
                App.CurrentView(tempViewModel);
            }
        }


        private async Task AddFirstCompareConditionCommandAsync()
        {
            CompareConditionEditModel tempModel = new CompareConditionEditModel();
            tempModel.CompareConditionId = Guid.NewGuid();
            tempModel.RuleDetailId = _RuleDetail.RuleDetailId;
            tempModel.RuleId = _RuleDetail.RuleId;
            tempModel.IsBelongToFirstCompareCondition = true;
            tempModel.VersionId = _RuleDetail.VersionId;

            var tempViewModel = new CompareConditionCreateUpdateViewModel(tempModel, _RuleDetail.LocationId, PageStatus.Add);
            App.CurrentView(tempViewModel);
        }

        private async Task EditFirstCompareConditionCommandAsync()
        {
            if (_SelectedFirstCompareCondition != null)
            {
                var tempViewModel = new CompareConditionCreateUpdateViewModel(_SelectedFirstCompareCondition, _RuleDetail.LocationId, PageStatus.Edit);
                App.CurrentView(tempViewModel);
            }
        }

        private async Task DeleteFirstCompareConditionCommandAsync()
        {
            if (_SelectedFirstCompareCondition != null)
            {
                if (FirstCompareConditionList.Count == 1 && SecondCompareConditionList.Count == 0)
                {
                    var tempViewModel = new CompareConditionCreateUpdateViewModel(_SelectedFirstCompareCondition, _RuleDetail.LocationId, PageStatus.Delete);
                    App.CurrentView(tempViewModel);
                }
            }
        }


        private async Task AddSecondCompareConditionCommandAsync()
        {
            CompareConditionEditModel tempModel = new CompareConditionEditModel();
            tempModel.CompareConditionId = Guid.NewGuid();
            tempModel.RuleDetailId = _RuleDetail.RuleDetailId;
            tempModel.RuleId = _RuleDetail.RuleId;
            tempModel.IsBelongToFirstCompareCondition = false;
            tempModel.VersionId = _RuleDetail.VersionId;

            var tempViewModel = new CompareConditionCreateUpdateViewModel(tempModel, _RuleDetail.LocationId, PageStatus.Add);
            App.CurrentView(tempViewModel);
        }

        private async Task EditSecondCompareConditionCommandAsync()
        {
            if (_SelectedSecondCompareCondition != null)
            {
                var tempViewModel = new CompareConditionCreateUpdateViewModel(_SelectedSecondCompareCondition, _RuleDetail.LocationId, PageStatus.Edit);
                App.CurrentView(tempViewModel);
            }
        }

        private async Task DeleteSecondCompareConditionCommandAsync()
        {
            if (_SelectedSecondCompareCondition != null)
            {
                var tempViewModel = new CompareConditionCreateUpdateViewModel(_SelectedSecondCompareCondition, _RuleDetail.LocationId, PageStatus.Delete);
                App.CurrentView(tempViewModel);
            }
        }

        private void LocationLookup_OnSelectedData(LookupDataModel selectedData)
        {
            if (selectedData != null)
            {
                RuleDetail.LocationId = new Guid(selectedData.Id);
                RuleDetail.LocationName = selectedData.Name;

                OnPropertyChanged(nameof(RuleDetail));
                ActiveTimeVisibility = Visibility.Visible;
            }
        }

        #endregion

    }
}
