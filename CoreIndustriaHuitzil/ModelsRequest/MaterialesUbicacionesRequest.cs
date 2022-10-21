using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class MaterialesUbicacionesRequest
    {
        public int IdMaterialUbicacion { get; set; }
        public int IdMaterial { get; set; }
        public int IdUbicacion { get; set; }
        public MaterialRequest material { get; set; }
        public UbicacionRequest ubicacion { get; set; }
    }
}
