namespace GreenerConfigurator.ClientCore.Models;

/// <summary>
/// Represents a specific floor or sub-area inside a physical location.
/// </summary>
public sealed class LocationDetailModel
{
    public Guid Id { get; set; }
    public Guid LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? FloorNumber { get; set; }
}
