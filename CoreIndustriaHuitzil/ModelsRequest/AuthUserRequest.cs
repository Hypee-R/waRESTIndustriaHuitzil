using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class AuthUserRequest
    {
        public string usuario { get; set; } = string.Empty;
        public string contrasena { get; set; } = string.Empty;
    }
}
