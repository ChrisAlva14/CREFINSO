namespace Crefinso.DTOs
{
    public class PrestamoResponse
    {
        public int PrestamoId { get; set; }

        public int SolicitudId { get; set; }

        public string MontoAprobado { get; set; }

        public string TasaInteres { get; set; }

        public DateOnly FechaInicio { get; set; }

        public DateOnly FechaVencimiento { get; set; }

        public string Estado { get; set; }
    }

    public class PrestamoRequest
    {
        public int PrestamoId { get; set; }

        public int SolicitudId { get; set; }

        public string MontoAprobado { get; set; }

        public string TasaInteres { get; set; }

        public DateOnly FechaInicio { get; set; }

        public DateOnly FechaVencimiento { get; set; }

        public string Estado { get; set; }
    }
}
