using GreenerConfigurator.ClientCore.Models.Rule;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Rule
{
    public class ActiveTimeService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ActiveTimeService> _logger;

        public ActiveTimeService(IApiService apiService, ILogger<ActiveTimeService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<ActiveTimeEditModel> AddActiveTimeAsync(ActiveTimeEditModel activeTimeEditModel)
        {
            string apiUrl = "/api/1.0/ActiveTime/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, activeTimeEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<ActiveTimeEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<ActiveTimeEditModel> EditActiveTimeAsync(ActiveTimeEditModel activeTimeEditModel)
        {
            string apiUrl = "/api/1.0/ActiveTime/Edit";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, activeTimeEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<ActiveTimeEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<ActiveTimeEditModel> RemoveActiveTimeAsync(ActiveTimeEditModel activeTimeEditModel)
        {
            string apiUrl = "/api/1.0/ActiveTime/Delete";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, activeTimeEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<ActiveTimeEditModel>(jsonReq);
            }
            return null;
        }
    }
}

