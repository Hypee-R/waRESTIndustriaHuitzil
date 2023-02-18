using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class ClienteRequest
    {
        public int IdCliente { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public string? ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; } = string.Empty;
        public string? Telefono1 { get; set; } = string.Empty;
        public string? Telefono2 { get; set; } = string.Empty;
        public string? Direccion { get; set; } = string.Empty;
        public bool? Visible { get; set; } 
    }
}
