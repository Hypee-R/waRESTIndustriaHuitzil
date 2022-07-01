using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsResponse
{
    public class VistasRolResponse
    {
        public int idVistaRol { get; set; }
        public int idRol { get; set; }
        public string rol { get; set; } = String.Empty;
        public int idVista { get; set; }
        public string vista { get; set; } = String.Empty;
    }
}
