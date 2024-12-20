namespace Crefinso.DTOs
{
    public class PagoResponse
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public string? MontoPagado { get; set; }

        public string? SaldoAcumulado { get; set; }
    }

    public class PagoRequest
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public string? MontoPagado { get; set; }

        public string? SaldoAcumulado { get; set; }
    }
}
