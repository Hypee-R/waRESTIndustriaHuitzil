using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Users = new HashSet<User>();
            VistasRols = new HashSet<VistasRol>();
        }

        public int IdRol { get; set; }
        public string? Descripcion { get; set; }
        public bool? Visible { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<VistasRol> VistasRols { get; set; }
    }
}
