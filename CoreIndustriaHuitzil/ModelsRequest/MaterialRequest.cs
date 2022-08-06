using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class MaterialRequest
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; } = String.Empty;
        public string Descripcion { get; set; } = String.Empty;
        public double Precio { get; set; }
        public string TipoMedicion { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public double Stock { get; set; }
        public bool Visible { get; set; }
    }
}
