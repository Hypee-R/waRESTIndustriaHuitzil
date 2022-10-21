using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class MaterialesUbicacione
    {
        public int IdMaterialUbicacion { get; set; }
        public int IdMaterial { get; set; }
        public int IdUbicacion { get; set; }

        public virtual Materiale IdMaterialNavigation { get; set; } = null!;
        public virtual CatUbicacione IdUbicacionNavigation { get; set; } = null!;
    }
}
