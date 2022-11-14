using CoreIndustriaHuitzil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class CambiosDevolucionesRequest
    {
        public int IdCambioDevolucion { get; set; }
        public int IdVenta { get; set; }
        public string Fecha { get; set; } = String.Empty;
        public int NoArticulos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Total { get; set; }

        public VentaRequest? Venta { get; set; } = null!;
        public List<CambiosDevolucionesArticuloRequest>? CambiosDevolucionesArticulos { get; set; } = null!;
    }

    public class CambiosDevolucionesArticuloRequest
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

        public virtual ProductoRequest? Articulo { get; set; } = null!;
        public virtual VentaArticuloRequest? VentaArticulo {
            
            get; set; } = null!;
    }




}
