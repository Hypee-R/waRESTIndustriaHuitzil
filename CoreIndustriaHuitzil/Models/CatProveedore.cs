using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class CatProveedore
    {
        public CatProveedore()
        {
            SolicitudesMateriales = new HashSet<SolicitudesMateriale>();
        }

        public int IdProveedor { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? EncargadoNombre { get; set; }

        public virtual ICollection<SolicitudesMateriale> SolicitudesMateriales { get; set; }
    }
}
