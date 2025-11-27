using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using GreenerConfigurator.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace GreenerConfigurator.Controls.Lookup
{
    public class LookupViewModel : ViewModelBase
    {

        #region [ Constructor(s) ]

        public LookupViewModel()
        {
            OnSelectCommand = new RelayCommand(SelectCommand);
            OnCancelCommand = new RelayCommand(CancelCommand);
            OnSortDataCommand = new RelayCommand<string>(SortDataCommand);
            OnSearchTextChangeCommand = new RelayCommand(SearchTextChangeCommand);
            OnClearSearchText = new RelayCommand(ClearSearchText);
        }

        #endregion

        #region [ Public Property(s) ]

        public delegate void LookupSelectedDataHandler(LookupDataModel selectedData);

        public event LookupSelectedDataHandler OnSelectedData;

        #region [ Command(s)  ]

        public ICommand OnSelectCommand { get; private set; }

        public ICommand OnCancelCommand { get; private set; }

        public ICommand OnSortDataCommand { get; private set; }

        public ICommand OnSearchTextChangeCommand { get; private set; }

        public ICommand OnClearSearchText { get; private set; }

        #endregion

        public Visibility LookupVisibility
        {
            get => _LookupVisibility;
            set
            {
                _LookupVisibility = value;
                if (_LookupVisibility == Visibility.Hidden)
                    CurrentHeight = "0";
                else
                    CurrentHeight = "Auto";

                OnPropertyChanged(nameof(LookupVisibility));
            }
        }

        public string CurrentHeight
        {
            get => _CurrentHeight;
            set
            {
                _CurrentHeight = value;
                OnPropertyChanged(nameof(CurrentHeight));
            }
        }
        public ObservableCollection<LookupDataModel> LookupDataList
        {
            get => _LookupDataList;
            set
            {
                _LookupDataList = value;
                OnPropertyChanged(nameof(LookupDataList));
            }
        }

        public LookupDataModel SelectedLookupData
        {
            get => _SelectedLookupData;
            set
            {
                _SelectedLookupData = value;
                OnPropertyChanged(nameof(SelectedLookupData));
            }
        }

        public string SearchText
        {
            get => _SearchText;
            set
            {
                _SearchText = value;
                SearchTextChangeCommand();
                OnPropertyChanged(nameof(SearchText));
            }
        }

        #endregion

        #region [ Public Method(s) ]

        public void SetDataList(List<LookupDataModel> datalist)
        {
            if (datalist != null)
            {
                mainDataList = datalist;
                LookupDataList = new ObservableCollection<LookupDataModel>(mainDataList);
            }
        }

        #endregion

        #region [ Private Field(s) ]

        //private bool _IsLookupEnable = true;

        private Visibility _LookupVisibility = Visibility.Hidden;

        private ObservableCollection<LookupDataModel> _LookupDataList = null;

        private List<LookupDataModel> mainDataList = null;

        private LookupDataModel _SelectedLookupData = null;

        private string _SearchText = string.Empty;

        private bool previousSortAsc = true;

        private string _CurrentHeight;

        #endregion

        #region [ Private Method(s) ]

        private void SelectCommand()
        {
            if (SelectedLookupData != null)
            {
                if (OnSelectedData != null)
                    OnSelectedData(SelectedLookupData);

                LookupVisibility = Visibility.Hidden;
                SearchText = string.Empty;
            }
        }

        private void CancelCommand()
        {
            if (OnSelectedData != null)
                OnSelectedData(null);

            LookupVisibility = Visibility.Hidden;
            SearchText = string.Empty;
        }

        private void SearchTextChangeCommand()
        {
            if (mainDataList != null)
            {
                List<LookupDataModel> tempList = mainDataList;

                if (!string.IsNullOrWhiteSpace(SearchText))
                    tempList = mainDataList.Where(w => w.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

                LookupDataList = new ObservableCollection<LookupDataModel>(tempList);
            }
        }

        private void SortDataCommand(string parameter)
        {
            List<LookupDataModel> tempList = null;
            Type type = typeof(LookupDataModel);

            var propertyInfo = type.GetProperty(parameter);

            if (previousSortAsc)
                tempList = LookupDataList.OrderByDescending(o => propertyInfo.GetValue(o, null)).ToList();
            else
                tempList = LookupDataList.OrderBy(o => propertyInfo.GetValue(o, null)).ToList();

            previousSortAsc = !previousSortAsc;

            LookupDataList = new ObservableCollection<LookupDataModel>(tempList);
        }

        private void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        #endregion
    }
}
