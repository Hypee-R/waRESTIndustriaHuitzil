using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class CatTalla
    {
        public CatTalla()
        {
            Articulos = new HashSet<Articulo>();
        }

        public int IdTalla { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Visible { get; set; }

        public virtual ICollection<Articulo> Articulos { get; set; }
    }
}
