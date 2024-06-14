using System;
using System.Collections.Generic;

namespace ApiPasteles.Models;

public partial class Pastel
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Origen { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();
}
