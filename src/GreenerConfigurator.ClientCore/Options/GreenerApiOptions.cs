namespace GreenerConfigurator.ClientCore.Options;

/// <summary>
/// Holds the configuration required for calling the Greener backend API.
/// </summary>
public sealed class GreenerApiOptions
{
    /// <summary>
    /// Gets or sets the base URI that every request should be relative to.
    /// </summary>
    public Uri? BaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the scopes that should be requested when acquiring API access tokens.
    /// </summary>
    public string[] Scopes { get; set; } = Array.Empty<string>();
}
