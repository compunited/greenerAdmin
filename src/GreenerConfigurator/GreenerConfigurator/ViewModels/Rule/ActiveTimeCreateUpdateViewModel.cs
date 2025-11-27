using GreenerConfigurator.ClientCore.Models.Rule;
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
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.Rule
{
    public class ActiveTimeCreateUpdateViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly ActiveTimeService _activeTimeService;

        public ActiveTimeCreateUpdateViewModel(ActiveTimeEditModel activeTimeEdit, PageStatus pageStatus)
        {
            _activeTimeService = App.ServiceProvider.GetRequiredService<ActiveTimeService>();
            ActiveTimeModel = activeTimeEdit;
            PageStatus = pageStatus;

            OnOKCommand = new AsyncRelayCommand(OkCommandAsync);
            OnCancelCommand = new AsyncRelayCommand(CancelCommandAsync);

        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnOKCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

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

        public ActiveTimeEditModel ActiveTimeModel
        {
            get => _ActiveTimeModel;
            set
            {
                _ActiveTimeModel = value;

                if (_ActiveTimeModel.FromTime.HasValue)
                    _ActiveTimeModel.StartTime = new DateTime() + _ActiveTimeModel.FromTime;
                if (_ActiveTimeModel.ToTime.HasValue)
                    _ActiveTimeModel.EndTime = new DateTime() + _ActiveTimeModel.ToTime;

                OnPropertyChanged(nameof(ActiveTimeModel));
            }
        }

        public bool AllDaysSelected
        {
            get => _AllDaysSelected;
            set
            {
                _AllDaysSelected = value;
                OnPropertyChanged(nameof(AllDaysSelected));
                SelectAllDaysAsync();
            }
        }
        public bool MondaySelected
        {
            get => _MondaySelected;
            set
            {
                _MondaySelected = value;
                OnPropertyChanged(nameof(MondaySelected));
                SelectDayAsync(DayOfWeek.Monday);
            }
        }
        public bool TuesdaySelected
        {
            get => _TuesdaySelected;
            set
            {
                _TuesdaySelected = value;
                OnPropertyChanged(nameof(TuesdaySelected));
                SelectDayAsync(DayOfWeek.Tuesday);
            }
        }
        public bool WednesdaySelected
        {
            get => _WednesdaySelected;
            set
            {
                _WednesdaySelected = value;
                OnPropertyChanged(nameof(WednesdaySelected));
                SelectDayAsync(DayOfWeek.Wednesday);
            }
        }
        public bool ThursdaySelected
        {
            get => _ThursdaySelected;
            set
            {
                _ThursdaySelected = value;
                OnPropertyChanged(nameof(ThursdaySelected));
                SelectDayAsync(DayOfWeek.Thursday);
            }
        }
        public bool FridaySelected
        {
            get => _FridaySelected;
            set
            {
                _FridaySelected = value;
                OnPropertyChanged(nameof(FridaySelected));
                SelectDayAsync(DayOfWeek.Friday);
            }
        }
        public bool SaturdaySelected
        {
            get => _SaturdaySelected;
            set
            {
                _SaturdaySelected = value;
                OnPropertyChanged(nameof(SaturdaySelected));
                SelectDayAsync(DayOfWeek.Saturday);
            }
        }
        public bool SundaySelected
        {
            get => _SundaySelected;
            set
            {
                _SundaySelected = value;
                OnPropertyChanged(nameof(SundaySelected));
                SelectDayAsync(DayOfWeek.Sunday);
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private PageStatus _PageStatus;
        private string _ButtonOkText = Language.Save;
        private bool _IsEditMode;
        private ActiveTimeEditModel _ActiveTimeModel;
        private bool _AllDaysSelected;
        private bool _TuesdaySelected;
        private bool _MondaySelected;
        private bool _WednesdaySelected;
        private bool _ThursdaySelected;
        private bool _FridaySelected;
        private bool _SaturdaySelected;
        private bool _SundaySelected;
        private bool allowProcess = true;

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
            ManageWeekDays();
        }

        private void ManageWeekDays()
        {
            if (_ActiveTimeModel != null && _ActiveTimeModel.DaysOfWeek != null)
            {
                allowProcess = false;

                if (_ActiveTimeModel.DaysOfWeek.Count == 7)
                {
                    AllDaysSelected = true;
                }
                else
                {
                    foreach (var dayOfWeek in _ActiveTimeModel.DaysOfWeek)
                    {
                        switch (dayOfWeek)
                        {
                            case DayOfWeek.Sunday:
                                SundaySelected = true;
                                break;
                            case DayOfWeek.Monday:
                                MondaySelected = true;
                                break;
                            case DayOfWeek.Tuesday:
                                TuesdaySelected = true;
                                break;
                            case DayOfWeek.Wednesday:
                                WednesdaySelected = true;
                                break;
                            case DayOfWeek.Thursday:
                                ThursdaySelected = true;
                                break;
                            case DayOfWeek.Friday:
                                FridaySelected = true;
                                break;
                            case DayOfWeek.Saturday:
                                SaturdaySelected = true;
                                break;
                        }
                    }
                }

                allowProcess = true;
            }
        }

        private async Task OkCommandAsync()
        {
            if (_ActiveTimeModel.IsValid)
            {
                ActiveTimeEditModel result = null;
                switch (_PageStatus)
                {

                    case PageStatus.Add:
                        {
                            result = await _activeTimeService.AddActiveTimeAsync(_ActiveTimeModel);
                        }
                        break;
                    case PageStatus.Edit:
                        {
                            result = await _activeTimeService.EditActiveTimeAsync(_ActiveTimeModel);
                        }
                        break;
                    case PageStatus.Delete:
                        {
                            await _activeTimeService.RemoveActiveTimeAsync(_ActiveTimeModel);
                            result = new ActiveTimeEditModel();
                        }
                        break;
                }

                if (result != null)
                {
                    var tempViewModel = new RuleDetailCreateUpdateViewModel(_ActiveTimeModel.RuleId, _ActiveTimeModel.RuleDetailId);
                    App.CurrentView(tempViewModel);
                }
            }
        }

        private async Task CancelCommandAsync()
        {
            var tempViewModel = new RuleDetailCreateUpdateViewModel(_ActiveTimeModel.RuleId, _ActiveTimeModel.RuleDetailId);
            App.CurrentView(tempViewModel);
        }

        private async Task SelectAllDaysAsync()
        {
            _ActiveTimeModel.DaysOfWeek.Clear();
            if (_AllDaysSelected)
            {
                allowProcess = false;
                foreach (DayOfWeek item in Enum.GetValues(typeof(DayOfWeek)))
                {
                    _ActiveTimeModel.DaysOfWeek.Add(item);
                }
                MondaySelected = TuesdaySelected = WednesdaySelected = ThursdaySelected = FridaySelected = SaturdaySelected = SundaySelected = !_AllDaysSelected;
                allowProcess = true;
            }
        }

        private async Task SelectDayAsync(DayOfWeek dayOfWeek)
        {
            if (allowProcess)
            {
                if (_AllDaysSelected)
                {
                    _ActiveTimeModel.DaysOfWeek.Clear();
                    _AllDaysSelected = false;
                    OnPropertyChanged(nameof(AllDaysSelected));
                }
                _ActiveTimeModel.DaysOfWeek.Add(dayOfWeek);
            }
        }

        #endregion

    }
}
