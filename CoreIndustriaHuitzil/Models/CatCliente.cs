using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class CatCliente
    {
        public CatCliente() {
            Apartados = new HashSet<Apartados>();
        }
        public int IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? Direccion { get; set; }
        public bool? Visible { get; set; }
        public virtual ICollection<Apartados> Apartados { get; set; }
    }
}
