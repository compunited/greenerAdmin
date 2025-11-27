using Greener.Web.Definitions.Api.Navigation;
using Greener.Web.Definitions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenerConfigurator.ClientCore.Models
{
    public class NavigationCategoryModel : LogicalDeviceNavigationCategoryDto
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
