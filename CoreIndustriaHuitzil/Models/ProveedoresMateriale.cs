using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class ProveedoresMateriale
    {
        public ProveedoresMateriale()
        {
            SolicitudesMateriales = new HashSet<SolicitudesMateriale>();
        }

        public int IdProveedorMaterial { get; set; }
        public int IdProveedor { get; set; }
        public int IdMaterial { get; set; }

        public virtual Materiale IdProveedorMaterialNavigation { get; set; } = null!;
        public virtual CatProveedore IdProveedorNavigation { get; set; } = null!;
        public virtual ICollection<SolicitudesMateriale> SolicitudesMateriales { get; set; }
    }
}
