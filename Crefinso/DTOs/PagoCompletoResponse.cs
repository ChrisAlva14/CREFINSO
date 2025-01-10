namespace Crefinso.DTOs
{
    public class PagoCompletoResponse
    {
        public int PagoId { get; set; }
        public int PrestamoId { get; set; }
        public decimal MontoPagado { get; set; }
        public DateOnly FechaPago { get; set; }
        public string Estado { get; set; }
        public PrestamoResponse Prestamo { get; set; }
        public SolicitudResponse Solicitud { get; set; }
        public ClienteResponse Cliente { get; set; }
        public UserResponse Usuario { get; set; }
    }
}
