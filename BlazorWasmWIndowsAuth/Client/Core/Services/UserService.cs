using Blazored.LocalStorage;
using BlazorWasmWIndowsAuth.Client.Core.HttpClients;
using BlazorWasmWIndowsAuth.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;

namespace BlazorWasmWIndowsAuth.Client.Core.Services
{
    public class UserService
    {
        private readonly UserServiceClient _usc;
        private readonly ISyncLocalStorageService _sls;

        public ClaimsPrincipal User { get; set; } = new(new ClaimsIdentity());

        public UserService(UserServiceClient USC, ISyncLocalStorageService sls )
        {
            _usc = USC;
            _sls = sls;
        }
        private UserModel? userModel;
        public UserModel? UserModel
        {
            get
            {
                return userModel;
            }
            set
            {
                userModel = value;
            }
        }


        public async Task<UserModel?> GetUser()
        {
            try
            {
                userModel = await _usc.GetUser();
                this._sls.SetItem<string>("JWT", userModel!.JWT!);
                //Console.WriteLine(userModel!.JWT!);
                return userModel!;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
