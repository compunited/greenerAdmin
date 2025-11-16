using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GreenerConfigurator.ClientCore.Options;

namespace GreenerConfigurator.ClientCore.Services;

/// <summary>
/// Provides a small wrapper around <see cref="HttpClient"/> that ensures the Greener API
/// is called with a consistent base address and serializer configuration.
/// </summary>
public sealed class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiClient> _logger;

    public ApiClient(HttpClient httpClient, IOptions<GreenerApiOptions> options, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        var configuredBase = options.Value.BaseAddress;
        if (configuredBase != null && _httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = configuredBase;
        }

        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
        await EnsureSuccess(response).ConfigureAwait(false);
        if (response.Content.Headers.ContentLength == 0)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(_serializerOptions, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string requestUri, TRequest payload, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(requestUri, payload, _serializerOptions, cancellationToken).ConfigureAwait(false);
        await EnsureSuccess(response).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string requestUri, TRequest payload, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(requestUri, payload, _serializerOptions, cancellationToken).ConfigureAwait(false);
        await EnsureSuccess(response).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
        await EnsureSuccess(response).ConfigureAwait(false);
    }

    private async Task EnsureSuccess(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        _logger.LogError("API request failed with status {StatusCode}. Body: {Body}", (int)response.StatusCode, body);
        response.EnsureSuccessStatusCode();
    }
}
