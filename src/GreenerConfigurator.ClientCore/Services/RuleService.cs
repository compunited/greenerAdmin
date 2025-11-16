using GreenerConfigurator.ClientCore.Models;

namespace GreenerConfigurator.ClientCore.Services;

/// <summary>
/// Typed access to the Greener rule endpoints.
/// </summary>
public sealed class RuleService
{
    private const string BasePath = "api/1.0/rule";
    private readonly ApiClient _apiClient;

    public RuleService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<RuleSummaryModel>?> GetRulesAsync(CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<IReadOnlyList<RuleSummaryModel>>(BasePath, cancellationToken);

    public Task<RuleEditModel?> GetRuleAsync(Guid ruleId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<RuleEditModel>($"{BasePath}/{ruleId}", cancellationToken);

    public Task<RuleEditModel?> CreateRuleAsync(RuleEditModel payload, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<RuleEditModel, RuleEditModel>(BasePath, payload, cancellationToken);

    public Task<RuleEditModel?> UpdateRuleAsync(RuleEditModel payload, CancellationToken cancellationToken = default)
    {
        if (payload.Id == Guid.Empty)
        {
            throw new ArgumentException("The payload must contain an identifier before it can be updated.", nameof(payload));
        }

        return _apiClient.PutAsync<RuleEditModel, RuleEditModel>($"{BasePath}/{payload.Id}", payload, cancellationToken);
    }

    public Task DeleteRuleAsync(Guid ruleId, CancellationToken cancellationToken = default) =>
        _apiClient.DeleteAsync($"{BasePath}/{ruleId}", cancellationToken);
}
