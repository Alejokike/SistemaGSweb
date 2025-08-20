using System;
using System.Collections.Generic;

namespace SistemaGS.Model;

public partial class ListaItem
{
    public int IdLista { get; set; }

    public int IdPlanilla { get; set; }

    public int IdItem { get; set; }

    public string? DetalleJson { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public virtual Item IdItemNavigation { get; set; } = null!;

    public virtual Planilla IdPlanillaNavigation { get; set; } = null!;
}
