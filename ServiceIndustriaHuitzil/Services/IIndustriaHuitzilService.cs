using CoreIndustriaHuitzil.Models;
using CoreIndustriaHuitzil.ModelsRequest;
using CoreIndustriaHuitzil.ModelsResponse;
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
        Task<ResponseModel> auth(AuthUserRequest usuario);
        #endregion

        #region Materiales
        Task<ResponseModel> getMateriales();
        Task<ResponseModel> postMaterial(MaterialRequest material);
        Task<ResponseModel> putMaterial(MaterialRequest material);
        Task<ResponseModel> deleteMaterial(MaterialRequest material);
        #endregion

        #region Proveedores
        Task<ResponseModel> getProveedores();
        Task<ResponseModel> postProveedor(ProveedorRequest proveedor);
        Task<ResponseModel> putProveedor(ProveedorRequest proveedor);
        Task<ResponseModel> deleteProveedor(ProveedorRequest proveedor);
        #endregion

        #region ProveedoresMateriales
        Task<ResponseModel> getProveedoresMateriales();
        Task<ResponseModel> postProveedorMaterial(ProveedoresMaterialesRequest proveedoresMateriales);
        Task<ResponseModel> deleteProveedorMaterial(ProveedoresMaterialesRequest proveedoresMateriales);
        #endregion

        #region Roles
        Task<ResponseModel> getRoles();
        Task<ResponseModel> postRol(RolRequest rol);
        Task<ResponseModel> putRol(RolRequest rol);
        Task<ResponseModel> deleteRol(RolRequest rol);
        #endregion

        #region Tallas
        Task<ResponseModel> getTallas();
        Task<ResponseModel> postTalla(TallaRequest talla);
        Task<ResponseModel> putTalla(TallaRequest talla);
        Task<ResponseModel> deleteTalla(TallaRequest talla);
        #endregion

        #region Ubicaciones
        Task<ResponseModel> getUbicaciones();
        Task<ResponseModel> postUbicacion(UbicacionRequest ubicacionRequest);
        Task<ResponseModel> putUbicacion(UbicacionRequest ubicacionRequest);
        Task<ResponseModel> deleteUbicacion(UbicacionRequest ubicacionRequest);

        #endregion

        #region Categorias
        Task<ResponseModel> getCategorias();
        Task<ResponseModel> postCategoria(CategoriaRequest categoriaRequest);
        Task<ResponseModel> putCategoria(CategoriaRequest categoriaRequest);
        Task<ResponseModel> deleteCategoria(CategoriaRequest categoriaRequest);

        #endregion

        #region Usuarios
        Task<ResponseModel> getUsuarios();
        Task<ResponseModel> postUsuario(UsuarioRequest usuario);
        Task<ResponseModel> putUsuario(UsuarioRequest usuario);
        Task<ResponseModel> deleteUsuario(UsuarioRequest usuario);
        #endregion

        #region Vistas
        Task<ResponseModel> getVistas();
        #endregion

        #region VistasRoles
        Task<ResponseModel> getVistasRol(int idRol);
        Task<ResponseModel> postVistaRol(VistaRolRequest vistaRol);
        Task<ResponseModel> putVistaRol(VistaRolRequest vistaRol);
        Task<ResponseModel> deleteVistaRol(VistaRolRequest vistasRol);
        #endregion
    }
}
