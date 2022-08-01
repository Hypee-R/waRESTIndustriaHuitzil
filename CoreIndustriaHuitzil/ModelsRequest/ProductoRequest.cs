using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class ProductoRequest
    {
        public int IdArticulo { get; set; }
        public string Unidad { get; set; } = String.Empty;
        public string Existencia { get; set; } = String.Empty;
        public string Descripcion { get; set; } = String.Empty;
        public DateTime FechaIngreso { get; set; }
        public int idUbicacion { get; set; }
        public int idCategoria { get; set; }
        public int idTalla { get; set; }
        public string talla { get; set; } 
        public string categoria { get; set; }
        public string ubicacion { get; set; }
    }
}
