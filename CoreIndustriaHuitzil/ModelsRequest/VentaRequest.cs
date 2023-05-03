using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class VentaRequest
    {
        public int IdVenta { get; set; }
        public int IdCaja { get; set; }
        public string Fecha { get; set; }
        public string NoTicket { get; set; } = null!;
        public string TipoPago { get; set; } = null!;
        public string TipoVenta { get; set; } = null!;
        public int NoArticulos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Total { get; set; }

        public decimal? Tarjeta { get; set; }
        public decimal? Efectivo { get; set; }

        public virtual CajaRequest? Caja { get; set; } = null!;

        public List<VentaArticuloRequest> ventaArticulo { get; set; } = new List<VentaArticuloRequest>();
        
    }
    public class VentaArticuloRequest
    {
        public int IdVentaArticulo { get; set; }
        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual ProductoRequest? Articulo { get; set; } = null!;
        public virtual VentaRequest? Venta { get; set; } = null!;
    }
}
