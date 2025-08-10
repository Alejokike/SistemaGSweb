namespace SistemaGS.DTO
{
    public class PersonaDTO
    {
        public int Cedula { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? Genero { get; set; }

        public string? Profesion { get; set; }

        public string? Ocupacion { get; set; }

        public string? LugarTrabajo { get; set; }

        public string? DireccionTrabajo { get; set; }

        public string? TelefonoTrabajo { get; set; }

        public string? DireccionHabitacion { get; set; }

        public string? TelefonoHabitacion { get; set; }

        public bool Solicitante { get; set; }

        public bool Beneficiario { get; set; }

        public bool Funcionario { get; set; }

        public bool Usuario { get; set; }

        public int? IdUsuario { get; set; }
    }
}
