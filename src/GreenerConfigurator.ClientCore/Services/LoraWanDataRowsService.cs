using GreenerConfigurator.ClientCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class LoraWanDataRowsService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LoraWanDataRowsService> _logger;

        public LoraWanDataRowsService(IApiService apiService, ILogger<LoraWanDataRowsService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Public Method(s) ]

        public async Task<List<LoraWanDataRowsModel>> GetLoraWanDataRowsLatestByLocationDetailId(Guid locationDetailId)
        {
            string apiUrl = "api/1.0/LoraWanDataRows/ByLocationDetailId";
            var loraWanDataRowsJson = await _apiService.SendGetRequestAsync(apiUrl, locationDetailId);

            if (!string.IsNullOrEmpty(loraWanDataRowsJson))
            {
                return JsonConvert.DeserializeObject<List<LoraWanDataRowsModel>>(loraWanDataRowsJson);
            }
            return new List<LoraWanDataRowsModel>();
        }

        public async Task<List<LoraWanDataRowsModel>> GetLoraWanDataRowsLatestByLocationId(Guid locationId)
        {
            string apiUrl = "api/1.0/LoraWanDataRows/ByLocationId";
            var loraWanDataRowsJson = await _apiService.SendGetRequestAsync(apiUrl, locationId);

            if (!string.IsNullOrEmpty(loraWanDataRowsJson))
            {
                return JsonConvert.DeserializeObject<List<LoraWanDataRowsModel>>(loraWanDataRowsJson);
            }
            return new List<LoraWanDataRowsModel>();
        }

        #endregion
    }
}

