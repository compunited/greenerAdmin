using GreenerConfigurator.ClientCore.Models;
using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class PhysicalDeviceService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<PhysicalDeviceService> _logger;

        public PhysicalDeviceService(IApiService apiService, ILogger<PhysicalDeviceService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Public Method(s) ]

        public async Task<PhysicalDeviceModel> GetCurrentPhysicalDeviceByLogicalDeviceId(Guid logicalDeviceId)
        {
            string apiUrl = "api/1.0/PhysicalDevice/ByLogicalDeviceId/CurrentDevice";
            var physicalDeviceDeviceCardJson = await _apiService.SendGetRequestAsync(apiUrl, logicalDeviceId);

            if (!string.IsNullOrEmpty(physicalDeviceDeviceCardJson))
            {
                return JsonConvert.DeserializeObject<PhysicalDeviceModel>(physicalDeviceDeviceCardJson);
            }
            return null;
        }

        #endregion
    }
}

