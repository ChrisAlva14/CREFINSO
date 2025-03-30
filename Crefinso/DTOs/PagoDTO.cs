namespace Crefinso.DTOs
{
    public class PagoResponse
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public decimal MontoAPagar { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal InteresPagado { get; set; }

        public decimal CapitalPagado { get; set; }

        public decimal SaldoRestante { get; set; }

        public string Estado { get; set; }

        public int ClienteId { get; set; }
    }

    public class PagoRequest
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public decimal MontoAPagar { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal InteresPagado { get; set; }

        public decimal CapitalPagado { get; set; }

        public decimal SaldoRestante { get; set; }

        public string Estado { get; set; }
    }

    public class PagoFuturoResponse
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public decimal MontoAPagar { get; set; }

        public decimal SaldoRestante { get; set; }

        public decimal InteresPagado { get; set; }

        public decimal CapitalPagado { get; set; }

        public string Estado { get; set; }

        public decimal MontoPagado { get; set; }

        public string ClienteNombre { get; set; }

        public bool PuedePagar { get; set; }
    }

    public class ReportePagosResponse
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int TotalPagos { get; set; }

        public decimal TotalMontoPagado { get; set; }

        public List<PagoResponse> Pagos { get; set; }
    }
}
