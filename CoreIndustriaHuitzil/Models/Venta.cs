using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Venta
    {
        public Venta()
        {
            CambiosDevoluciones = new HashSet<CambiosDevolucione>();
            VentaArticulos = new HashSet<VentaArticulo>();
        }

        public int IdVenta { get; set; }
        public int IdCaja { get; set; }
        public DateTime Fecha { get; set; }
        public string NoTicket { get; set; } = null!;
        public string TipoPago { get; set; } = null!;
        public string TipoVenta { get; set; } = null!;
        public int NoArticulos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Total { get; set; }

        public virtual Caja IdCajaNavigation { get; set; } = null!;
        public virtual ICollection<CambiosDevolucione> CambiosDevoluciones { get; set; }
        public virtual ICollection<VentaArticulo> VentaArticulos { get; set; }
    }
}
