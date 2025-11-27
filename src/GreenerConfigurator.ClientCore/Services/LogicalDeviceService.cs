using GreenerConfigurator.ClientCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class LogicalDeviceService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LogicalDeviceService> _logger;

        public LogicalDeviceService(IApiService apiService, ILogger<LogicalDeviceService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Public Method(s) ]

        public async Task<List<LogicalDeviceModel>> GetLogicalDeviceListByLocationId(Guid locationId)
        {
            string apiUrl = "api/1.0/LogicalDevice/ByLocationId";
            var logicalDeviceCardJson = await _apiService.SendGetRequestAsync(apiUrl, locationId);

            if (!string.IsNullOrEmpty(logicalDeviceCardJson))
            {
                return JsonConvert.DeserializeObject<List<LogicalDeviceModel>>(logicalDeviceCardJson);
            }
            return new List<LogicalDeviceModel>();
        }

        public async Task<List<LogicalDeviceModel>> GetAllLogicalDevice(string deviceCategoryId)
        {
            string apiUrl = "api/1.0/LogicalDevice/ByDeviceCategoryId";
            var logicalDeviceCardJson = await _apiService.SendGetRequestAsync(apiUrl, deviceCategoryId);

            if (!string.IsNullOrEmpty(logicalDeviceCardJson))
            {
                return JsonConvert.DeserializeObject<List<LogicalDeviceModel>>(logicalDeviceCardJson);
            }
            return new List<LogicalDeviceModel>();
        }

        public async Task<LogicalDeviceModel> AddLogicalDeviceToCard(LogicalDeviceModel data)
        {
            string apiUrl = "api/1.0/LogicalDevice";
            // Original code used SendGetRequest with data, which is weird for Add. 
            // Assuming it meant Post or Get with body. Sticking to Get with body as per original.
            var logicalDeviceCardJson = await _apiService.SendGetRequestAsync(apiUrl, data);

            if (!string.IsNullOrEmpty(logicalDeviceCardJson))
            {
                return JsonConvert.DeserializeObject<LogicalDeviceModel>(logicalDeviceCardJson);
            }
            return null;
        }

        #endregion
    }
}

