using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class CambiosDevolucione
    {
        public CambiosDevolucione()
        {
            CambiosDevolucionesArticulos = new HashSet<CambiosDevolucionesArticulo>();
        }

        public int IdCambioDevolucion { get; set; }
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int NoArticulos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Total { get; set; }

        public virtual Venta IdVentaNavigation { get; set; } = null!;
        public virtual ICollection<CambiosDevolucionesArticulo> CambiosDevolucionesArticulos { get; set; }
    }
}
