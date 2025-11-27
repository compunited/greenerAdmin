using Greener.Web.Definitions.API.DeviceState;
using Greener.Web.Definitions.Enums;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class DeviceStateService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<DeviceStateService> _logger;

        public DeviceStateService(IApiService apiService, ILogger<DeviceStateService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<DeviceStateViewDto>> GetDeviceStateByLocationIdAsync(Guid locationId)
        {
            List<DeviceStateViewDto> result = null;

            string apiUrl = "api/1.0/DeviceState/ByLocationId";
            DeviceStateLocationFilterDto deviceStateLocationFilterDto = new DeviceStateLocationFilterDto();
            deviceStateLocationFilterDto.LocationId = locationId;
      
            deviceStateLocationFilterDto.AlarmLevels = new List<AlarmLevel>() { AlarmLevel.Alert, AlarmLevel.Normal, AlarmLevel.Warning };

            var tempJsonResult = await _apiService.SendPostRequestAsync(apiUrl, deviceStateLocationFilterDto);

            if (!string.IsNullOrEmpty(tempJsonResult))
            {
                result = JsonConvert.DeserializeObject<List<DeviceStateViewDto>>(tempJsonResult);
            }

            return result ?? new List<DeviceStateViewDto>();
        }
    }
}

