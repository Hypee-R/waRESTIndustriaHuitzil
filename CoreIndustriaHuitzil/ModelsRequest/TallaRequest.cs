using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class TallaRequest
    {
        public int IdTalla { get; set; }
        public string Nombre { get; set; } = String.Empty;
        public string Descripcion { get; set; } = String.Empty;
    }
}
