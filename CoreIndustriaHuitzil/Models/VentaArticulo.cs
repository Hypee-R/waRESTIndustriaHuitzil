using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class VentaArticulo
    {
        public VentaArticulo()
        {
            CambiosDevolucionesArticulos = new HashSet<CambiosDevolucionesArticulo>();
        }

        public int IdVentaArticulo { get; set; }
        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual Articulo IdArticuloNavigation { get; set; } = null!;
        public virtual Venta IdVentaNavigation { get; set; } = null!;
        public virtual ICollection<CambiosDevolucionesArticulo> CambiosDevolucionesArticulos { get; set; }
    }
}
