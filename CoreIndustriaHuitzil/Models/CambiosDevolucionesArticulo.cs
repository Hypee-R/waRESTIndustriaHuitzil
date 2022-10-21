using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class CambiosDevolucionesArticulo
    {
        public int IdCambioArticulo { get; set; }
        public int IdCambioDevolucion { get; set; }
        public int IdVentaArticulo { get; set; }
        public int? IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; } = null!;
        public string MotivoCambio { get; set; } = null!;
        public decimal PrecioAnterior { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal? Deducible { get; set; }

        public virtual Articulo? IdArticuloNavigation { get; set; }
        public virtual CambiosDevolucione IdCambioDevolucionNavigation { get; set; } = null!;
        public virtual VentaArticulo IdVentaArticuloNavigation { get; set; } = null!;
    }
}
