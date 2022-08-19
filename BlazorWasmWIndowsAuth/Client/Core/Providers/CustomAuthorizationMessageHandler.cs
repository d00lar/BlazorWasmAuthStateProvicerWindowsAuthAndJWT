using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWasmWIndowsAuth.Client.Core.Providers;

public class CustomAuthorizationMessageHandler:AuthorizationMessageHandler
{

    public CustomAuthorizationMessageHandler(IAccessTokenProvider provider , NavigationManager nav) :base(provider, nav)
    {
        ConfigureHandler(authorizedUrls: new[] { nav.BaseUri });
    }
}
