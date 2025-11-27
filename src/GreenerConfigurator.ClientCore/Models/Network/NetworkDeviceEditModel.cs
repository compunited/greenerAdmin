using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Greener.Web.Definitions.API.Network;

namespace GreenerConfigurator.ClientCore.Models.Network
{
    public class NetworkDeviceEditModel : NetworkDeviceEditDto, IDataErrorInfo
    {
        public NetworkDeviceEditModel()
        {
            LocationName = string.Empty;
            LocationDetailName = string.Empty;
            ErrorList = new ObservableCollection<string>();
        }

        #region [ Public Property(s) ]

        public string NetworkDeviceTypeName
        {
            get
            {
                string tempName = string.Empty;

                try
                {
                    tempName = NetworkDeviceType.GetDisplayAttributeName();
                }
                catch { tempName = NetworkDeviceType.ToString(); }

                return tempName;
            }
        }

        public string LocationName { get; set; }

        public string LocationDetailName { get; set; }

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
