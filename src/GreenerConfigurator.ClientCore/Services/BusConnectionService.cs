
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class BusConnectionService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<BusConnectionService> _logger;

        public BusConnectionService(IApiService apiService, ILogger<BusConnectionService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<T> AddBusConnectionAsync<T>(T busConnectionEditModel)
        {
            string apiUrl = "api/1.0/BusConnection/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, busConnectionEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<T>(jsonReq);
            }
            return default(T);
        }

        public async Task<T> EditBusConnectionAsync<T>(T busConnectionEditModel)
        {
            string apiUrl = "api/1.0/BusConnection/Edit";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, busConnectionEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<T>(jsonReq);
            }
            return default(T);
        }
    }
}

