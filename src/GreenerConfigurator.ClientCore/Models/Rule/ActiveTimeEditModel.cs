using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Greener.Web.Definitions.API.Rule;

namespace GreenerConfigurator.ClientCore.Models.Rule
{
    public class ActiveTimeEditModel : ActiveTimeEditDto, IDataErrorInfo
    {
        #region [ Constructor(s) ]

        public ActiveTimeEditModel()
        {
            ErrorList = new ObservableCollection<string>();
            DaysOfWeek = new List<DayOfWeek>();
        }

        #endregion

        #region [ Public Property(s) ]

        [JsonIgnore]
        public DateTime? EndTime
        {
            get => _EndTime;
            set
            {
                _EndTime = value;
                if (value.HasValue)
                    ToTime = value.Value.TimeOfDay;
            }
        }

        [JsonIgnore]
        public DateTime? StartTime
        {
            get => _StartTime;
            set
            {
                _StartTime = value;
                if (value.HasValue)
                    FromTime = value.Value.TimeOfDay;
            }
        }

        public string Error { get => _Error; }

        public bool IsValid
        {
            get
            {
                bool result = false;
                if (ErrorList.Count() == 0)
                    result = true;

                return result;
            }
        }

        public ObservableCollection<string> ErrorList
        {
            get => _ErrorList;
            set
            {
                _ErrorList = value;
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private string _Error;
        private ObservableCollection<string> _ErrorList;
        private DateTime? _StartTime;
        private DateTime? _EndTime;

        #endregion

        #region [ Private Property(s) ]

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                _Error = ValidateProperty(columnName);
                return _Error;
            }
        }

        #endregion

        #region [ Private Method(s) ] 

        private string ValidateProperty(string propertyName)
        {
            string tempError = string.Empty;

            //if (propertyName == nameof(Name))
            //{
            //    if (string.IsNullOrWhiteSpace(Name))
            //    {
            //        tempError = Language.NetworkDeviceNameNotBeEmpty;
            //    }
            //    else
            //        ErrorList.Remove(Language.NetworkDeviceNameNotBeEmpty);
            //}

            if (!string.IsNullOrWhiteSpace(tempError) && ErrorList.Where(w => w == tempError).Count() == 0)
                ErrorList.Add(tempError);

            return tempError;
        }

        #endregion
    }
}
