using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Articulo
    {
        public Articulo()
        {
            CambiosDevolucionesArticulos = new HashSet<CambiosDevolucionesArticulo>();
            VentaArticulos = new HashSet<VentaArticulo>();
            Apartados = new HashSet<Apartados>();
        }

        public int IdArticulo { get; set; }
        public string? Status { get; set; }
        public string? Existencia { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int? IdUbicacion { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdTalla { get; set; }
        public string? Imagen { get; set; }
        public string? Sku { get; set; }
        public int? Precio { get; set; }

        public virtual CatCategoria? IdCategoriaNavigation { get; set; }
        public virtual CatTalla? IdTallaNavigation { get; set; }
        public virtual CatUbicacione? IdUbicacionNavigation { get; set; }
        public virtual ICollection<CambiosDevolucionesArticulo> CambiosDevolucionesArticulos { get; set; }
        public virtual ICollection<VentaArticulo> VentaArticulos { get; set; }

        public virtual ICollection<Apartados> Apartados { get; set; }
    }
}
