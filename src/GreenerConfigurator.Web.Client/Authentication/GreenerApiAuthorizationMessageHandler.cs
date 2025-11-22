using System;
using System.Collections.Generic;
using GreenerConfigurator.ClientCore.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace GreenerConfigurator.Web.Client.Authentication;

/// <summary>
/// Ensures outgoing API calls include an Azure AD B2C access token for the configured backend.
/// </summary>
public sealed class GreenerApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public GreenerApiAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation,
        IOptions<GreenerApiOptions> apiOptions)
        : base(provider, navigation)
    {
        var options = apiOptions.Value;
        var authorizedUrls = new List<string>();

        if (options.BaseAddress != null)
        {
            var baseUri = options.BaseAddress;
            var authorizedBase = $"{baseUri.Scheme}://{baseUri.Authority}";
            authorizedUrls.Add(authorizedBase);
        }
        else
        {
            authorizedUrls.Add(navigation.BaseUri);
        }

        var scopes = options.Scopes?.Length > 0 ? options.Scopes : Array.Empty<string>();
        ConfigureHandler(authorizedUrls: authorizedUrls, scopes: scopes);
    }
}
