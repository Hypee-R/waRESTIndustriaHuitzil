using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class UsuarioRequest
    {
        public int IdUser { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string pc { get; set; }
    }

    public class UpdatePswRequest
    {
        public int IdUser { get; set; }
        public string NewPassword { get; set; } = String.Empty;
    }
}
