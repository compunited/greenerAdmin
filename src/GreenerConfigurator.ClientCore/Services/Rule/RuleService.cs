using Greener.Web.Definitions.API.Rule;
using GreenerConfigurator.ClientCore.Models.Rule;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Rule
{
    public class RuleService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<RuleService> _logger;

        public RuleService(IApiService apiService, ILogger<RuleService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<RuleViewDto>> GetAllRulesAsync()
        {
            List<RuleViewDto> result = null;

            string apiUrl = "/api/1.0/Rule/GetAll";

            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                result = JsonConvert.DeserializeObject<List<RuleViewDto>>(jsonReq);
            }

            return result ?? new List<RuleViewDto>();
        }

        public async Task<RuleEditModel> GetRuleByRuleIdAsync(Guid ruleId)
        {
            RuleEditModel temp = null;

            string apiUrl = "/api/1.0/Rule/GetRuleByRuleId";

            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl, ruleId);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                temp = JsonConvert.DeserializeObject<RuleEditModel>(jsonReq);
            }

            return temp;
        }

        public async Task<RuleEditModel> AddRuleAsync(RuleEditDto ruleModel)
        {
            RuleEditModel result = null;

            string apiUrl = "/api/1.0/Rule/Add";

            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, ruleModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                result = JsonConvert.DeserializeObject<RuleEditModel>(jsonReq);
            }

            return result;
        }

        public async Task<RuleEditModel> EditRuleAsync(RuleEditDto ruleModel)
        {
            RuleEditModel result = null;

            string apiUrl = "/api/1.0/Rule/Edit";

            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, ruleModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                result = JsonConvert.DeserializeObject<RuleEditModel>(jsonReq);
            }

            return result;
        }

        public async Task RemoveRuleAsync(RuleEditDto ruleModel)
        {
            string apiUrl = "api/1.0/Rule/Delete";
            await _apiService.SendPostRequestAsync(apiUrl, ruleModel);
        }
    }
}

