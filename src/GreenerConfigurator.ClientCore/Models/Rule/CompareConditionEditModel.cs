using Greener.Web.Definitions.API.Rule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models.Rule
{
    public class CompareConditionEditModel : CompareConditionEditDto, IDataErrorInfo
    {

        #region [ Constructor(s) ]

        public CompareConditionEditModel()
        {
            ErrorList = new ObservableCollection<string>();
        }

        #endregion

        #region [ Public Property(s) ]
        
        public string CompareOperatorTypeName
        {
            get
            {
                string tempName = string.Empty;

                try
                {
                    tempName = CompareOperatorType.GetDisplayAttributeName();
                }
                catch { tempName = CompareOperatorType.ToString(); }

                return tempName;
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
