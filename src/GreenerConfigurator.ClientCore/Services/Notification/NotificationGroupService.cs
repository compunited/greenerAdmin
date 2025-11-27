using Greener.Web.Definitions.API.Notification;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Notification
{
    public class NotificationGroupService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NotificationGroupService> _logger;

        public NotificationGroupService(IApiService apiService, ILogger<NotificationGroupService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<List<NotificationGroupViewDto>> GetNotificationGroupAsync()
        {
            string apiUrl = "/api/1.0/NotificationGroup/GetAll";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, null);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<List<NotificationGroupViewDto>>(jsonReq);
            }
            return null;
        }
    }
}
