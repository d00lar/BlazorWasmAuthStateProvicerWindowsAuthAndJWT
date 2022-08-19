using BlazorWasmWIndowsAuth.Client.Core.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWasmWIndowsAuth.Client.Core.Providers
{
    public class MyAuthStateProvider: AuthenticationStateProvider
    {
        private readonly UserService _us;

        public MyAuthStateProvider(UserService us)
        {
            this._us = us;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_us.User?.Claims?.Count() > 0) return new AuthenticationState(this._us.User);


            var identity = new ClaimsIdentity();
            try
            {
                if (_us.UserModel is null) await _us.GetUser();

                if (_us.UserModel != null)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, _us.UserModel!.Login!) };
                    foreach (string role in _us.UserModel.Roles)
                    {

                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            this._us.User = new ClaimsPrincipal(identity);
            return new AuthenticationState(this._us.User);
        }
    }
}
