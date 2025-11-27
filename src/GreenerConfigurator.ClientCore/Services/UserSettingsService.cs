using Greener.Web.Definitions.User;
using GreenerConfigurator.ClientCore.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services
{
    public class UserSettingsService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<UserSettingsService> _logger;

        public UserSettingsService(IApiService apiService, ILogger<UserSettingsService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<UserSettingsDto> GetUserSettings()
        {
            var userSettingsJson = await _apiService.SendGetRequestAsync("api/1.0/UserSettings/GetUserSettings");
            if (!string.IsNullOrEmpty(userSettingsJson))
            {
                var userSettings = JsonConvert.DeserializeObject<UserSettingsDto>(userSettingsJson);
                return userSettings;
            }
            return null;
        }
    }
}

