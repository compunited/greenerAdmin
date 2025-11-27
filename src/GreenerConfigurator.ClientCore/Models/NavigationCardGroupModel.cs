using Greener.Web.Definitions.API.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models
{
    public class NavigationCardGroupModel : LogicalDeviceNavigationCardGroupDto
    {
        public string NavigationCategoryName
        {
            get
            {
                var temp = string.Empty;

                try
                {
                    temp = NavigationCategory.GetDisplayAttributeName();
                }
                catch { temp = NavigationCategory.ToString(); }

                return temp;
            }
        }

    }
}
