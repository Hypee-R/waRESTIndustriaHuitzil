using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Apartados
    {
        public int IdApartado { get; set; }
        public int? IdEmpleado { get; set; }
        public int? idArticulo { get; set; }
        public int? IdTalla { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? Status { get; set; }

        public virtual CatTalla? IdTallaNavigation { get; set; }
        public virtual Articulo? IdArticuloNavigation { get; set; }

    }
}
