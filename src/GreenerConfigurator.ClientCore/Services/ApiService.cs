using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GreenerConfigurator.ClientCore.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> SendGetRequestAsync(string apiUrl, object bodyContent = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                if (bodyContent != null)
                {
                    var json = JsonConvert.SerializeObject(bodyContent);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.SendAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    _logger.LogError($"Error calling API {apiUrl}: {response.StatusCode}");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception calling API {apiUrl}");
                throw;
            }
        }

        public async Task<string> SendPostRequestAsync(string apiUrl, object bodyContent)
        {
            try
            {
                var json = JsonConvert.SerializeObject(bodyContent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    _logger.LogError($"Error calling API {apiUrl}: {response.StatusCode}");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception calling API {apiUrl}");
                throw;
            }
        }
    }
}
