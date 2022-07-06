﻿using CoreIndustriaHuitzil.Models;
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

        #region Ubicaciones
        Task<object> getUbicaciones();
        Task<object> postUbicacion(UbicacionRequest ubicacionRequest);
        Task<object> putUbicacion(UbicacionRequest ubicacionRequest);
        Task<object> deleteUbicacion(UbicacionRequest ubicacionRequest);

        #endregion

        #region Usuarios
        Task<object> getUsuarios();
        Task<object> postUsuario(UsuarioRequest usuario);
        Task<object> putUsuario(UsuarioRequest usuario);
        Task<object> deleteUsuario(UsuarioRequest usuario);
        #endregion

        #region Vistas
        Task<object> getVistas();
        #endregion

        #region VistasRoles
        Task<object> getVistasRol(int idRol);
        Task<object> postVistaRol(VistaRolRequest vistaRol);
        Task<object> putVistaRol(VistaRolRequest vistaRol);
        Task<object> deleteVistaRol(VistaRolRequest vistasRol);
        #endregion
    }
}
