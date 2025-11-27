using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.RuleSetting
{
    public class NewRuleSettingViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        public NewRuleSettingViewModel()
        {
            OnOpenCalendarCommand = new RelayCommand(OpenCalendarCommand);
        }

        #endregion

        #region  [ Public Property(s) ]

        public ICommand OnOpenCalendarCommand { get; private set; }

        public ICommand SelectCalendarCommand { get; private set; }

        public ICommand TapEventCommand { get; private set; }

        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                CheckedText = value ? "Enabled" : "Disabled";
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public string CheckedText
        {
            get => _CheckedText;
            set
            {
                _CheckedText = value;
                OnPropertyChanged(nameof(CheckedText));
            }
        }

        public int HeightCalendar
        {
            get => _HeightCalendar;
            set
            {
                _HeightCalendar = value;
                OnPropertyChanged(nameof(HeightCalendar));
            }
        }

        public int WidthCalendar
        {
            get => _WidthCalendar;
            set
            {
                _WidthCalendar = value;
                OnPropertyChanged(nameof(WidthCalendar));
            }
        }

        public Visibility CalendarVisibility
        {
            get => _CalendarVisibility;
            set
            {
                _CalendarVisibility = value;
                OnPropertyChanged(nameof(CalendarVisibility));
            }
        }

        public DateTime SelectedDate
        {
            get => _SelectedDate;
            set
            {
                _SelectedDate = value;
                SelectedDateText = _SelectedDate.ToString();
                HeightCalendar = 0;
                WidthCalendar = 0;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public string SelectedDateText
        {
            get => _SelectedDateText;
            set
            {
                _SelectedDateText = value;
                HeightCalendar = 0;
                WidthCalendar = 0;
                OnPropertyChanged(nameof(SelectedDateText));
            }
        }

        #endregion

        #region [ Private Method(s) ]

        private Visibility _CalendarVisibility = Visibility.Hidden;
        private DateTime _SelectedDate;
        private int _HeightCalendar = 0;
        private int _WidthCalendar = 0;
        private string _SelectedDateText = "  /  /  ";
        private bool _IsChecked = false;
        private string _CheckedText = "Disabled";

        #endregion

        #region [ Private Method(s) ]

        private void OpenCalendarCommand()
        {
            if (CalendarVisibility == Visibility.Hidden)
            {
                HeightCalendar = 180;
                WidthCalendar = 180;
                CalendarVisibility = Visibility.Visible;
            }
            else
            {
                HeightCalendar = 0;
                WidthCalendar = 0;
                CalendarVisibility = Visibility.Hidden;
            }
        }

        #endregion
    }
}
