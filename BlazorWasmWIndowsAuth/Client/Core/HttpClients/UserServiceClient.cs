using BlazorWasmWIndowsAuth.Shared;
using System.Net.Http.Json;

namespace BlazorWasmWIndowsAuth.Client.Core.HttpClients
{
    public class UserServiceClient
    {
        private readonly HttpClient _http;


        public UserServiceClient(HttpClient http)
        {
            this._http = http;
        }

        public async Task<UserModel?> GetUser()
        {
            try
            {
                UserModel? userModel = await _http.GetFromJsonAsync<UserModel>("user/GetUser");
                return userModel!;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
