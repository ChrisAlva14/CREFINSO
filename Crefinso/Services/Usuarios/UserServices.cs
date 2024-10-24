using System.Net.Http.Headers;
using Crefinso.DTOs;

namespace Crefinso.Services.Usuarios
{
    public class UserServices
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServices _authServices;

        public UserServices(HttpClient httpClient, AuthServices authServices)
        {
            _httpClient = httpClient;
            _authServices = authServices;
        }

        public async Task<List<UserResponse>> GetUsers() 
        {
            try
            {
                var token = await _authServices.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");

                return response;
            }

            catch (HttpRequestException ex)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LOS USUARIOS, POR FAVOR REINICIAR EL SISTEMA");
            }
        
        }

        public async Task<bool> PostUser(UserRequest newUser)
        {
            try
            {
                // Obtener el token de autenticación
                var token = await _authServices.GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("TOKEN INVALIDO O NULO, POR FAVOR, INICIAR SESIÓN");
                }

                // Agregar el token al encabezado de autorización
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Enviar la solicitud POST con los datos del nuevo usuario
                var response = await _httpClient.PostAsJsonAsync("api/users", newUser);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return true; // Indica que la creación fue exitosa
                }
                else
                {
                    // Manejar errores de la respuesta
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear el usuario. Código de estado: {response.StatusCode}. Detalle: {errorMessage}");
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

    }
}
