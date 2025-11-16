using System.Collections.Concurrent;
using GreenerConfigurator.ClientCore.Models;

namespace GreenerConfigurator.Web.Server.Repositories;

public sealed class RuleRepository
{
    private readonly ConcurrentDictionary<Guid, RuleEditModel> _rules = new();

    public RuleRepository()
    {
        var ruleId = Guid.NewGuid();
        _rules[ruleId] = new RuleEditModel
        {
            Id = ruleId,
            Name = "Temperature Watch",
            Category = "HVAC",
            Description = "Alerts when the average temperature is outside of the comfort window.",
            IsEnabled = true,
            StartTime = new TimeOnly(6, 0),
            EndTime = new TimeOnly(21, 0),
            NotificationGroups = { "Facility Managers" }
        };
    }

    public IReadOnlyCollection<RuleSummaryModel> GetSummaries() => _rules.Values
        .Select(rule => new RuleSummaryModel
        {
            Id = rule.Id,
            Name = rule.Name,
            Category = rule.Category,
            Description = rule.Description,
            IsEnabled = rule.IsEnabled
        })
        .OrderBy(rule => rule.Name, StringComparer.OrdinalIgnoreCase)
        .ToList();

    public RuleEditModel? Get(Guid id) => _rules.TryGetValue(id, out var rule) ? Clone(rule) : null;

    public RuleEditModel Save(RuleEditModel rule)
    {
        if (rule.Id == Guid.Empty)
        {
            rule.Id = Guid.NewGuid();
        }

        var clone = Clone(rule);
        _rules.AddOrUpdate(rule.Id, clone, (_, _) => clone);
        return Clone(clone);
    }

    public void Delete(Guid id) => _rules.TryRemove(id, out _);

    private static RuleEditModel Clone(RuleEditModel rule) => new()
    {
        Id = rule.Id,
        Name = rule.Name,
        Category = rule.Category,
        Description = rule.Description,
        IsEnabled = rule.IsEnabled,
        StartTime = rule.StartTime,
        EndTime = rule.EndTime,
        NotificationGroups = rule.NotificationGroups.ToList()
    };
}
