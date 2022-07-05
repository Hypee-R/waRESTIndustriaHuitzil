using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class VistasRol
    {
        public int IdVistaRol { get; set; }
        public int IdVista { get; set; }
        public int IdRol { get; set; }

        public virtual Rol IdRolNavigation { get; set; } = null!;
        public virtual Vista IdVistaNavigation { get; set; } = null!;
    }
}
