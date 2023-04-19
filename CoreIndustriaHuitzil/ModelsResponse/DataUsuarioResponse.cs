using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsResponse
{
    public class DataUsuarioResponse
    {
        public int id { get; set; }
        public string usuario { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellidoPaterno { get; set; } = string.Empty;
        public string apellidoMaterno { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string ultimoAcceso { get; set; } = string.Empty;
        public DateTime expiredTime { get; set; }

        public string pc { get; set; } = string.Empty;
        public string ubicacion { get; set; } = string.Empty;
        public string impresora { get; set; } = string.Empty;

        public int idRol { get; set; }
        public string rol { get; set; } = string.Empty;
        public List<VistasResponse> vistas { get; set; }
    }
}
