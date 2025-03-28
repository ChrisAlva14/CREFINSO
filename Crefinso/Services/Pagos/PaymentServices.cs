using System.Net.Http.Headers;
using ClosedXML.Excel;
using Crefinso.DTOs;
using Microsoft.EntityFrameworkCore; 

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

        // Método para calcular el interés basado en el saldo actual
        public decimal CalculateInterest(decimal currentBalance, decimal annualRate, int period)
        {
            decimal monthlyRate = annualRate / 12 / 100;
            decimal interest = currentBalance * monthlyRate * period;
            return Math.Round(interest, 2, MidpointRounding.AwayFromZero); 
        }

        // Método para calcular el capital basado en el saldo actual
        public decimal CalculateCapital(
            decimal currentBalance,
            decimal paymentAmount,
            decimal annualRate
        )
        {
            decimal interest = CalculateInterest(currentBalance, annualRate, 1);
            decimal capital = paymentAmount - interest;
            return Math.Round(capital, 2, MidpointRounding.AwayFromZero); 
        }

        // Método para calcular el nuevo saldo
        public decimal CalculateNewBalance(decimal currentBalance, decimal capital)
        {
            decimal newBalance = currentBalance - capital;
            return Math.Round(newBalance, 2, MidpointRounding.AwayFromZero); 
        }

        // OBTENER TODOS LOS PAGOS
        public async Task<List<PagoResponse>> GetPayments()
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
                var response = await _httpClient.GetFromJsonAsync<List<PagoResponse>>("api/pagos");
                return response ?? new List<PagoResponse>();
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pagos. Detalle: " + ex.Message);
            }
        }

        // OBTENER PAGO POR ID
        // OBTENER PAGO POR ID
        public async Task<PagoResponse> GetPaymentById(int paymentId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "Token inválido o nulo. Por favor, iniciar sesión."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<PagoResponse>(
                    $"api/pagos/{paymentId}"
                );
                return response ?? throw new Exception("Pago no encontrado.");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el pago. Detalle: " + ex.Message);
            }
        }

        // CREAR NUEVO PAGO
        public async Task<bool> PostPayment(PagoRequest newPayment)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "Token inválido o nulo. Por favor, iniciar sesión."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.PostAsJsonAsync("api/pagos", newPayment);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear el pago: {errorMessage}");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el pago. Detalle: " + ex.Message);
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
                    throw new InvalidOperationException(
                        "Token inválido o nulo. Por favor, iniciar sesión."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.PutAsJsonAsync(
                    $"api/pagos/{payment.PagoId}",
                    payment
                );

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar el pago: {errorMessage}");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el pago. Detalle: " + ex.Message);
            }
        }

        // OBTENER PAGOS FUTUROS
        public async Task<List<PagoFuturoResponse>> GetPagosFuturos(int prestamoId)
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "Token inválido o nulo. Por favor, iniciar sesión."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<List<PagoFuturoResponse>>(
                    $"api/pagos/futuros/{prestamoId}"
                );

                return response ?? new List<PagoFuturoResponse>();
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pagos futuros. Detalle: " + ex.Message);
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
                    throw new InvalidOperationException(
                        "TOKEN INVÁLIDO O NULO, POR FAVOR, INICIAR SESIÓN"
                    );
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.DeleteAsync($"api/pagos/{paymentId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"Error al eliminar el pago. Código de estado: {response.StatusCode}. Detalle: {errorMessage}"
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
                    "HA OCURRIDO UN ERROR AL DESHABILITAR EL PAGO, POR FAVOR REINICIAR EL SISTEMA. Detalle: "
                        + ex.Message
                );
            }
        }

        // OBTENER EL PAGO COMPLETO
        public async Task<List<PagoCompletoResponse>> GetPagosCompletos()
        {
            try
            {
                // Obtener todos los datos necesarios
                var pagos = await _httpClient.GetFromJsonAsync<List<PagoResponse>>("api/pagos");
                var prestamos = await _httpClient.GetFromJsonAsync<List<PrestamoResponse>>(
                    "api/prestamos"
                );
                var solicitudes = await _httpClient.GetFromJsonAsync<List<SolicitudResponse>>(
                    "api/solicitudes"
                );
                var clientes = await _httpClient.GetFromJsonAsync<List<ClienteResponse>>(
                    "api/clientes"
                );
                var usuarios = await _httpClient.GetFromJsonAsync<List<UserResponse>>("api/users");

                // Combinar los datos
                var pagosCompletos = pagos
                    ?.Select(p => new PagoCompletoResponse
                    {
                        PagoId = p.PagoId,
                        PrestamoId = p.PrestamoId,
                        MontoPagado = p.MontoPagado,
                        InteresPagado = p.InteresPagado,
                        CapitalPagado = p.CapitalPagado,
                        SaldoRestante = p.SaldoRestante,
                        FechaPago = p.FechaPago,
                        Estado = p.Estado,
                        Prestamo = prestamos?.FirstOrDefault(pr => pr.PrestamoId == p.PrestamoId),
                        Solicitud = solicitudes?.FirstOrDefault(s =>
                            s.SolicitudId
                            == prestamos
                                ?.FirstOrDefault(pr => pr.PrestamoId == p.PrestamoId)
                                ?.SolicitudId
                        ),
                        Cliente = clientes?.FirstOrDefault(c =>
                            c.ClienteId
                            == solicitudes
                                ?.FirstOrDefault(s =>
                                    s.SolicitudId
                                    == prestamos
                                        ?.FirstOrDefault(pr => pr.PrestamoId == p.PrestamoId)
                                        ?.SolicitudId
                                )
                                ?.ClienteID
                        ),
                        Usuario = usuarios?.FirstOrDefault(u =>
                            u.UserId
                            == solicitudes
                                ?.FirstOrDefault(s =>
                                    s.SolicitudId
                                    == prestamos
                                        ?.FirstOrDefault(pr => pr.PrestamoId == p.PrestamoId)
                                        ?.SolicitudId
                                )
                                ?.UserID
                        ),
                    })
                    .ToList();

                return pagosCompletos ?? new List<PagoCompletoResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pagos completos: " + ex.Message);
            }
        }

        public async Task<List<PagoResponse>> GetPaymentsByDateRange(
            DateTime startDate,
            DateTime endDate
        )
        {
            try
            {
                var token = await _authServices.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        "Token inválido o nulo. Por favor, iniciar sesión."
                    );
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetFromJsonAsync<List<PagoResponse>>(
                    $"api/pagos?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}"
                );

                return response ?? new List<PagoResponse>();
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al obtener los pagos por rango de fechas. Detalle: " + ex.Message
                );
            }
        }

        public async Task<ReportePagosResponse> GetWeeklyReport(DateTime weekStartDate)
        {
            var weekEndDate = weekStartDate.AddDays(6); 
            var payments = await GetPaymentsByDateRange(weekStartDate, weekEndDate);

            return new ReportePagosResponse
            {
                FechaInicio = weekStartDate,
                FechaFin = weekEndDate,
                TotalPagos = payments.Count,
                TotalMontoPagado = payments.Sum(p => p.MontoPagado),
                Pagos = payments,
            };
        }

        public async Task<ReportePagosResponse> GetMonthlyReport(int year, int month)
        {
            var monthStartDate = new DateTime(year, month, 1);
            var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1); 
            var payments = await GetPaymentsByDateRange(monthStartDate, monthEndDate);

            return new ReportePagosResponse
            {
                FechaInicio = monthStartDate,
                FechaFin = monthEndDate,
                TotalPagos = payments.Count,
                TotalMontoPagado = payments.Sum(p => p.MontoPagado),
                Pagos = payments,
            };
        }

        //EXPORTAR A EXCEL
        public byte[] GenerateExcelReport(ReportePagosResponse reporte)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte de Pagos");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Fecha Inicio";
                worksheet.Cell(1, 2).Value = reporte.FechaInicio.ToShortDateString();
                worksheet.Cell(2, 1).Value = "Fecha Fin";
                worksheet.Cell(2, 2).Value = reporte.FechaFin.ToShortDateString();
                worksheet.Cell(3, 1).Value = "Total Pagos";
                worksheet.Cell(3, 2).Value = reporte.TotalPagos;
                worksheet.Cell(4, 1).Value = "Total Monto Pagado";
                worksheet.Cell(4, 2).Value = reporte.TotalMontoPagado;

                // Detalles de los pagos
                worksheet.Cell(6, 1).Value = "Codigo Pago";
                worksheet.Cell(6, 2).Value = "Monto Pagado";
                worksheet.Cell(6, 3).Value = "Fecha Pago";
                worksheet.Cell(6, 4).Value = "Estado";

                int row = 7;
                foreach (var pago in reporte.Pagos)
                {
                    worksheet.Cell(row, 1).Value = pago.PagoId;
                    worksheet.Cell(row, 2).Value = pago.MontoPagado;
                    worksheet.Cell(row, 3).Value = pago.FechaPago.ToShortDateString();
                    worksheet.Cell(row, 4).Value = pago.Estado;
                    row++;
                }

                // Guardar en un MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task ProcesarPagosAutomaticosAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("api/pagos/procesar-automaticos", null);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al procesar pagos automáticos: {ex.Message}");
            }
        }
    }
}
