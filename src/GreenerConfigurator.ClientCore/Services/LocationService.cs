using Greener.Web.Definitions.Api.MasterData.Location;
using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Location
{
    public class LocationService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LocationService> _logger;

        public LocationService(IApiService apiService, ILogger<LocationService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Public Method(s) ]

        public async Task<List<LocationModel>> GetAllLocation()
        {
            List<LocationModel> tempList = null;

            string apiUrl = "/api/1.0/location";

            var locationDetailJson = await _apiService.SendGetRequestAsync(apiUrl);

            if (!string.IsNullOrEmpty(locationDetailJson))
            {
                tempList = JsonConvert.DeserializeObject<List<LocationModel>>(locationDetailJson);
            }

            return tempList ?? new List<LocationModel>();
        }

        public async Task<LocationDetailModel> AddLocationDetail(LocationDetailModel locationDetail)
        {
            string apiUrl = "/api/1.0/LocationDetail/Add";

            var locationDetailJson = await _apiService.SendPostRequestAsync(apiUrl, locationDetail);

            // The original code didn't return the result properly, it returned null 'tempList'.
            // Assuming we want to return the deserialized object.
            if (!string.IsNullOrEmpty(locationDetailJson))
            {
                 return JsonConvert.DeserializeObject<LocationDetailModel>(locationDetailJson);
            }
            
            return null;
        }

        public async Task<LocationModel> AddLocationAsync(LocationModel location)
        {
            LocationModel result = null;
            try
            {
                if (location != null)
                {
                    LocationDto locationDto = MapModelToDto(location);

                    string apiUrl = "api/1.0/Location/Add";

                    var locationCardJson = await _apiService.SendPostRequestAsync(apiUrl, locationDto);

                    if (!string.IsNullOrEmpty(locationCardJson))
                    {
                        result = JsonConvert.DeserializeObject<LocationModel>(locationCardJson);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error adding location");
            }

            return result;
        }

        public async Task<LocationModel> EditLocationAsync(LocationModel location)
        {
            LocationModel result = null;
            try
            {
                if (location != null)
                {
                    LocationDto locationDto = MapModelToDto(location);

                    string apiUrl = "api/1.0/Location/Edit";

                    var locationCardJson = await _apiService.SendPostRequestAsync(apiUrl, locationDto);

                    if (!string.IsNullOrEmpty(locationCardJson))
                    {
                        result = JsonConvert.DeserializeObject<LocationModel>(locationCardJson);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error editing location");
            }

            return result;
        }

        #endregion

        #region [ Private Method(s) ]

        private LocationDto MapModelToDto(LocationModel location)
        {
            LocationDto result = new LocationDto();

            result.LocationId = location.LocationId;
            result.LocationName = location.LocationName;
            result.LocationType = location.LocationType;
            result.LocationDescription = location.LocationDescription;
            result.Latitude = location.Latitude;
            result.Longitude = location.Longitude;
            result.CountryCode = location.CountryCode;

            return result;
        }

        #endregion
    }
}

