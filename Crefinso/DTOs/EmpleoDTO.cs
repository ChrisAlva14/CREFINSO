namespace Crefinso.DTOs
{
    public class EmpleoResponse
    {
        public int EmpleoId { get; set; }

        public int ClienteID { get; set; }

        public string? NombreCliente { get; set; }

        public string? LugarTrabajo { get; set; }

        public string? Cargo { get; set; }

        public string? SueldoBase { get; set; }

        public DateOnly FechaIngreso { get; set; }

        public string? TelefonoTrabajo { get; set; }

        public string? DireccionTrabajo { get; set; }
    }

    public class EmpleoRequest
    {
        public int EmpleoId { get; set; }

        public int ClienteID { get; set; }

        public string? LugarTrabajo { get; set; }

        public string? Cargo { get; set; }

        public string? SueldoBase { get; set; }

        public DateOnly FechaIngreso { get; set; }

        public string? TelefonoTrabajo { get; set; }

        public string? DireccionTrabajo { get; set; }
    }
}
