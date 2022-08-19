using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreIndustriaHuitzil.ModelsRequest
{
    public class SolicitudesMaterialesRequest
    {
        public int IdSolicitud { get; set; }
        public DateTime? Fecha { get; set; }
        public double? Cantidad { get; set; }
        public string? Comentarios { get; set; }
        public int? IdProveedorMaterial { get; set; }
        public ProveedoresMaterialesRequest proveedorMaterial { get; set; }
        public string? Status { get; set; }
        public DateTime? FechaUpdate { get; set; }
        public double? CostoTotal { get; set; }
        public int? IdUser { get; set; }
        public UsuarioRequest usuario { get; set; }
    }
}
