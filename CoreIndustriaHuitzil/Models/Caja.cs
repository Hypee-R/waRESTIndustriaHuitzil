using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Caja
    {
        public Caja()
        {
            Venta = new HashSet<Venta>();
        }

        public int IdCaja { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal Monto { get; set; }
        public decimal? MontoCierre { get; set; }

        public virtual User IdEmpleadoNavigation { get; set; } = null!;
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
