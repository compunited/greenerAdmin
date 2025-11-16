using GreenerConfigurator.ClientCore.Extensions;
using GreenerConfigurator.ClientCore.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GreenerConfigurator.Web.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

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
});

await builder.Build().RunAsync();
