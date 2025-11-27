using Greener.Web.Definitions.Api.Navigation;
using System.Collections.Generic;

namespace GreenerConfigurator.ClientCore.Models
{
    public class NavigationCardModel : LogicalDeviceNavigationCardDto
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
                catch
                {
                    temp = NavigationCategory.ToString();
                }

                return temp;
            }
        }

    }
}
