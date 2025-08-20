using System;
using System.Collections.Generic;

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

    public bool Solicitante { get; set; }

    public bool Beneficiario { get; set; }

    public bool Funcionario { get; set; }

    public virtual ICollection<Planilla> PlanillaBeneficiarioNavigations { get; set; } = new List<Planilla>();

    public virtual ICollection<Planilla> PlanillaFuncionarioNavigations { get; set; } = new List<Planilla>();

    public virtual ICollection<Planilla> PlanillaSolicitanteNavigations { get; set; } = new List<Planilla>();

    public virtual Usuario? Usuario { get; set; }
}
