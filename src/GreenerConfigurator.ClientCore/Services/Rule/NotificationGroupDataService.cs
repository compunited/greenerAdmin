using GreenerConfigurator.ClientCore.Models.Rule;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GreenerConfigurator.ClientCore.Services.Rule
{
    public class NotificationGroupDataService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<NotificationGroupDataService> _logger;

        public NotificationGroupDataService(IApiService apiService, ILogger<NotificationGroupDataService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<NotificationGroupDataEditModel> AddNotificationGroupDataAsync(NotificationGroupDataEditModel notificationGroupDataEditModel)
        {
            string apiUrl = "/api/1.0/NotificationGroupData/Add";
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, notificationGroupDataEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<NotificationGroupDataEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<NotificationGroupDataEditModel> EditNotificationGroupDataAsync(NotificationGroupDataEditModel notificationGroupDataEditModel)
        {
            string apiUrl = "/api/1.0/CompareCondition/Edit"; // Note: Original code had this URL, keeping it but it looks suspicious (CompareCondition vs NotificationGroupData)
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, notificationGroupDataEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<NotificationGroupDataEditModel>(jsonReq);
            }
            return null;
        }

        public async Task<NotificationGroupDataEditModel> RemoveNotificationGroupDataAsync(NotificationGroupDataEditModel notificationGroupDataEditModel)
        {
            string apiUrl = "/api/1.0/CompareCondition/Delete"; // Note: Original code had this URL
            var jsonReq = await _apiService.SendPostRequestAsync(apiUrl, notificationGroupDataEditModel);

            if (!string.IsNullOrEmpty(jsonReq))
            {
                return JsonConvert.DeserializeObject<NotificationGroupDataEditModel>(jsonReq);
            }
            return null;
        }
    }
}

