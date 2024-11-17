namespace Crefinso.DTOs
{
    public class ClienteResponse
    {
        public int ClienteId { get; set; }

        public string Nombre { get; set; } = null!;

        public DateOnly FechaNacimiento { get; set; }

        public string DUI { get; set; } = null!;

        public string NIT { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string TelefonoCelular { get; set; } = null!;

        public string TelefonoFijo { get; set; } = null!;

        public int UserID { get; set; }
    }
}
