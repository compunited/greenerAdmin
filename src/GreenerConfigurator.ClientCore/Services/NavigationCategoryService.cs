using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class NavigationCategoryService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NavigationCategoryService> _logger;

        public NavigationCategoryService(IApiService apiService, ILogger<NavigationCategoryService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        #region [ Internal Method(s) ]

        public async Task<List<NavigationCategoryModel>> GetAllNavigationCategoryAsync()
        {
            List<NavigationCategoryModel> result = null;

            try
            {
                var apiUrl = "api/1.0/LogicalDeviceNavigationCategory/getall";
                var resultJsonString = await _apiService.SendGetRequestAsync(apiUrl, string.Empty);

                if (!string.IsNullOrEmpty(resultJsonString))
                {
                    result = JsonConvert.DeserializeObject<List<NavigationCategoryModel>>(resultJsonString);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting all navigation categories");
            }

            return result ?? new List<NavigationCategoryModel>();
        }

        public async Task<List<NavigationCategoryModel>> GetNavigationCategoryByNavigationCardGroupIdAsync(Guid navigationCardGroupId)
        {
            List<NavigationCategoryModel> result = null;

            try
            {
                var apiUrl = "api/1.0/LogicalDeviceNavigationCategory/GetByNavigationCardGroupId";
                var resultJsonString = await _apiService.SendPostRequestAsync(apiUrl, navigationCardGroupId);

                if (!string.IsNullOrEmpty(resultJsonString))
                {
                    result = JsonConvert.DeserializeObject<List<NavigationCategoryModel>>(resultJsonString);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting navigation categories by group id");
            }

            return result ?? new List<NavigationCategoryModel>();
        }

        public async Task<List<NavigationCategoryModel>> GetAvailableNavigationCategoryByNavigationCardGroupIdAsync(Guid navigationCardGroupId)
        {
            List<NavigationCategoryModel> result = null;

            try
            {
                var apiUrl = "api/1.0/LogicalDeviceNavigationCategory/GetAvailableByNavigationCardGroupId";
                var resultJsonString = await _apiService.SendPostRequestAsync(apiUrl, navigationCardGroupId);

                if (!string.IsNullOrEmpty(resultJsonString))
                {
                    result = JsonConvert.DeserializeObject<List<NavigationCategoryModel>>(resultJsonString);
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Error getting available navigation categories by group id");
            }

            return result ?? new List<NavigationCategoryModel>();
        } 

        #endregion

    }
}

