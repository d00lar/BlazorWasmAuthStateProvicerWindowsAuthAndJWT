using Blazored.LocalStorage;
using BlazorWasmWIndowsAuth.Client;
using BlazorWasmWIndowsAuth.Client.Core.HttpClients;
using BlazorWasmWIndowsAuth.Client.Core.Providers;
using BlazorWasmWIndowsAuth.Client.Core.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationStateProvider, MyAuthStateProvider>();
builder.Services.AddScoped<IAccessTokenProvider, MyIAccessTokenProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<UserServiceClient>((serviceProvider, client) => {
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});


builder.Services.AddHttpClient<IHttpService, HttpService>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});



await builder.Build().RunAsync();
