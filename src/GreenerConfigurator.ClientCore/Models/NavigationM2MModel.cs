using Greener.Web.Definitions.API.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models
{
    public class NavigationM2MModel : LogicalDeviceNavigationM2MViewDto
    {
        public string UnitMeasurmentName
        {
            get
            {
                string tempName = string.Empty;

                try
                {
                    tempName = UnitOfMeasurement.GetDisplayAttributeName();
                }
                catch { tempName = UnitOfMeasurement.ToString(); }

                return tempName;
            }
        }

        public decimal LatestValue { get; set; }
    }
}
