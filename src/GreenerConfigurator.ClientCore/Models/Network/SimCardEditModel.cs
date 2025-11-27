using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Greener.Web.Definitions.API.Network;
using GreenerConfigurator.ClientCore.Utilities.MUI;

namespace GreenerConfigurator.ClientCore.Models.Network
{
    public class SimCardEditModel : SimCardEditDto, IDataErrorInfo
    {
        //TODO: All models should be follow this validation structure
        #region [ Constructor(s) ]

        public SimCardEditModel()
        {
            _ErrorList = new ObservableCollection<string>();
            SimCardState = Greener.Web.Definitions.Enums.Networks.SimCardState.Active;;
            ValidToDate = DateTime.Today.AddDays(1);
        }

        #endregion

        #region [ Public Property(s) ]

        public string SimCardStateName
        {
            get
            {
                string tempName = string.Empty;

                try
                {
                    tempName = SimCardState.GetDisplayAttributeName();
                }
                catch { tempName = SimCardState.ToString(); }

                return tempName;
            }
        }

        public string Error { get => _Error; }

        public bool IsValid
        {
            get
            {
                bool result = false;
                //  if (string.IsNullOrEmpty(_Error))
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

            if (propertyName == nameof(ICCDID))
            {
                if (string.IsNullOrWhiteSpace(ICCDID))
                {
                    tempError = Language.IccdIdIsNotValid;
                }
                else
                    ErrorList.Remove(Language.IccdIdIsNotValid);
            }

            if (propertyName == nameof(SimCardNumber))
            {
                if (string.IsNullOrWhiteSpace(SimCardNumber))
                {
                    tempError = Language.SimCardNumberIsNotValid;
                }
                else
                    ErrorList.Remove(Language.SimCardNumberIsNotValid);
            }

            if (!string.IsNullOrWhiteSpace(tempError) && ErrorList.Where(w => w == tempError).Count() == 0)
                ErrorList.Add(tempError);

            return tempError;
        }

        #endregion
    }
}
