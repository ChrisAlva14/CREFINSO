using System.Net.Http.Headers;
using Crefinso.DTOs;

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
                    throw new InvalidOperationException(
                        "El token es nulo o invalido. Iniciar sesion."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>(
                    "api/clientes"
                );

                // Contar la cantidad de clientes en la lista y devolver ese valor
                return response?.Count ?? 0;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException(
                    "Error al obtener el número de clientes activos",
                    ex
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER LOS CLIENTES, POR FAVOR REINICIAR EL SISTEMA",
                    ex
                );
            }
        }

        public async Task<int> GetActiveLoansCountAsync()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "El token es nulo o invalido. Iniciar sesion."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>(
                    "api/prestamos"
                );

                // Contar la cantidad de préstamos en la lista y devolver ese valor
                return response?.Count ?? 0;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException(
                    "Error al obtener el número de préstamos activos",
                    ex
                );
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER LOS PRÉSTAMOS, POR FAVOR REINICIAR EL SISTEMA",
                    ex
                );
            }
        }

        // Método para obtener préstamos recientes con el nombre del cliente
        public async Task<List<PrestamoResponseWithCliente>> GetRecentLoansAsync()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "El token es nulo o invalido. Iniciar sesion."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                // Obtener préstamos y solicitudes
                var prestamos = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>(
                    "api/prestamos"
                );
                var solicitudes = await _httpClient.GetFromJsonAsync<List<SolicitudResponse>>(
                    "api/solicitudes"
                );
                var clientes = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>(
                    "api/clientes"
                );

                // Combinar préstamos con solicitudes y clientes para obtener el nombre
                var recentLoans = prestamos
                    ?.OrderByDescending(p => p.FechaInicio)
                    .Take(3)
                    .Select(p => new PrestamoResponseWithCliente
                    {
                        PrestamoId = p.PrestamoId,
                        SolicitudId = p.SolicitudId,
                        MontoAprobado = p.MontoAprobado,
                        TasaInteres = p.TasaInteres,
                        FechaInicio = p.FechaInicio,
                        FechaVencimiento = p.FechaVencimiento,
                        Estado = p.Estado,
                        ClienteNombre =
                            clientes
                                ?.FirstOrDefault(c =>
                                    c.ClienteId
                                    == solicitudes
                                        ?.FirstOrDefault(s => s.SolicitudId == p.SolicitudId)
                                        ?.ClienteID
                                )
                                ?.Nombre ?? "Desconocido",
                    })
                    .ToList();

                return recentLoans ?? new List<PrestamoResponseWithCliente>();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error al obtener los préstamos recientes", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER LOS PRÉSTAMOS RECIENTES, POR FAVOR REINICIAR EL SISTEMA",
                    ex
                );
            }
        }

        // Método para obtener pagos realizados con el nombre del cliente
        public async Task<List<PagoResponseWithCliente>> GetCompletedPaymentsAsync()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("El token es nulo o inválido. Iniciar sesión.");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Obtener todos los pagos desde la API
                var pagos = await _httpClient.GetFromJsonAsync<List<PagoResponse>>("api/pagos");

                if (pagos == null || !pagos.Any())
                {
                    return new List<PagoResponseWithCliente>(); // Si no hay pagos, retornar una lista vacía
                }

                // Obtener préstamos, solicitudes y clientes para asociar los nombres
                var prestamos = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>("api/prestamos");
                var solicitudes = await _httpClient.GetFromJsonAsync<List<SolicitudResponse>>("api/solicitudes");
                var clientes = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>("api/clientes");

                // Lista para almacenar los pagos realizados con el nombre del cliente
                var completedPayments = new List<PagoResponseWithCliente>();

                foreach (var pago in pagos)
                {
                    // Obtener el préstamo asociado al pago
                    var prestamo = prestamos?.FirstOrDefault(p => p.PrestamoId == pago.PrestamoId);
                    if (prestamo == null) continue;

                    // Obtener la solicitud asociada al préstamo
                    var solicitud = solicitudes?.FirstOrDefault(s => s.SolicitudId == prestamo.SolicitudId);
                    if (solicitud == null) continue;

                    // Obtener el cliente asociado a la solicitud
                    var cliente = clientes?.FirstOrDefault(c => c.ClienteId == solicitud.ClienteID);
                    var clienteNombre = cliente?.Nombre ?? "Desconocido";

                    // Agregar el pago a la lista
                    completedPayments.Add(new PagoResponseWithCliente
                    {
                        PagoId = pago.PagoId,
                        PrestamoId = pago.PrestamoId,
                        FechaPago = pago.FechaPago,
                        MontoAPagar = pago.MontoAPagar,
                        MontoPagado = pago.MontoPagado,
                        SaldoRestante = pago.SaldoRestante,
                        Estado = pago.Estado,
                        ClienteNombre = clienteNombre,
                    });
                }

                // Ordenar por fecha de pago (del más reciente al más antiguo) y tomar los 3 más recientes
                return completedPayments.OrderByDescending(p => p.FechaPago).Take(3).ToList();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error al obtener los pagos realizados", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("HA OCURRIDO UN ERROR AL OBTENER LOS PAGOS REALIZADOS, POR FAVOR REINICIAR EL SISTEMA", ex);
            }
        }

        //TASA DE MOROSIDAD
        public async Task<decimal> GetTasaMorosidadAsync()
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "El token es nulo o inválido. Iniciar sesión."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                // Obtener todos los préstamos activos
                var prestamos = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>(
                    "api/prestamos"
                );

                if (prestamos == null || !prestamos.Any())
                {
                    return 0; // Si no hay préstamos, la tasa de morosidad es 0
                }

                // Obtener los pagos vencidos
                var pagosVencidos = await _httpClient.GetFromJsonAsync<List<PagoResponse>>(
                    "api/pagos/vencidos"
                );

                // Calcular el número de préstamos en mora
                var prestamosEnMora = prestamos.Count(p =>
                    pagosVencidos?.Any(pv => pv.PrestamoId == p.PrestamoId) == true
                );

                // Calcular la tasa de morosidad
                var tasaMorosidad = (decimal)prestamosEnMora / prestamos.Count * 100;

                return Math.Round(tasaMorosidad, 2); // Redondear a 2 decimales
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error al calcular la tasa de morosidad", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL CALCULAR LA TASA DE MOROSIDAD, POR FAVOR REINICIAR EL SISTEMA",
                    ex
                );
            }
        }
    }
}
