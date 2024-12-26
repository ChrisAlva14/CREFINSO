using Crefinso.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;

namespace Crefinso.Services
{
    public class AuthServices
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly HttpClient _httpClient;
        private const string TokenKey = "token";
        private string? _token;

        public AuthServices(ProtectedLocalStorage localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task<string?> LoginAsync(UserSession userSession)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/login", userSession);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<string>();
            }
            catch (Exception ex)
            {
                // Manejar el error (registro, reintento, etc.)
                return null;
            }
        }

        public async Task SetTokenAsync(string token)
        {
            _token = token;
            await _localStorage.SetAsync(TokenKey, _token);
        }

        public async Task<string?> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                var localStorageResult = await _localStorage.GetAsync<string>(TokenKey);
                if (!localStorageResult.Success || string.IsNullOrEmpty(localStorageResult.Value))
                {
                    _token = null;
                    return null;
                }
                _token = localStorageResult.Value;
            }

            return _token;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token) && !IsTokenExpired(token);
        }

        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }

        public async Task LogoutAsync()
        {
            _token = null;
            await _localStorage.DeleteAsync(TokenKey);
        }
    }
}
