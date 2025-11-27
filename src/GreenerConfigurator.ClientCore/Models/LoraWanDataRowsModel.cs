using Greener.Web.Definitions.Api.LoraWanDataRow;
using Greener.Web.Definitions.Enums;

namespace GreenerConfigurator.ClientCore.Models
{
    public class LoraWanDataRowsModel : LoraWanDataRowsDto
    {
        public string UnitOfMeasurementName
        {
            get
            {
                var temp = string.Empty;
                try
                {
                    temp = UnitOfMeasurement.GetDisplayAttributeName();
                }
                catch { temp = UnitOfMeasurement.ToString(); }

                return temp;
            }
        }
    }
}
