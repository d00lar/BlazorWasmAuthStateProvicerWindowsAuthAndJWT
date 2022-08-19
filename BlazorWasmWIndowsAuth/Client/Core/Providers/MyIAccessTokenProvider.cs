using Blazored.LocalStorage;
using BlazorWasmWIndowsAuth.Client.Core.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWasmWIndowsAuth.Client.Core.Providers
{
    public class MyIAccessTokenProvider : IAccessTokenProvider
    {
        private readonly ISyncLocalStorageService _sls;

        public MyIAccessTokenProvider(ISyncLocalStorageService sls)
        {
            this._sls = sls;
        }
        public ValueTask<AccessTokenResult> RequestAccessToken()
        {
            AccessToken token = new AccessToken();
            token.Value = _sls.GetItemAsString("JWT").Replace("\"","");// no idea why but barer mus be without "" in header.
            AccessTokenResult resutlt = new AccessTokenResult(AccessTokenResultStatus.Success, token, "");
            return new ValueTask<AccessTokenResult>(resutlt);
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            return this.RequestAccessToken();
        }
    }
}
