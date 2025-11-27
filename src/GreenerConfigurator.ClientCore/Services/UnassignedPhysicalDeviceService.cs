using Greener.Web.Definitions.Api.MasterData.Device;
using Greener.Web.Definitions.API.MasterData.Device.Manufacturer;
using Greener.Web.Definitions.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenerConfigurator.ClientCore.Services
{
    public class UnassignedPhysicalDeviceService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<UnassignedPhysicalDeviceService> _logger;

        public UnassignedPhysicalDeviceService(IApiService apiService, ILogger<UnassignedPhysicalDeviceService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<UnassignedPhysicalDeviceDto>> ImportKeys(List<UnassignedPhysicalDeviceDto> keyList)
        {
            List<UnassignedPhysicalDeviceDto> tempList = new List<UnassignedPhysicalDeviceDto>();
            try
            {
                string apiUrl = "api/1.0/UnassignedPhysicalDevice/Importkeys";

                var keys = await _apiService.SendGetRequestAsync(apiUrl, keyList);

                tempList = JsonConvert.DeserializeObject<List<UnassignedPhysicalDeviceDto>>(keys);

            }
            catch (Exception exp)
            {
                tempList = null;
                _logger.LogError(exp, "Error importing keys");
            }

            return tempList;
        }

        public async Task<ManufacturerAndDeviceInfoDto> GetManufacturerAndDeviceInfo(DeviceConnectionType deviceConnectionType)
        {
            ManufacturerAndDeviceInfoDto manInfo = new ManufacturerAndDeviceInfoDto();
            try
            {
                string apiUrl = "api/1.0/UnassignedPhysicalDevice/GetManufacturerAndDeviceInfo";

                var result = await _apiService.SendGetRequestAsync(apiUrl, deviceConnectionType);

                manInfo = JsonConvert.DeserializeObject<ManufacturerAndDeviceInfoDto>(result);
            }
            catch (Exception exp)
            {
                manInfo = null;
                _logger.LogError(exp, "Error getting manufacturer and device info");
            }

            return manInfo;
        }
    }
}
