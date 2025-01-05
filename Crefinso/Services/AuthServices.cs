using System.IdentityModel.Tokens.Jwt;
using Crefinso.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Crefinso.Services
{
    public class AuthServices
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly HttpClient _httpClient;
        private const string TokenKey = "token";
        private const string UsernameKey = "username";
        private const string RoleKey = "role";
        private string _token;
        private string _username;
        private string _role;

        public AuthServices(ProtectedLocalStorage localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(UserSession userSession)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/login", userSession);
                response.EnsureSuccessStatusCode();

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    await SetTokenAsync(loginResponse.Token);
                    await SetUsernameAsync(loginResponse.Username);
                    await SetRoleAsync(loginResponse.Role);
                }

                return loginResponse;
            }
            catch (Exception)
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

        public async Task<string> GetTokenAsync()
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

        public async Task SetUsernameAsync(string username)
        {
            _username = username;
            await _localStorage.SetAsync(UsernameKey, _username);
        }

        public async Task<string> GetUsernameAsync()
        {
            if (string.IsNullOrEmpty(_username))
            {
                var localStorageResult = await _localStorage.GetAsync<string>(UsernameKey);
                if (!localStorageResult.Success || string.IsNullOrEmpty(localStorageResult.Value))
                {
                    _username = null;
                    return null;
                }
                _username = localStorageResult.Value;
            }

            return _username;
        }

        public async Task SetRoleAsync(string role)
        {
            _role = role;
            await _localStorage.SetAsync(RoleKey, _role);
        }

        public async Task<string> GetRoleAsync()
        {
            if (string.IsNullOrEmpty(_role))
            {
                var localStorageResult = await _localStorage.GetAsync<string>(RoleKey);
                if (!localStorageResult.Success || string.IsNullOrEmpty(localStorageResult.Value))
                {
                    _role = null;
                    return null;
                }
                _role = localStorageResult.Value;
            }

            return _role;
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
            _username = null;
            _role = null;
            await _localStorage.DeleteAsync(TokenKey);
            await _localStorage.DeleteAsync(UsernameKey);
            await _localStorage.DeleteAsync(RoleKey);
        }
    }
}
