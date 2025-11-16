namespace GreenerConfigurator.ClientCore.Models;

/// <summary>
/// Simplified representation of a managed building or installation.
/// </summary>
public sealed class LocationModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ExternalId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public IList<LocationDetailModel> Details { get; set; } = new List<LocationDetailModel>();
}
