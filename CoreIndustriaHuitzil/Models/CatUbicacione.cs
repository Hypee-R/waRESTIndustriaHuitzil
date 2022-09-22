using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class CatUbicacione
    {
        public CatUbicacione()
        {
            Articulos = new HashSet<Articulo>();
            MaterialesUbicaciones = new HashSet<MaterialesUbicacione>();
        }

        public int IdUbicacion { get; set; }
        public string? Direccion { get; set; }
        public string? NombreEncargado { get; set; }
        public string? ApellidoPEncargado { get; set; }
        public string? ApellidoMEncargado { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? Correo { get; set; }

        public virtual ICollection<Articulo> Articulos { get; set; }
        public virtual ICollection<MaterialesUbicacione> MaterialesUbicaciones { get; set; }
    }
}
