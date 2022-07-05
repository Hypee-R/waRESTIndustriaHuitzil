using CoreIndustriaHuitzil.Models;
using CoreIndustriaHuitzil.ModelsRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceIndustriaHuitzil.Services
{
    public interface IIndustriaHuitzilService
    {
        #region Auth
        Task<object> auth(AuthUserRequest usuario);
        #endregion

        #region Roles
        Task<object> getRoles();
        Task<object> postRol(RolRequest rol);
        Task<object> putRol(RolRequest rol);
        Task<object> deleteRol(RolRequest rol);
        #endregion

        #region Vistas
        Task<object> getVistas();
        #endregion

        #region Ubicaciones
        Task<object> getUbicaciones();
        #endregion

        #region VistasRoles
        Task<object> getVistasRol(int idRol);
        Task<object> postVistaRol(VistaRolRequest vistaRol);
        Task<object> putVistaRol(VistaRolRequest vistaRol);
        Task<object> deleteVistaRol(VistaRolRequest vistasRol);
        #endregion
    }
}
