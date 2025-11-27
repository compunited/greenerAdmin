using GreenerConfigurator.ClientCore.Models.Rule;

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Rule
{
    public class RuleDetailService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<RuleDetailService> _logger;

        public RuleDetailService(IApiService apiService, ILogger<RuleDetailService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<RuleDetailEditModel> GetRuleDetailByRuleDetailIdAsync(Guid ruleDetailId)
        {
            string apiUrl = "/api/1.0/RuleDetail/GetRuleByRuleId";
            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl, ruleDetailId);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<RuleDetailEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<RuleDetailEditModel> AddRuleDetailAsync(RuleDetailEditModel ruleDetailEdit)
        {
            string apiUrl = "/api/1.0/RuleDetail/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, ruleDetailEdit);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<RuleDetailEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<RuleDetailEditModel> EditRuleDetailAsync(RuleDetailEditModel ruleDetailEdit)
        {
            string apiUrl = "/api/1.0/RuleDetail/Edit";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, ruleDetailEdit);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<RuleDetailEditModel>(jsonReq);
            }
            return null;
        }

        public async Task RemoveRuleDetailAsync(RuleDetailEditModel ruleDetailEdit)
        {
            string apiUrl = "/api/1.0/RuleDetail/Delete";
            await _apiService.SendPostRequestAsync(apiUrl, ruleDetailEdit);
        }
    }
}

