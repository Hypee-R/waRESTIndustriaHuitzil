using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class CorreoRequest
    {
        public string correo { get; set; } = String.Empty;
        public string mensaje { get; set; } = String.Empty;
        public string subject { get; set; } = String.Empty;
        public string titulo { get; set; } = String.Empty;

    }
}
