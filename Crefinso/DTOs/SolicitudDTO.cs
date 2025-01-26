namespace Crefinso.DTOs
{
    public class SolicitudResponse
    {
        public int SolicitudId { get; set; }

        public int ClienteID { get; set; }

        public string NombreCliente { get; set; } = null!;

        public DateOnly FechaSolicitud { get; set; }

        public decimal CantidadSolicitada { get; set; }

        public string DestinoPrestamo { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public DateOnly FechaAnalisis { get; set; }

        public int UserID { get; set; }
    }

    public class SolicitudRequest
    {
        public int SolicitudId { get; set; }

        public int ClienteID { get; set; }

        public DateOnly FechaSolicitud { get; set; }

        public decimal CantidadSolicitada { get; set; }

        public string DestinoPrestamo { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public DateOnly FechaAnalisis { get; set; }

        public int UserID { get; set; }
    }
}
