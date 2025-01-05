using System.Net.Http.Headers;
using Crefinso.DTOs;

namespace Crefinso.Services.Empleos
{
    public class JobServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public JobServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        // OBTENER TODOS LOS EMPLEOS
        public async Task<List<EmpleoResponse>> GetJobs()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "El token es nulo o inválido. Iniciar sesión"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<List<EmpleoResponse>>(
                    "api/empleos"
                );

                // SE LLAMA AL NOMBRE DEL CLIENTE PARA CADA EMPLEO
                foreach (var empleo in response)
                {
                    empleo.NombreCliente = await GetClienteNombre(empleo.ClienteID);
                }

                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER LOS EMPLEOS, POR FAVOR REINICIAR EL SISTEMA"
                );
            }
        }

        // METODO PARA LLAMAR AL NOMBRE DEL CLIENTE
        private async Task<string> GetClienteNombre(int clienteID)
        {
            try
            {
                var cliente = await _httpClient.GetFromJsonAsync<ClienteResponse>(
                    $"api/clientes/{clienteID}"
                );
                return cliente?.Nombre ?? "Desconocido";
            }
            catch (Exception)
            {
                // Manejo de errores al obtener el nombre del cliente
                return "Error al obtener el nombre del cliente";
            }
        }

        // OBTENER EMPLEO POR ID
        public async Task<EmpleoResponse> GetJobById(int jobId)
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
                var response = await _httpClient.GetFromJsonAsync<EmpleoResponse>(
                    $"api/empleos/{jobId}"
                );

                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER EL EMPLEO, POR FAVOR REINICIAR EL SISTEMA"
                );
            }
        }

        // CREAR NUEVO EMPLEO
        public async Task<bool> PostJob(EmpleoRequest newJob)
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

                // Enviar la solicitud POST con los datos del nuevo empleo
                var response = await _httpClient.PostAsJsonAsync("api/empleos", newJob);

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
                        $"Error al crear el Empleo. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL CREAR EL EMPLEO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // MODIFICAR UN EMPLEO
        public async Task<bool> UpdateJob(EmpleoResponse job)
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
                    job.EmpleoId,
                    job.ClienteID,
                    job.LugarTrabajo,
                    job.Cargo,
                    job.SueldoBase,
                    job.FechaIngreso,
                    job.TelefonoTrabajo,
                    job.DireccionTrabajo,
                };

                var response = await _httpClient.PutAsJsonAsync(
                    $"api/empleos/{job.EmpleoId}",
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
                        $"Error al actualizar el Empleo. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL ACTUALIZAR EL EMPLEO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // ELIMINAR UN EMPLEO
        public async Task<bool> DeleteJob(int jobId)
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
                var response = await _httpClient.DeleteAsync($"api/empleos/{jobId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al eliminar el empleo. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL DESHABILITAR EL EMPLEO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }
    }
}
