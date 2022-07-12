using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Materiale
    {
        public int IdMaterial { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public double? Precio { get; set; }
        public string? TipoMedicion { get; set; }
        public string? Status { get; set; }
        public double? Stock { get; set; }
        public bool? Visible { get; set; }

        public virtual ProveedoresMateriale ProveedoresMateriale { get; set; } = null!;
    }
}
