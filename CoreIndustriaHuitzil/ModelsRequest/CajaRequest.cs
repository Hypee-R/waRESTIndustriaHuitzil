using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class CajaRequest
    {
        public int IdCaja { get; set; }
        public int IdEmpleado { get; set; }
        public string Fecha { get; set; }
        public string? FechaCierre { get; set; }
        public decimal Monto { get; set; }
        public decimal? MontoCierre { get; set; }
    }
}
