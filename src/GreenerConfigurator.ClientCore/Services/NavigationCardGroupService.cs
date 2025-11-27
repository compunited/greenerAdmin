using Greener.Web.Definitions.API.Navigation;
using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class NavigationCardGroupService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NavigationCardGroupService> _logger;

        public NavigationCardGroupService(IApiService apiService, ILogger<NavigationCardGroupService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Public Method(s) ]

        public async Task<List<NavigationCardGroupModel>> GetNavigationCardGroupByNavigationCategoryIdAsync(Guid navigaitionCategoryId)
        {
            List<NavigationCardGroupModel> result = null;

            try
            {
                var apiUrl = "api/1.0/LogicalDeviceNavigationCard/Group/ByNavigationCategoryId";
                var resultJsonString = await _apiService.SendGetRequestAsync(apiUrl, navigaitionCategoryId);

                if (!string.IsNullOrEmpty(resultJsonString))
                {
                    result = JsonConvert.DeserializeObject<List<NavigationCardGroupModel>>(resultJsonString);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting navigation card groups by category id");
            }

            return result ?? new List<NavigationCardGroupModel>();
        }

        public async Task<NavigationCardGroupModel> AddNavigationCardGroupAsync(NavigationCardGroupModel navigationCardGroup)
        {
            NavigationCardGroupModel result = null;

            try
            {
                if (navigationCardGroup != null)
                {
                    LogicalDeviceNavigationCardGroupDto navigationCardDto = MapModelToDto(navigationCardGroup);

                    string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Group/Add";

                    var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, navigationCardDto);

                    if (!string.IsNullOrEmpty(navigationCardJson))
                    {
                        result = JsonConvert.DeserializeObject<NavigationCardGroupModel>(navigationCardJson);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error adding navigation card group");
            }

            return result;
        }

        public async Task<NavigationCardGroupModel> EditNavigationCardGroupAsync(NavigationCardGroupModel navigationCardGroup)
        {
            NavigationCardGroupModel result = null;

            try
            {
                if (navigationCardGroup != null)
                {
                    LogicalDeviceNavigationCardGroupDto navigationCardDto = MapModelToDto(navigationCardGroup);

                    string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Group/Edit";

                    var navigationCardJson = await _apiService.SendPostRequestAsync(apiUrl, navigationCardDto);

                    if (!string.IsNullOrEmpty(navigationCardJson))
                    {
                        result = JsonConvert.DeserializeObject<NavigationCardGroupModel>(navigationCardJson);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error editing navigation card group");
            }

            return result;
        }

        public async Task RemoveNavigationCardGroupAsync(Guid navigationCardGroupId)
        {
            try
            {
                string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Group/Delete";
                await _apiService.SendPostRequestAsync(apiUrl, navigationCardGroupId);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error removing navigation card group");
            }
        }

        #region [ M2M ]

        public async Task AddNavigationCardGroupCategoryM2mAsync(LogicalDeviceNavigationCardGroupCategoryM2mDto navigationCardGroupCategoryM2MDto)
        {
            
            try
            {
                string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Group/M2M/Add";
                await _apiService.SendPostRequestAsync(apiUrl, navigationCardGroupCategoryM2MDto);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error adding navigation card group category m2m");
            }
        }

        public async Task DeleteNavigationCardGroupCategoryM2mAsync(LogicalDeviceNavigationCardGroupCategoryM2mDto navigationCardGroupCategoryM2MDto)
        {
            try
            {
                string apiUrl = "api/1.0/LogicalDeviceNavigationCard/Group/M2M/Delete";
                await _apiService.SendPostRequestAsync(apiUrl, navigationCardGroupCategoryM2MDto);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error deleting navigation card group category m2m");
            }
        }


        #endregion

        #endregion

        #region [ Private Method(s) ]

        private LogicalDeviceNavigationCardGroupDto MapModelToDto(NavigationCardGroupModel navigationCardGroup)
        {
            LogicalDeviceNavigationCardGroupDto result = new LogicalDeviceNavigationCardGroupDto()
            {
                LocationId = navigationCardGroup.LocationId,
                LogicalDeviceNavigationCardGroupId = navigationCardGroup.LogicalDeviceNavigationCardGroupId,
                LogicalDeviceNavigationCategoryId = navigationCardGroup.LogicalDeviceNavigationCategoryId,
                Name = navigationCardGroup.Name
            };

            return result;
        }

        #endregion
    }
}

