using System.Net.Http.Headers;
using Crefinso.DTOs;

namespace Crefinso.Services.Prestamos
{
    public class LoanServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public LoanServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        //OBTENER TODOS LOS PRESTAMOS
        public async Task<List<PrestamoResponse>> GetLoans()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "El token es nulo o invalido.Iniciar sesion"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>(
                    "api/prestamos"
                );

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER LOS PRESTAMOS, POR FAVOR REINICIAR EL SISTEMA"
                );
            }
        }

        // OBTENER PRÉSTAMO POR ID
        public async Task<PrestamoResponse> GetLoanById(int loanId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<PrestamoResponse>(
                    $"api/prestamos/{loanId}"
                );

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER EL PRÉSTAMO, POR FAVOR REINICIAR EL SISTEMA"
                );
            }
        }

        // CREAR NUEVO PRÉSTAMO
        public async Task<bool> PostLoan(PrestamoRequest newLoan)
        {
            try
            {
                // Obtener el token de autenticación
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }

                // Agregar el token al encabezado de autorización
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                // Enviar la solicitud POST con los datos del nuevo préstamo
                var response = await _httpClient.PostAsJsonAsync("api/prestamos", newLoan);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return true; // Indica que la creación fue exitosa
                }
                else
                {
                    // Manejar errores de la respuesta
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al crear el Préstamo. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL CREAR EL PRÉSTAMO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // MODIFICAR UN PRÉSTAMO
        public async Task<bool> UpdateLoan(PrestamoResponse loan)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                // Construir el contenido a enviar en la solicitud
                var data = new
                {
                    loan.PrestamoId,
                    loan.SolicitudId,
                    loan.MontoAprobado,
                    loan.TasaInteres,
                    loan.FechaInicio,
                    loan.FechaVencimiento,
                    loan.Estado,
                };

                var response = await _httpClient.PutAsJsonAsync(
                    $"api/prestamos/{loan.PrestamoId}",
                    data
                );

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al actualizar el Préstamo. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL ACTUALIZAR EL PRÉSTAMO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // ELIMINAR UN PRÉSTAMO
        public async Task<bool> DeleteLoan(int loanId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.DeleteAsync($"api/prestamos/{loanId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al eliminar el préstamo. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL DESHABILITAR EL PRÉSTAMO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }
    }
}
