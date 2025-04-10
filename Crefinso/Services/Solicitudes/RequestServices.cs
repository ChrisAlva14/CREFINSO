using System.Net.Http.Headers;
using Crefinso.DTOs;

namespace Crefinso.Services.Solicitudes
{
    public class RequestServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public RequestServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        //OBTENER TODAS LAS SOLICITUDES
        public async Task<List<SolicitudResponse>> GetSolicitudes()
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
                var response = await _httpClient.GetFromJsonAsync<List<SolicitudResponse>>(
                    "api/solicitudes"
                );

                //SE LLAMA AL NOMBRE DEL CLIENTE
                foreach (var solicitud in response)
                {
                    solicitud.NombreCliente = await GetClienteNombre(solicitud.ClienteID);
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
                    "HA OCURRIDO UN ERROR AL OBTENER LAS SOLICITUDES, POR FAVOR REINICIAR EL SISTEMA"
                );
            }
        }

        // METODO PARA LLAMAR AL NOMBRE DEL CLIENTE
        private async Task<string> GetClienteNombre(int clienteID)
        {
            var cliente = await _httpClient.GetFromJsonAsync<ClienteResponse>(
                $"api/clientes/{clienteID}"
            );
            return cliente?.Nombre ?? "Desconocido";
        }

        //OBETENER SOLICITUD POR ID POR ID
        public async Task<SolicitudResponse> GetSolicitudById(int solicitudId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<SolicitudResponse>(
                    $"api/solicitudes/{solicitudId}"
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
                    "HA OCURRIDO UN ERROR AL OBTENER LA SOLICITUD, POR FAVOR REINICIAR EL SISTEMA"
                );
            }
        }

        //CREAR SOLICITUD
        public async Task<bool> PostSolicitud(SolicitudRequest newSolicitud)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                var response = await _httpClient.PostAsJsonAsync("api/solicitudes", newSolicitud);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al crear la Solicitud. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL CREAR LA SOLICITUD, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // MODIFICAR SOLICITUD
        public async Task<bool> UpdateSolicitud(SolicitudResponse solicitud)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                // Construir el contenido a enviar en la solicitud
                var data = new
                {
                    solicitud.ClienteID,
                    solicitud.FechaSolicitud,
                    solicitud.CantidadSolicitada,
                    solicitud.DestinoPrestamo,
                    solicitud.Estado,
                    solicitud.FechaAnalisis,
                    solicitud.UserID,
                };

                var response = await _httpClient.PutAsJsonAsync(
                    $"api/solicitudes/{solicitud.SolicitudId}",
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
                        $"Error al actualizar la Solicitud. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL ACTUALIZAR LA SOLICITUD, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        //ELIMINAR SOLICITUD
        public async Task<bool> DeleteSolicitud(int solicitudId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.DeleteAsync($"api/solicitudes/{solicitudId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al eliminar la Solicitud. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL DESHABILITAR LA SOLICITUD, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // En tu RequestServices.cs, añade este nuevo método
        public async Task<List<SolicitudResponse>> GetSolicitudesAprobadas()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("El token es nulo o invalido. Iniciar sesion");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Obtener todas las solicitudes
                var response = await _httpClient.GetFromJsonAsync<List<SolicitudResponse>>("api/solicitudes");

                // Filtrar solo las aprobadas
                var solicitudesAprobadas = response.Where(s => s.Estado == "Aprobado").ToList();

                // Obtener nombres de clientes para las solicitudes aprobadas
                foreach (var solicitud in solicitudesAprobadas)
                {
                    solicitud.NombreCliente = await GetClienteNombre(solicitud.ClienteID);
                }

                return solicitudesAprobadas;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LAS SOLICITUDES APROBADAS");
            }
        }
    }
}
