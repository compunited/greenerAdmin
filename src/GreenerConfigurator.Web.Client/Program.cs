using GreenerConfigurator.ClientCore.Extensions;
using GreenerConfigurator.ClientCore.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GreenerConfigurator.Web.Client;
using GreenerConfigurator.Web.Client.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddGreenerClientCore(options =>
{
    var configuredBase = builder.Configuration["Api:BaseAddress"];
    if (!string.IsNullOrWhiteSpace(configuredBase) && Uri.TryCreate(configuredBase, UriKind.Absolute, out var parsed))
    {
        options.BaseAddress = parsed;
    }
    else
    {
        options.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    }

    options.Scopes = builder.Configuration.GetSection("AzureAdB2C:ApiScopes").Get<string[]>() ?? Array.Empty<string>();
});

builder.Services.AddScoped<GreenerApiAuthorizationMessageHandler>();
builder.Services.AddHttpClient("GreenerApi", (sp, client) =>
    {
        var apiOptions = sp.GetRequiredService<IOptions<GreenerApiOptions>>().Value;
        client.BaseAddress = apiOptions.BaseAddress ?? new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<GreenerApiAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GreenerApi"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

    var apiScopes = builder.Configuration.GetSection("AzureAdB2C:ApiScopes").Get<string[]>();
    if (apiScopes is { Length: > 0 })
    {
        foreach (var scope in apiScopes)
        {
            options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
        }
    }
});

await builder.Build().RunAsync();
