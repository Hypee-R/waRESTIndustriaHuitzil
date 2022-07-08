using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class ProveedorRequest
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = String.Empty;
        public string ApellidoPaterno { get; set; } = String.Empty;
        public string ApellidoMaterno { get; set; } = String.Empty;
        public string Telefono1 { get; set; } = String.Empty;
        public string Telefono2 { get; set; } = String.Empty;
        public string Correo { get; set; } = String.Empty;
        public string Direccion { get; set; } = String.Empty;
        public string EncargadoNombre { get; set; } = String.Empty;
    }
}
