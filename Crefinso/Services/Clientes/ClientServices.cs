using Crefinso.DTOs;
using System.Net.Http.Headers;

namespace Crefinso.Services.Clientes
{
    public class ClientServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public ClientServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        public async Task<List<ClienteResponse>> GetClientes()
        {
            try
            {
                var token = await _authServices.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("El token es nulo o invalido.Iniciar sesion");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>("api/clientes");

                return response;
            }

            catch (HttpRequestException ex)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LOS CLIENTES, POR FAVOR REINICIAR EL SISTEMA");
            }

        }
    }
}
