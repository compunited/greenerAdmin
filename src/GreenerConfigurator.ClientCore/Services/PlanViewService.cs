using Greener.Web.Definitions.API.Plans;
using Greener.Web.Definitions.API.Plans.Configurator.CreateDtos;
using Greener.Web.Definitions.API.Plans.Configurator.Overview;
using Greener.Web.Definitions.API.Plans.Configurator.UpdateDtos;
using Greener.Web.Definitions.API.Plans.Frontend.RequestParameter;
using Greener.Web.Definitions.API.Plans.PlanFrontend;
using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class PlanViewService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<PlanViewService> _logger;

        public PlanViewService(IApiService apiService, ILogger<PlanViewService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<PlanItem>> GetPlanViewByLocationIdAsync(GetPlanViewsByLocationIdParameter requestParameter)
        {
            List<PlanItem> result = null;

            try
            {
                string apiUrl = "api/1.0/PlanViewFrontend/GetPlanViewsByLocationId";
                var tempResult = await _apiService.SendPostRequestAsync(apiUrl, requestParameter);

                if (!string.IsNullOrEmpty(tempResult))
                {
                    result = JsonConvert.DeserializeObject<List<PlanItem>>(tempResult);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting plan views by location id");
                throw;
            }

            return result ?? new List<PlanItem>();
        }

        public async Task<PlanViewWithDataDto> GetPlanViewByRefId(GetPlanViewByPlanViewRefIdParameter requestParameter)
        {
            PlanViewWithDataDto result = null;

            string apiUrl = "api/1.0/PlanViewFrontend/GetPlanViewByPlanViewRefId";
            var tempResult = await _apiService.SendPostRequestAsync(apiUrl, requestParameter);

            if (!string.IsNullOrEmpty(tempResult))
            {
                result = JsonConvert.DeserializeObject<PlanViewWithDataDto>(tempResult);
            }

            return result;
        }

        public async Task<bool> CreatePlanViewAsync(PlanViewForCreateDto planViewCreate)
        {
            bool result = false;
            try
            {
                if (planViewCreate != null)
                {
                    string apiUrl = "api/1.0/PlanViewForGreenerConfigurator/CreateSavePlanView";
                    var tempResult = await _apiService.SendPostRequestAsync(apiUrl, planViewCreate);
                    if (!string.IsNullOrWhiteSpace(tempResult))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error creating plan view");
            }

            return result;
        }

        public async Task<bool> UpdatePlanViewAsync(PlanViewUpdateDto planViewUpdate)
        {
            bool result = false;
            try
            {
                if (planViewUpdate != null)
                {
                    string apiUrl = "api/1.0/PlanViewForGreenerConfigurator/UpdatePlanView";
                    var tempResult = await _apiService.SendPostRequestAsync(apiUrl, planViewUpdate);

                    result = true;
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error updating plan view");
            }

            return result;
        }
    }
}

