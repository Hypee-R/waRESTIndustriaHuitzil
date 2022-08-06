using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class SolicitudesMateriale
    {
        public int IdSolicitud { get; set; }
        public DateTime? Fecha { get; set; }
        public double? Cantidad { get; set; }
        public string? Comentarios { get; set; }
        public int? IdProveedorMaterial { get; set; }
        public string? Status { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public double? CostoTotal { get; set; }
        public int? IdUser { get; set; }

        public virtual ProveedoresMateriale? IdProveedorMaterialNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
    }
}
