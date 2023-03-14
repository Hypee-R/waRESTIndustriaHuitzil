using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    
        public partial class ApartadosRequest
        {
            public int IdApartado { get; set; }
            public int? IdEmpleado { get; set; }
            public int? idArticulo { get; set; }
            public int? IdTalla { get; set; }
            public string? Telefono { get; set; }
            public string? Direccion { get; set; }
            public DateTime? Fecha { get; set; }
            public DateTime? FechaEntrega { get; set; }
            public string? Status { get; set; }
            public string? talla { get; set; }
            public string? articulo { get; set; }
            public int? precio { get; set; }    
    }
    

}
