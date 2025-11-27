using GreenerConfigurator.Controls.Lookup;
using GreenerConfigurator.ClientCore.Models.Rule;
using GreenerConfigurator.ClientCore.Services.Notification;
using GreenerConfigurator.ClientCore.Services.Rule;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.Rule
{
    public class NotificationGroupDataCreateUpdateViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly NotificationGroupDataService _notificationGroupDataService;
        private readonly NotificationGroupService _notificationGroupService;

        public NotificationGroupDataCreateUpdateViewModel(PageStatus pageStatus, NotificationGroupDataEditModel notificationGroupDataEditModel)
        {
            _notificationGroupDataService = App.ServiceProvider.GetRequiredService<NotificationGroupDataService>();
            _notificationGroupService = App.ServiceProvider.GetRequiredService<NotificationGroupService>();

            NotificationGroupDataModel = notificationGroupDataEditModel;
            PageStatus = pageStatus;

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);
            OnNotificationGroupLookupCommand = new AsyncRelayCommand(NotificationGroupLookupCommandAsync);

            LocationLookup = new LookupViewModel();
            LocationLookup.OnSelectedData += LocationLookup_OnSelectedData;
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnOKCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnNotificationGroupLookupCommand { get; private set; }

        public NotificationGroupDataEditModel NotificationGroupDataModel
        {
            get => _NotificationGroupDataModel;
            set
            {
                _NotificationGroupDataModel = value;
                OnPropertyChanged(nameof(NotificationGroupDataModel));
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

        #endregion

        #region [ Private Field(s) ]

        private PageStatus _PageStatus;
        private bool _IsEditMode = false;
        private string _ButtonOkText = Language.Save;
        private NotificationGroupDataEditModel _NotificationGroupDataModel = null;
        private bool _IsLookupDeactivated = false;

        #endregion

        #region [ Private Method ]

        private async Task ManagePageStatusAsync()
        {
            switch (_PageStatus)
            {
                case PageStatus.Add:
                case PageStatus.Edit:
                    {
                        IsEditMode = true;
                        ButtonOkText = Language.Save;
                    }
                    break;
                case PageStatus.Delete:
                    {
                        IsEditMode = false;
                        ButtonOkText = Language.Delete;
                    }
                    break;
            }
        }

        private async Task OkCommandAsync()
        {
            if (_NotificationGroupDataModel.IsValid)
            {
                NotificationGroupDataEditModel tempResponse = null;
                switch (_PageStatus)
                {
                    case PageStatus.Add:
                        {
                            tempResponse = await _notificationGroupDataService.AddNotificationGroupDataAsync(_NotificationGroupDataModel);
                        }
                        break;
                    case PageStatus.Edit:
                        {
                            tempResponse = await _notificationGroupDataService.EditNotificationGroupDataAsync(_NotificationGroupDataModel);
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            await _notificationGroupDataService.RemoveNotificationGroupDataAsync(_NotificationGroupDataModel);
                            tempResponse = new NotificationGroupDataEditModel();
                        }
                        break;
                }

                if (tempResponse != null)
                {
                    RuleEditModel tempModel = new RuleEditModel();
                    tempModel.Id = _NotificationGroupDataModel.RuleId;

                    var tempViewModel = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
                    App.CurrentView(tempViewModel);
                }
            }
        }

        private async Task CancelCommandAsync()
        {
            RuleEditModel tempModel = new RuleEditModel();
            tempModel.Id = _NotificationGroupDataModel.RuleId;

            var tempViewModel = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
            App.CurrentView(tempViewModel);
        }

        private async Task NotificationGroupLookupCommandAsync()
        {
            var notificationGroupList = await _notificationGroupService.GetNotificationGroupAsync();

            if (notificationGroupList!=null)
            {
                List<LookupDataModel> tempLookupList = new List<LookupDataModel>();

                foreach (var item in notificationGroupList)
                {
                    tempLookupList.Add(
                        new LookupDataModel()
                        {
                            Id = item.Id.ToString(),
                            Name = item.Name
                        }
                    );
                }
                LocationLookup.SetDataList(tempLookupList);

                IsLookupDeactivated = false;
                LocationLookup.LookupVisibility = Visibility.Visible;
            }
        }

        private void LocationLookup_OnSelectedData(LookupDataModel selectedData)
        {
            if (selectedData != null)
            {
                _NotificationGroupDataModel.NotificationGroupId = new Guid(selectedData.Id);
                _NotificationGroupDataModel.NotificationGroupName = selectedData.Name;

                OnPropertyChanged(nameof(NotificationGroupDataModel));
            }

            IsLookupDeactivated = true;
        }


        #endregion
    }
}
