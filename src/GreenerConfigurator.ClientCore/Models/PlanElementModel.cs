using Greener.Web.Definitions.API.Plans;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenerConfigurator.ClientCore.Models
{
    public class PlanElementModel : PlanElement
    {
        #region [ Constructor(s) ]

        public PlanElementModel()
        {

        }

        public PlanElementModel(PlanElement planElement)
        {
            foreach (var prop in planElement.GetType().GetProperties())
            {
                this.GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(planElement, null), null);
            }
        }

        #endregion

        public string DeviceName { get; set; }
    }
}
