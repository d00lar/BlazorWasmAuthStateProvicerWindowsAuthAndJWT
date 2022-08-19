using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlazorWasmWIndowsAuth.Client.Core.HttpClients
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri, CancellationToken token = default);
        Task<T> Post<T>(string uri, object value, CancellationToken token = default);

        Task<T> Post<T>(string uri, HttpContent content, CancellationToken token = default);
    }


    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly IConfiguration _configuration;
        private readonly ISyncLocalStorageService _sls;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            IConfiguration configuration,
            ISyncLocalStorageService sls
        )
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _configuration = configuration;
            _sls = sls;
        }

        public async Task<T> Get<T>(string uri, CancellationToken token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request, token);
        }

        public async Task<T> Post<T>(string uri, HttpContent content, CancellationToken token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = content;
            return await sendRequest<T>(request, token);
        }

        public async Task<T> Post<T>(string uri, object value, CancellationToken token = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await sendRequest<T>(request, token);
        }

        // helper methods

        private async Task<T> sendRequest<T>(HttpRequestMessage request, CancellationToken token = default)
        {

            var jwt = _sls.GetItem<string>("JWT");
            if (jwt != null)
                request!.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            using var response = await _httpClient.SendAsync(request, token);


            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {

                var error = await response.Content.ReadAsStringAsync(token);
                throw new Exception(error);
            }

            var ret = await response.Content.ReadFromJsonAsync<T>();
            if (ret is not null) return ret;
            else return default!;
        }
    }
}

