namespace GreenerConfigurator.ClientCore.Models;

/// <summary>
/// Light-weight representation of a rule used in list views.
/// </summary>
public sealed class RuleSummaryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public string? Description { get; set; }
}
