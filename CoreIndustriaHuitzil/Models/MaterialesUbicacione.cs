using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.Models
{
    public class MaterialesUbicacione
    {
        public int IdMaterialUbicacion { get; set; }
        public int IdMaterial { get; set; }
        public int IdUbicacion { get; set; }

        public virtual Materiale IdMaterialNavigation { get; set; } = null!;
        public virtual CatUbicacione IdUbicacionNavigation { get; set; } = null!;
    }
}
