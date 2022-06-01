using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class Materiale
    {
        public Materiale()
        {
            SolicitudesMateriales = new HashSet<SolicitudesMateriale>();
        }

        public int IdMaterial { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public double? Precio { get; set; }
        public string? TipoMedicion { get; set; }
        public string? Status { get; set; }
        public int? IdProveedor { get; set; }
        public double? Cantidad { get; set; }

        public virtual ICollection<SolicitudesMateriale> SolicitudesMateriales { get; set; }
    }
}
