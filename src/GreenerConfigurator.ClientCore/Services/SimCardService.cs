using GreenerConfigurator.ClientCore.Models.Network;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class SimCardService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<SimCardService> _logger;

        public SimCardService(IApiService apiService, ILogger<SimCardService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<SimCardViewModel>> GetSimCardByNetworkDeviceIdAsync(Guid networkDeviceId)
        {
            List<SimCardViewModel> tempList = null;

            string apiUrl = "/api/1.0/SimCard/GetSimCardsByNetworkDeviceId";

            var simCardJson = await _apiService.SendGetRequestAsync(apiUrl, networkDeviceId);

            if (!string.IsNullOrEmpty(simCardJson))
            {
                tempList = JsonConvert.DeserializeObject<List<SimCardViewModel>>(simCardJson);
            }

            return tempList ?? new List<SimCardViewModel>();
        }


        public async Task<SimCardEditModel> GetSimCardByNetworkDeviceIdAndSimCardIdAsync(SimCardViewModel simCardViewModel)
        {
            SimCardEditModel result = null;

            string apiUrl = "/api/1.0/SimCard/GetSimCardsByNetworkDeviceIdAndSimCardId";

            var simCardJson = await _apiService.SendGetRequestAsync(apiUrl, simCardViewModel);

            if (!string.IsNullOrEmpty(simCardJson))
            {
                result = JsonConvert.DeserializeObject<SimCardEditModel>(simCardJson);
            }

            return result;
        }


        public async Task<SimCardEditModel> AddSimCardAsync(SimCardEditModel simCardEditModel)
        {
            string apiUrl = "api/1.0/SimCard/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, simCardEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<SimCardEditModel>(jsonReq);
            }
            return null;
        }


        public async Task<SimCardEditModel> EditSimCardAsync(SimCardEditModel simCardEditModel)
        {
            string apiUrl = "api/1.0/SimCard/Edit";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, simCardEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<SimCardEditModel>(jsonReq);
            }
            return null;
        }


        public async Task RemoveSimCardAsync(SimCardEditModel simCardEditModel)
        {
            string apiUrl = "api/1.0/SimCard/Delete";
            await _apiService.SendPostRequestAsync(apiUrl, simCardEditModel);
        }

    }
}

