using GreenerConfigurator.ClientCore.Models;

namespace GreenerConfigurator.ClientCore.Services;

/// <summary>
/// Provides typed access to the Greener location endpoints.
/// </summary>
public sealed class LocationService
{
    private static readonly string BasePath = "api/1.0/location";
    private readonly ApiClient _apiClient;

    public LocationService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<LocationModel>?> GetLocationsAsync(CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<IReadOnlyList<LocationModel>>(BasePath, cancellationToken);

    public Task<LocationModel?> GetLocationAsync(Guid locationId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<LocationModel>($"{BasePath}/{locationId}", cancellationToken);

    public Task<LocationModel?> CreateLocationAsync(LocationModel payload, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<LocationModel, LocationModel>(BasePath, payload, cancellationToken);

    public Task<LocationModel?> UpdateLocationAsync(LocationModel payload, CancellationToken cancellationToken = default)
    {
        if (payload.Id == Guid.Empty)
        {
            throw new ArgumentException("The payload must contain an identifier before it can be updated.", nameof(payload));
        }

        return _apiClient.PutAsync<LocationModel, LocationModel>($"{BasePath}/{payload.Id}", payload, cancellationToken);
    }

    public Task DeleteLocationAsync(Guid locationId, CancellationToken cancellationToken = default) =>
        _apiClient.DeleteAsync($"{BasePath}/{locationId}", cancellationToken);
}
