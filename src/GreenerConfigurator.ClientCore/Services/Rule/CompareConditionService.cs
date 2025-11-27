using GreenerConfigurator.ClientCore.Models.Rule;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Rule
{
    public class CompareConditionService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CompareConditionService> _logger;

        public CompareConditionService(IApiService apiService, ILogger<CompareConditionService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<CompareConditionEditModel> AddCompareConditionAsync(CompareConditionEditModel compareConditionEditModel)
        {
            string apiUrl = "/api/1.0/CompareCondition/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, compareConditionEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<CompareConditionEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<CompareConditionEditModel> EditCompareConditionAsync(CompareConditionEditModel compareConditionEditModel)
        {
            string apiUrl = "/api/1.0/CompareCondition/Edit";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, compareConditionEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<CompareConditionEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<CompareConditionEditModel> RemoveCompareConditionAsync(CompareConditionEditModel compareConditionEditModel)
        {
            string apiUrl = "/api/1.0/CompareCondition/Delete";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, compareConditionEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<CompareConditionEditModel>(jsonReq);
            }
            return null;
        }
    }
}

