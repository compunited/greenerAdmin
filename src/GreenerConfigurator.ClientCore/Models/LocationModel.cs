using Greener.Web.Definitions.Api.MasterData.Location;
using Greener.Web.Definitions.Enums;


namespace GreenerConfigurator.ClientCore.Models
{
    public class LocationModel : LocationDto
    {
        
        public string LocationTypeName
        {
            get
            {
                var temp = string.Empty;

                try
                {
                    temp = LocationType.GetDisplayAttributeName();
                }
                catch
                {
                    temp = LocationType.ToString();
                }

                return temp;
            }
        }

    }
}
