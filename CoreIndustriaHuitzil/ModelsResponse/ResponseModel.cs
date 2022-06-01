using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsResponse
{
    public class ResponseModel
    {
        public bool exito { get; set; }
        public string mensaje { get; set; } = string.Empty;
        public object respuesta { get; set; }
    }
}
