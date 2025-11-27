using Greener.Web.Definitions.Api.Navigation;
using Greener.Web.Definitions.API.Navigation;
using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class NavigationCardService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NavigationCardService> _logger;

        public NavigationCardService(IApiService apiService, ILogger<NavigationCardService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Public Method(s) ]

        public async Task<List<NavigationCardModel>> GetLogicalDeviceNavigationCardByNavigationCategoryIdAsync(Guid navigationCategoryId)
        {
            List<NavigationCardModel> tempList = null;

            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/ByNavigationCategoryId/CardList";

            var navigationCardJson = await _apiService.SendGetRequestAsync(apiUrl, navigationCategoryId);

            if (!string.IsNullOrEmpty(navigationCardJson))
            {
                tempList = JsonConvert.DeserializeObject<List<NavigationCardModel>>(navigationCardJson);
            }

            return tempList ?? new List<NavigationCardModel>();
        }

        public async Task<NavigationCardModel> AddNavigationCardAsync(NavigationCardModel navigationCard)
        {
            NavigationCardModel result = null;

            try
            {
                if (navigationCard != null)
                {
                    LogicalDeviceNavigationCardDto navigationCardDto = MapModelToDto(navigationCard);

                    string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Add";

                    var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, navigationCardDto);

                    if (!string.IsNullOrEmpty(navigationCardJson))
                    {
                        result = JsonConvert.DeserializeObject<NavigationCardModel>(navigationCardJson);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error adding navigation card");
            }

            return result;
        }

        public async Task<NavigationCardModel> EditNavigationCardAsync(NavigationCardModel navigationCard)
        {
            NavigationCardModel result = null;

            try
            {
                if (navigationCard != null)
                {
                    LogicalDeviceNavigationCardDto navigationCardDto = MapModelToDto(navigationCard);

                    string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Edit";

                    var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, navigationCardDto);

                    if (!string.IsNullOrEmpty(navigationCardJson))
                    {
                        result = JsonConvert.DeserializeObject<NavigationCardModel>(navigationCardJson);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error editing navigation card");
            }

            return result;
        }

        public async Task RemoveLogicalDeviceNavigationCardAsync(Guid logicalDeviceNavigationCardId)
        {
            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Delete";

            await _apiService.SendPostRequestAsync(apiUrl, logicalDeviceNavigationCardId);
        }

        public async Task<NavigationCardModel> RemoveLogicalDeviceNavigationCardFromGroupAsync(Guid logicalDeviceNavigationCardId)
        {
            NavigationCardModel result = null;

            try
            {
                string apiUrl = "api/1.0/LogicalDeviceNavigationCard/DeleteFromGroup";
                var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, logicalDeviceNavigationCardId);
                if (!string.IsNullOrEmpty(navigationCardJson))
                {
                    result = JsonConvert.DeserializeObject<NavigationCardModel>(navigationCardJson);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error removing navigation card from group");
            }

            return result;
        }

        public async Task<NavigationCardModel> SetLogicalDeviceNavigationCardToGroupAsync(NavigationCardModel navigationCard)
        {
            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/SetGroup";

            var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, navigationCard);

            if (!string.IsNullOrEmpty(navigationCardJson))
            {
                return JsonConvert.DeserializeObject<NavigationCardModel>(navigationCardJson);
            }
            return null;
        }

        public async Task<List<NavigationCardModel>> GetLogicalDeviceNavigationCardByNavigationCardGroupoIdAsync(Guid navigationCardGroupId)
        {
            List<NavigationCardModel> tempList = null;

            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/ByNavigationCardGroupId/CardList";

            var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, navigationCardGroupId);

            if (!string.IsNullOrEmpty(navigationCardJson))
            {
                tempList = JsonConvert.DeserializeObject<List<NavigationCardModel>>(navigationCardJson);
            }

            return tempList ?? new List<NavigationCardModel>();
        }

        public async Task<List<NavigationCardModel>> GetLogicalDeviceNavigationCardByLoationIdAsync(Guid locationId)
        {
            List<NavigationCardModel> tempList = null;

            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/ByLocationId";

            var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, locationId);

            if (!string.IsNullOrEmpty(navigationCardJson))
            {
                tempList = JsonConvert.DeserializeObject<List<NavigationCardModel>>(navigationCardJson);
            }

            return tempList ?? new List<NavigationCardModel>();
        }

        #region [ M2M ]

        public async Task<List<NavigationM2MModel>> GetLogicalDeviceNavigationCardM2MByNavigationCardIdAsync(Guid navigationCardId)
        {
            List<NavigationM2MModel> result = null;

            try
            {
                var apiUrl = "api/1.0/LogicalDeviceNavigationCard/M2M/GetByNavigationCardId";
                var resultJsonString = await _apiService.SendPostRequestAsync(apiUrl, navigationCardId);

                if (!string.IsNullOrEmpty(resultJsonString))
                {
                    result = JsonConvert.DeserializeObject<List<NavigationM2MModel>>(resultJsonString);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting M2M by navigation card id");
            }

            return result ?? new List<NavigationM2MModel>();
        }

        public async Task<List<NavigationM2MModel>> GetAvailableDevicesByNavigationCardIdAsync(Guid navigationCardId)
        {
            List<NavigationM2MModel> result = null;

            try
            {
                var apiUrl = "api/1.0/LogicalDeviceNavigationCard/M2M/GetAvailableDevicesByNavigationCardId";
                var resultJsonString = await _apiService.SendPostRequestAsync(apiUrl, navigationCardId);

                if (!string.IsNullOrEmpty(resultJsonString))
                {
                    result = JsonConvert.DeserializeObject<List<NavigationM2MModel>>(resultJsonString);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting available devices by navigation card id");
            }

            return result ?? new List<NavigationM2MModel>();
        }

        public async Task RemoveLogicalDeviceNavigationM2MAsync(Guid logicalDeviceNavigationM2mId)
        {
            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/M2M/Delete";

            await _apiService.SendPostRequestAsync(apiUrl, logicalDeviceNavigationM2mId);
        }

        public async Task AddLogicalDeviceNavigationM2MAsync(NavigationM2MModel navigationM2MModel)
        {
            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/M2M/Add";
            LogicalDeviceNavigationM2MCreateDto dto = MapModelToDto(navigationM2MModel);

            await _apiService.SendPostRequestAsync(apiUrl, dto);
        }

        public async Task UpdateLogicalDeviceNavigationM2MListAsync(List<NavigationM2MModel> navigationM2MModelList)
        {
            string apiUrl = "api/1.0/LogicalDeviceNavigationCard/M2M/UpdateSortNumberAndDataDetailNumber";
            List<LogicalDeviceNavigationM2MViewDto> dtoList = new List<LogicalDeviceNavigationM2MViewDto>();
            foreach (var item in navigationM2MModelList)
                dtoList.Add(MapModelToViewDto(item));

            await _apiService.SendPostRequestAsync(apiUrl, navigationM2MModelList);
        }

        #endregion

        #endregion

        #region [ Private Method(s) ]

        private LogicalDeviceNavigationCardDto MapModelToDto(NavigationCardModel navigationCard)
        {
            LogicalDeviceNavigationCardDto result = null;

            if (navigationCard != null)
            {
                result = new LogicalDeviceNavigationCardDto();
                result.CardName = navigationCard.CardName;
                result.LocationId = navigationCard.LocationId;
                result.LogicalDeviceNavigationCardId = navigationCard.LogicalDeviceNavigationCardId;
                result.LogicalDeviceNavigationCategoryId = navigationCard.LogicalDeviceNavigationCategoryId;
            }

            return result;
        }

        private LogicalDeviceNavigationM2MCreateDto MapModelToDto(NavigationM2MModel navigationM2MModel)
        {
            LogicalDeviceNavigationM2MCreateDto result = null;

            if (navigationM2MModel != null)
            {
                result = new LogicalDeviceNavigationM2MCreateDto();
                result.DataDetailNumber = navigationM2MModel.DataDetailNumber;
                result.LogicalDeviceId = navigationM2MModel.LogicalDeviceId;
                result.LogicalDeviceNavigationCardId = navigationM2MModel.LogicalDeviceNavigationCardId;
                result.Name = navigationM2MModel.Name;
                result.SortNumber = navigationM2MModel.SortNumber;
            }

            return result;
        }

        private LogicalDeviceNavigationM2MViewDto MapModelToViewDto(NavigationM2MModel navigationM2MModel)
        {
            LogicalDeviceNavigationM2MViewDto result = null;

            if (navigationM2MModel != null)
            {
                result = new LogicalDeviceNavigationM2MViewDto();
                result.DataDetailNumber = navigationM2MModel.DataDetailNumber;
                result.LogicalDeviceId = navigationM2MModel.LogicalDeviceId;
                result.LogicalDeviceNavigationCardId = navigationM2MModel.LogicalDeviceNavigationCardId;
                result.Name = navigationM2MModel.Name;
                result.SortNumber = navigationM2MModel.SortNumber;
                result.LogicalDeviceNavigationM2mId = navigationM2MModel.LogicalDeviceNavigationM2mId;
            }

            return result;
        }

        #endregion
    }
}

