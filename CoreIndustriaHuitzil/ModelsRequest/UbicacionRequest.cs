using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class UbicacionRequest
    {

        public int IdUbicacion { get; set; }
        public string? Direccion { get; set; }
        public string? NombreEncargado { get; set; }
        public string? ApellidoPEncargado { get; set; }
        public string? ApellidoMEncargado { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? Correo { get; set; }


    }
}
