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

                // Obtener préstamos y clientes
                var prestamos = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>(
                    "api/prestamos"
                );
                var clientes = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>(
                    "api/clientes"
                );

                // Combinar préstamos con clientes para obtener el nombre
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
                            clientes?.FirstOrDefault(c => c.ClienteId == p.SolicitudId)?.Nombre
                            ?? "Desconocido",
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

        // Método para obtener próximos pagos con el nombre del cliente
        public async Task<List<PagoResponseWithCliente>> GetUpcomingPaymentsAsync()
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

                // Obtener pagos y clientes
                var pagos = await _httpClient.GetFromJsonAsync<List<PagoResponse>>("api/pagos");
                var clientes = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>(
                    "api/clientes"
                );

                // Combinar pagos con clientes para obtener el nombre
                var upcomingPayments = pagos
                    ?.Where(p => p.FechaPago >= DateOnly.FromDateTime(DateTime.Today))
                    .OrderBy(p => p.FechaPago)
                    .Take(3)
                    .Select(p => new PagoResponseWithCliente
                    {
                        PagoId = p.PagoId,
                        PrestamoId = p.PrestamoId,
                        FechaPago = p.FechaPago,
                        MontoPagado = p.MontoPagado,
                        SaldoAcumulado = p.SaldoAcumulado,
                        Estado = p.Estado,
                        ClienteNombre =
                            clientes?.FirstOrDefault(c => c.ClienteId == p.PrestamoId)?.Nombre
                            ?? "Desconocido",
                    })
                    .ToList();

                return upcomingPayments ?? new List<PagoResponseWithCliente>();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Error al obtener los próximos pagos", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "HA OCURRIDO UN ERROR AL OBTENER LOS PRÓXIMOS PAGOS, POR FAVOR REINICIAR EL SISTEMA",
                    ex
                );
            }
        }
    }
}
