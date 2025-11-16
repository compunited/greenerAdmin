namespace GreenerConfigurator.ClientCore.Models;

/// <summary>
/// Detailed rule payload used when editing existing configuration.
/// </summary>
public sealed class RuleEditModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public IList<string> NotificationGroups { get; set; } = new List<string>();
}
