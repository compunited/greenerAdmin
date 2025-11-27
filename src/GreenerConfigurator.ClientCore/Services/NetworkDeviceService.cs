using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenerConfigurator.ClientCore.Models.Network;

using Greener.Web.Definitions.API.Network;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class NetworkDeviceService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NetworkDeviceService> _logger;

        public NetworkDeviceService(IApiService apiService, ILogger<NetworkDeviceService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<UnassignedNetworkDeviceDto>> GetUnassignedNetworkDeviceAsync()
        {
            List<UnassignedNetworkDeviceDto> result = null;

            string apiUrl = "/api/1.0/NetworkDevice/UnassignedNetworkDevice/GetUnassigned";
            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                result = JsonConvert.DeserializeObject<List<UnassignedNetworkDeviceDto>>(jsonReq);
            }

            return result ?? new List<UnassignedNetworkDeviceDto>();
        }

        public async Task<List<AddUnassignedNetworkDeviceRequestResultDto>> AddUnassignedNetworkDeviceAsync(List<UnassignedNetworkDeviceDto> unassignedNetworkDeviceList)
        {
            List<AddUnassignedNetworkDeviceRequestResultDto> result = null;

            string apiUrl = "/api/1.0/NetworkDevice/UnassignedNetworkDevice/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, unassignedNetworkDeviceList);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                result = JsonConvert.DeserializeObject<List<AddUnassignedNetworkDeviceRequestResultDto>>(jsonReq);
            }

            return result;
        }

        public async Task<List<NetworkDeviceViewModel>> GetAllNetworkDevicesAsync()
        {
            List<NetworkDeviceViewModel> tempList = null;

            string apiUrl = "/api/1.0/NetworkDevice/NetworkDevices";

            var locationDetailJson = await _apiService.SendGetRequestAsync(apiUrl);

            if (!string.IsNullOrEmpty(locationDetailJson))
            {
                tempList = JsonConvert.DeserializeObject<List<NetworkDeviceViewModel>>(locationDetailJson);
            }

            return tempList ?? new List<NetworkDeviceViewModel>();
        }

        public async Task<List<NetworkDeviceViewModel>> GetNetworkDevicesForLocationIdAsync(Guid locationId)
        {
            List<NetworkDeviceViewModel> tempList = null;

            string apiUrl = "/api/1.0/NetworkDevice/NetworkDevicesForLocationId";

            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl, locationId);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                tempList = JsonConvert.DeserializeObject<List<NetworkDeviceViewModel>>(jsonReq);
            }

            return tempList ?? new List<NetworkDeviceViewModel>();
        }

        public async Task<List<NetworkDeviceViewModel>> GetNetworkDevicesForLocationDetailIdAsync(Guid locationDetailId)
        {
            List<NetworkDeviceViewModel> tempList = null;

            string apiUrl = "/api/1.0/NetworkDevice/NetworkDevicesForLocationDetailId";

            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl, locationDetailId);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                tempList = JsonConvert.DeserializeObject<List<NetworkDeviceViewModel>>(jsonReq);
            }

            return tempList ?? new List<NetworkDeviceViewModel>();
        }

        public async Task<NetworkDeviceEditModel> GetNetworkDeviceEditModelByNetworkDevideIdAsync(Guid networkDeviceId)
        {
            NetworkDeviceEditModel temp = null;

            string apiUrl = "/api/1.0/NetworkDevice/NetworkDeviceByNetworkDeviceId";

            var jsonReq = await _apiService.SendGetRequestAsync(apiUrl, networkDeviceId);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                temp = JsonConvert.DeserializeObject<NetworkDeviceEditModel>(jsonReq);
            }

            return temp;
        }

        public async Task<NetworkDeviceEditModel> AddNetworkDeviceAsync(NetworkDeviceEditDto networkDevice)
        {
            string apiUrl = "api/1.0/NetworkDevice/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, networkDevice);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<NetworkDeviceEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<NetworkDeviceEditModel> EditNetworkDeviceAsync(NetworkDeviceEditDto networkDevice)
        {
            string apiUrl = "api/1.0/NetworkDevice/Edit";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, networkDevice);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<NetworkDeviceEditModel>(jsonReq);
            }
            return null;
        }

        public async Task RemoveNetworkDeviceAsync(Guid networkDeviceId)
        {
            string apiUrl = "api/1.0/NetworkDevice/Delete";
            await _apiService.SendPostRequestAsync(apiUrl, networkDeviceId);
        }
    }
}

