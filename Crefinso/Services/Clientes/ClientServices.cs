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

        //OBTENER TODOS LOS CLIENTES
        public async Task<List<ClienteResponse>> GetClientes()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
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

        //OBETENER CLIENTE POR ID POR ID
        public async Task<ClienteResponse> GetClienteById(int clienteId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<ClienteResponse>($"api/clientes/{clienteId}");

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER EL CLIENTE, POR FAVOR REINICIAR EL SISTEMA");
            }
        }

        //CREAR NUEVO CLIENTE
        public async Task<bool> PostClient(ClienteRequest newClient)
        {
            try
            {
                // Obtener el token de autenticación
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }

                // Agregar el token al encabezado de autorización
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Enviar la solicitud POST con los datos del nuevo usuario
                var response = await _httpClient.PostAsJsonAsync("api/clientes", newClient);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return true; // Indica que la creación fue exitosa
                }
                else
                {
                    // Manejar errores de la respuesta
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear el Cliente. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL CREAR EL USUARIO, POR FAVOR REINICIAR EL SISTEMA. Detalle: " + ex.Message);
            }
        }

        //MODIFICAR UN CLIENTE
        public async Task<bool> UpdateClient(ClienteResponse client)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Construir el contenido a enviar en la solicitud
                var data = new
                {
                    client.ClienteId,
                    client.Nombre,
                    client.FechaNacimiento,
                    client.DUI,
                    client.NIT,
                    client.Direccion,
                    client.TelefonoCelular,
                    client.TelefonoFijo,
                    client.UserID,
                    client.Estado
                };

                var response = await _httpClient.PutAsJsonAsync($"api/clientes/{client.ClienteId}", data);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al actualizar el Cliente. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL ACTUALIZAR EL CLIENTE, POR FAVOR REINICIAR EL SISTEMA. Detalle: " + ex.Message);
            }
        }

        //ELIMINAR UN CLIENTE
        public async Task<bool> DeleteCliente(int clienteId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"api/clientes/{clienteId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al eliminar el cliente. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("ERROR EN LA SOLICITUD HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL DESHABILITAR EL CLIENTE, POR FAVOR REINICIAR EL SISTEMA. Detalle: " + ex.Message);
            }
        }
    }
}
