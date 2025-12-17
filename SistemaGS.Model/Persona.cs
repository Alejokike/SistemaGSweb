namespace SistemaGS.Model;

public partial class Persona
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

    public virtual ICollection<Ayuda> AyudumFuncionarioNavigations { get; set; } = new List<Ayuda>();

    public virtual ICollection<Ayuda> AyudumSolicitanteNavigations { get; set; } = new List<Ayuda>();
}
