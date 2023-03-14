using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.Models
{
    public partial class PagoApartado
    {
        public int IdPagoApartado { get; set; }
        public int IdApartado { get; set; }
        public int? IdArticulo { get; set; }
        public int? IdCliente { get; set; }
        public int? Cantidad { get; set; }
        public string? Status { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
