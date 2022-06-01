using System;
using System.Collections.Generic;

namespace CoreIndustriaHuitzil.Models
{
    public partial class User
    {
        public User()
        {
            SolicitudesMateriales = new HashSet<SolicitudesMateriale>();
        }

        public int IdUser { get; set; }
        public string? Usuario { get; set; }
        public string? Password { get; set; }
        public int? IdRol { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Token { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
        public virtual ICollection<SolicitudesMateriale> SolicitudesMateriales { get; set; }
    }
}
