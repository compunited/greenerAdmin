using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace GreenerConfigurator.Web.Client
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, 
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://api.greener.software" },
                scopes: new[] { "openid", "offline_access", "https://greenerswkunden.onmicrosoft.com/helloapi/demo.read" });
        }
    }
}
