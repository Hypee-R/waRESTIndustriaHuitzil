using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class SolicitudesMateriale
    {
        public int IdSolicitud { get; set; }
        public DateTime? Fecha { get; set; }
        public double? Cantidad { get; set; }
        public int? IdMaterial { get; set; }
        public string? Comentarios { get; set; }
        public int? IdProveedor { get; set; }
        public string? Status { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public double? CostoTotal { get; set; }
        public int? IdUser { get; set; }

        public virtual Materiale? IdMaterialNavigation { get; set; }
        public virtual CatProveedore? IdProveedorNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
    }
}
