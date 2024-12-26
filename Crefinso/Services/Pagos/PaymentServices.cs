using Crefinso.DTOs;
using System.Net.Http.Headers;

namespace Crefinso.Services.Pagos
{
    public class PaymentServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public PaymentServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        // OBTENER TODOS LOS PAGOS
        public async Task<List<PagoResponse>> GetPayments()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("El token es nulo o inválido. Iniciar sesión");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<List<PagoResponse>>("api/pagos");

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LOS PAGOS, POR FAVOR REINICIAR EL SISTEMA");
            }
        }

        // OBTENER PAGO POR ID
        public async Task<PagoResponse> GetPaymentById(int paymentId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<PagoResponse>($"api/pagos/{paymentId}");

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER EL PAGO, POR FAVOR REINICIAR EL SISTEMA");
            }
        }

        // CREAR NUEVO PAGO
        public async Task<bool> PostPayment(PagoRequest newPayment)
        {
            try
            {
                // Obtener el token de autenticación
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }

                // Agregar el token al encabezado de autorización
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Enviar la solicitud POST con los datos del nuevo pago
                var response = await _httpClient.PostAsJsonAsync("api/pagos", newPayment);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return true; // Indica que la creación fue exitosa
                }
                else
                {
                    // Manejar errores de la respuesta
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear el Pago. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL CREAR EL PAGO, POR FAVOR REINICIAR EL SISTEMA. Detalle: " + ex.Message);
            }
        }

        // MODIFICAR UN PAGO
        public async Task<bool> UpdatePayment(PagoResponse payment)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Construir el contenido a enviar en la solicitud
                var data = new
                {
                    payment.PagoId,
                    payment.PrestamoId,
                    payment.FechaPago,
                    payment.MontoPagado,
                    payment.SaldoAcumulado,
                    payment.Estado
                };

                var response = await _httpClient.PutAsJsonAsync($"api/pagos/{payment.PagoId}", data);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al actualizar el Pago. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL ACTUALIZAR EL PAGO, POR FAVOR REINICIAR EL SISTEMA. Detalle: " + ex.Message);
            }
        }

        // ELIMINAR UN PAGO
        public async Task<bool> DeletePayment(int paymentId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"api/pagos/{paymentId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al eliminar el pago. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL DESHABILITAR EL PAGO, POR FAVOR REINICIAR EL SISTEMA. Detalle: " + ex.Message);
            }
        }
    }
}