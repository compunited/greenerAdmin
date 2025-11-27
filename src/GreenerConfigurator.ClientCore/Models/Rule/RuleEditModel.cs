using Greener.Web.Definitions.API.Rule;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models.Rule
{
    public class RuleEditModel : RuleEditDto, IDataErrorInfo
    {
        #region [ Constructor(s) ]

        public RuleEditModel()
        {
            ErrorList = new ObservableCollection<string>();
            Hour = 0;
            Minute = 0;
        }

        #endregion

        #region [ Public Property(s) ]

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

        public bool PauseRule
        {
            get => _PauseRule;
            set
            {
                _PauseRule = value;
                if (!_PauseRule)
                {
                    PauseUntil = null;
                    Hour = 0;
                    Minute = 0;
                }
                else
                    ValidateProperty(nameof(PauseRule));
            }
        }

        public int Hour
        {
            get
            {
                if (PauseUntil.HasValue)
                    _Hour = PauseUntil.Value.Hour;
                return _Hour;
            }
            set
            {
                if (!PauseUntil.HasValue || value < 0 || value > 23)
                    _Hour = 0;
                else
                    _Hour = value;
            }
        }

        public int Minute
        {
            get
            {
                if (PauseUntil.HasValue)
                    _Minute = PauseUntil.Value.Minute;

                return _Minute;
            }
            set
            {
                if (!PauseUntil.HasValue || value < 0 || value > 59)
                    _Minute = 0;
                else
                    _Minute = value;
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
        private bool _PauseRule = false;
        private int _Minute;
        private int _Hour;

        #endregion

        #region [ Private Property(s) ]

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                _Error = ValidateProperty(columnName);
                if (columnName == nameof(PauseUntil) ||
                    columnName == nameof(Hour) ||
                    columnName == nameof(Minute))
                    _Error = string.Empty;

                return _Error;
            }
        }

        #endregion

        #region [ Private Method(s) ] 

        private string ValidateProperty(string propertyName)
        {
            string tempError = string.Empty;

            if (propertyName == nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    tempError = Language.RuleNameCouldNotBeEmpty;
                }
                else
                    ErrorList.Remove(Language.RuleNameCouldNotBeEmpty);
            }


            if (propertyName == nameof(MessageText))
            {
                if (string.IsNullOrWhiteSpace(MessageText))
                {
                    tempError = Language.RuleMessageCouldNotBeEmpty;
                }
                else
                    ErrorList.Remove(Language.RuleMessageCouldNotBeEmpty);
            }

            if (propertyName == nameof(PauseUntil) ||
                propertyName == nameof(Hour) ||
                propertyName == nameof(Minute) ||
                propertyName == nameof(PauseRule))
            {
                if (PauseRule && !PauseUntil.HasValue)
                {
                    tempError = Language.PauseUntilCouldNotBeNull;
                }
                else if (!ValidateDateTime())
                {
                    ErrorList.Remove(Language.PauseUntilCouldNotBeNull);
                    tempError = Language.PauseUntilCouldNotBeAsNow;
                }
                else
                {
                    ErrorList.Remove(Language.PauseUntilCouldNotBeAsNow);
                    ErrorList.Remove(Language.PauseUntilCouldNotBeNull);
                }
            }

            //if (propertyName == nameof(Hour) || propertyName == nameof(Minute))
            //{
            //    if (!ValidateDateTime())
            //    {
            //        tempError = Language.PauseUntilCouldNotBeAsNow;
            //    }
            //    else
            //    {
            //        ErrorList.Remove(Language.PauseUntilCouldNotBeAsNow);
            //    }
            //}

            //if (propertyName == nameof(PauseRule))
            //{
            //    if (PauseRule && !PauseUntil.HasValue)
            //    {
            //        tempError = Language.PauseUntilCouldNotBeNull;
            //    }
            //    else if (!ValidateDateTime())
            //    {
            //        tempError = Language.PauseUntilCouldNotBeAsNow;
            //    }
            //    else
            //    {
            //        ErrorList.Remove(Language.PauseUntilCouldNotBeAsNow);
            //        ErrorList.Remove(Language.PauseUntilCouldNotBeNull);
            //    }
            //}


            if (!string.IsNullOrWhiteSpace(tempError) && ErrorList.Where(w => w == tempError).Count() == 0)
                ErrorList.Add(tempError);

            return tempError;
        }

        private bool ValidateDateTime()
        {
            bool result = true;
            if (PauseUntil.HasValue)
            {
                DateTime temp = new DateTime(PauseUntil.Value.Year, PauseUntil.Value.Month, PauseUntil.Value.Day, Hour, Minute, 0);
                if (temp <= DateTime.Now.AddMinutes(-1))
                    result = false;
                else
                    PauseUntil = temp;
            }

            return result;
        }

        #endregion
    }
}
