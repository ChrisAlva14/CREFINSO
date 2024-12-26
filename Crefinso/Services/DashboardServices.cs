using Crefinso.DTOs;
using System.Net.Http.Headers;

namespace Crefinso.Services
{
    public class DashboardServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public DashboardServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        public async Task<int> GetActiveClientsCountAsync()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("El token es nulo o invalido. Iniciar sesion.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>("api/clientes");

                // Contar la cantidad de clientes en la lista y devolver ese valor
                return response?.Count ?? 0;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("Error al obtener el número de clientes activos", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LOS CLIENTES, POR FAVOR REINICIAR EL SISTEMA", ex);
            }
        }

        public async Task<int> GetActiveLoansCountAsync()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("El token es nulo o invalido. Iniciar sesion.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>("api/prestamos");

                // Contar la cantidad de préstamos en la lista y devolver ese valor
                return response?.Count ?? 0;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("Error al obtener el número de préstamos activos", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LOS PRÉSTAMOS, POR FAVOR REINICIAR EL SISTEMA", ex);
            }
        }
    }
}
