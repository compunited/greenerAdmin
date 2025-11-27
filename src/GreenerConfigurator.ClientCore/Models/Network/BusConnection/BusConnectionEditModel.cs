using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Greener.Web.Definitions.API.Network.BusConnection;
using Greener.Web.Definitions.Enums.Networks;

namespace GreenerConfigurator.ClientCore.Models.Network.BusConnection
{
    public class BusConnectionEditModel : BusConnectionBaseEditDto
    {
        #region [ Public Property(s) ]

        public string StateName
        {
            get
            {
                string tempName = string.Empty;

                try
                {
                    tempName = State.GetDisplayAttributeName();
                }
                catch { tempName = State.ToString(); }

                return tempName;
            }
        }

        #endregion
    }
}
