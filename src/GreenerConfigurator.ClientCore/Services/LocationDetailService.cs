using Greener.Web.Definitions.Api.MasterData.Location;
using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class LocationDetailService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LocationDetailService> _logger;

        public LocationDetailService(IApiService apiService, ILogger<LocationDetailService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<LocationDetailModel>> GetLocationDetailByLocationIdAsync(Guid locationId)
        {
            List<LocationDetailModel> result = null;

            string apiUrl = "/api/1.0/Location/LocationDetail/ByLocationId";

            var locationDetailJson = await _apiService.SendGetRequestAsync(apiUrl, locationId);

            if (!string.IsNullOrEmpty(locationDetailJson))
            {
                result = JsonConvert.DeserializeObject<List<LocationDetailModel>>(locationDetailJson);
            }

            return result ?? new List<LocationDetailModel>();
        }

        public async Task<LocationDetailModel> AddLocationDetail(LocationDetailModel locationDetailModel)
        {
            LocationDetailModel result = null;

            LocationDetailDto locationDetailDto = MapModelToDto(locationDetailModel);

            string apiUrl = "/api/1.0/Location/LocationDetail/Add";

            var locationDetailJson = await _apiService.SendPostRequestAsync(apiUrl, locationDetailDto);

            if (!string.IsNullOrEmpty(locationDetailJson))
            {
                result = JsonConvert.DeserializeObject<LocationDetailModel>(locationDetailJson);
            }

            return result;
        }

        public async Task<LocationDetailModel> EditLocationDetail(LocationDetailModel locationDetailModel)
        {
            LocationDetailModel result = null;

            LocationDetailDto locationDetailDto = MapModelToDto(locationDetailModel);

            string apiUrl = "/api/1.0/Location/LocationDetail/Edit";

            var locationDetailJson = await _apiService.SendPostRequestAsync(apiUrl, locationDetailDto);

            if (!string.IsNullOrEmpty(locationDetailJson))
            {
                result = JsonConvert.DeserializeObject<LocationDetailModel>(locationDetailJson);
            }

            return result;
        }


        #region [ Private Method(s) ]

        private LocationDetailDto MapModelToDto(LocationDetailModel locationDetailModel)
        {
            LocationDetailDto temp = new LocationDetailDto();

            temp.LocationDetailId = locationDetailModel.LocationDetailId;
            temp.LocationDetailName = locationDetailModel.LocationDetailName;
            temp.LocationDetailParentId = locationDetailModel.LocationDetailParentId;
            temp.LocationDetailType = locationDetailModel.LocationDetailType;
            temp.LocationId = locationDetailModel.LocationId;

            return temp;
        }

        #endregion
    }
}

