using Greener.Web.Definitions.API.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models.Network
{
    public class NetworkDeviceViewModel : NetworkDeviceViewDto
    {

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

    }
}
