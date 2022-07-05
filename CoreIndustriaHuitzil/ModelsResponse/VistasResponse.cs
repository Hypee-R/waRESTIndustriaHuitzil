using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsResponse
{
    public class VistasResponse
    {
        public int IdVista { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int Posicion { get; set; }
        public string RouterLink { get; set; } = null!;
        public bool Visible { get; set; }
    }
}
