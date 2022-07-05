using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Vista
    {
        public Vista()
        {
            VistasRols = new HashSet<VistasRol>();
        }

        public int IdVista { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int Posicion { get; set; }
        public string RouterLink { get; set; } = null!;
        public bool? Visible { get; set; }
        public string Icon { get; set; } = null!;

        public virtual ICollection<VistasRol> VistasRols { get; set; }
    }
}
