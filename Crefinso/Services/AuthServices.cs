using Crefinso.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;

namespace Crefinso.Services
{
    public class AuthServices
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly HttpClient _httpClient;
        private string? _token;

        public AuthServices(ProtectedLocalStorage localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        //ENVIAR DATOS A ENDPOINT LOGIN
        public async Task<string> Login(UserSession userSession)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/login", userSession);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<string>();

                return result;
            }

            return null;
        }

        //GUARDAR TOKEN EN EL NAVEGADOR
        public async Task SetToken(string token)
        {
            _token = token;
            await _localStorage.SetAsync("token", _token);
        }

        //OBTENER TOKEN
        public async Task<string> GetToken()
        {
            var localStorageResult = await _localStorage.GetAsync<string>("token");
            if (string.IsNullOrEmpty(_token))
            {
                if (!localStorageResult.Success || string.IsNullOrEmpty(localStorageResult.Value))
                {
                    _token = null;
                    return null;
                }
                _token = localStorageResult.Value;
            }

            return _token;
        }

        //VERIFICAR SI EL USUARIO ESTÁ AUTENTICADO
        public async Task<bool> IsAuthenticated()
        {
            var token = await GetToken();

            return !string.IsNullOrEmpty(token) && !IsTokenExpired(token);
        }

        //VERIFICAR SI EL TOKEN NO HA EXPIRADO
        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }

        //CERRAR SESION
        public async Task Logout()
        {
            _token = null;
            await _localStorage.DeleteAsync("token");
        }
    }
}
