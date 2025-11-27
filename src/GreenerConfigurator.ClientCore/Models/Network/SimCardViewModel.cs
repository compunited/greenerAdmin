using Greener.Web.Definitions.API.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models.Network
{
    public class SimCardViewModel : SimCardViewDto
    {

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

    }
}
