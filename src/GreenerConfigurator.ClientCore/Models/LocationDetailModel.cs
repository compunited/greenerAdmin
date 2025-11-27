using Greener.Web.Definitions.Api.MasterData.Location;

namespace GreenerConfigurator.ClientCore.Models
{
    public class LocationDetailModel : LocationDetailDto
    {

        public string LocationDetailTypeName
        {
            get
            {
                var temp = string.Empty;

                try
                {
                    temp = LocationDetailType.GetDisplayAttributeName();
                }
                catch
                {
                    temp = LocationDetailType.ToString();
                }

                return temp;
            }
        }

        public bool AddAsRoot { get; set; }
    }
}
