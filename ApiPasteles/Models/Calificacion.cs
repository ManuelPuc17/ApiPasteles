using System;
using System.Collections.Generic;

namespace ApiPasteles.Models;

public partial class Calificacion
{
    public int Id { get; set; }

    public int? Usuario { get; set; }

    public int? Pastel { get; set; }

    public double? Sabor { get; set; }

    public double? Presentacion { get; set; }

    public virtual Pastel? PastelNavigation { get; set; }

    public virtual Usuario? UsuarioNavigation { get; set; }
}
