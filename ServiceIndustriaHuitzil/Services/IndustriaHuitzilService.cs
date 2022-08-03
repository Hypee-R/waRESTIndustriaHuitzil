using AccessControl.JwtHelpers;
using AccessControl.Models;
using CoreIndustriaHuitzil.Models;
using CoreIndustriaHuitzil.ModelsRequest;
using CoreIndustriaHuitzil.ModelsResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ServiceIndustriaHuitzil.Services
{
    public class IndustriaHuitzilService : IIndustriaHuitzilService
    {
        private readonly IndustriaHuitzilDbContext _ctx;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public IndustriaHuitzilService(
            IndustriaHuitzilDbContext ctx,
            IConfiguration configuration,
            JwtSettings jwtSettings
            )
        {
            _ctx = ctx;
            //_connectionString = "Server=localHost\\Prueba1;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=Ventana0512";
            _connectionString = "Server=DESARROLLOXR\\SA;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=Ventana0512";
            _configuration = configuration;
            _jwtSettings = jwtSettings;
        }

        #region Auth
        public async Task<ResponseModel> auth(AuthUserRequest authRequest)
        {
            ResponseModel respuesta = new ResponseModel();
            DataUsuarioResponse dataLogin = new DataUsuarioResponse();
            respuesta.exito = false;
            respuesta.mensaje = "Credenciales incorrectas";
            respuesta.respuesta = "[]";
            try 
            {
                User existeUsuario = _ctx.Users.Where(x => x.Usuario == authRequest.usuario && x.Password == authRequest.contrasena)
                                                .Include(u => u.IdRolNavigation)
                                                .IgnoreAutoIncludes()
                                                .FirstOrDefault();

               if (existeUsuario != null)
               {
                    List<VistasResponse> vistasU = _ctx.VistasRols.Include(a => a.IdVistaNavigation).Where(x => x.IdRol == existeUsuario.IdRol && x.IdVistaNavigation.Visible == true)
                                                            .OrderBy(y => y.IdVistaNavigation.Posicion).ToList().ConvertAll<VistasResponse>(v => new VistasResponse
                                                            {
                                                                IdVista = v.IdVistaNavigation.IdVista,
                                                                Nombre = v.IdVistaNavigation.Nombre,
                                                                Descripcion = v.IdVistaNavigation.Descripcion,
                                                                Posicion = v.IdVistaNavigation.Posicion,
                                                                RouterLink = v.IdVistaNavigation.RouterLink,
                                                                Visible = (bool)v.IdVistaNavigation.Visible,
                                                                Icon = v.IdVistaNavigation.Icon
                                                            });
                    var dataAccess = generarToken(existeUsuario);
                    existeUsuario.Token = dataAccess.Token;
                    existeUsuario.UltimoAcceso = DateTime.Now;
                    _ctx.Users.Update(existeUsuario);
                    _ctx.SaveChanges();

                    dataLogin.id = existeUsuario.IdUser;
                    dataLogin.nombre = existeUsuario.Nombre;
                    dataLogin.usuario = existeUsuario.Usuario;
                    dataLogin.password = existeUsuario.Password;
                    dataLogin.apellidoPaterno = existeUsuario.ApellidoPaterno;
                    dataLogin.apellidoMaterno = existeUsuario.ApellidoMaterno;
                    dataLogin.correo = existeUsuario.Correo;
                    dataLogin.telefono = existeUsuario.Telefono;
                    dataLogin.token = existeUsuario.Token;
                    dataLogin.ultimoAcceso = existeUsuario.UltimoAcceso.ToString();
                    dataLogin.idRol = existeUsuario.IdRolNavigation.IdRol;
                    dataLogin.rol = existeUsuario.IdRolNavigation.Descripcion;
                    dataLogin.vistas = vistasU;

                    respuesta.exito = true;
                    respuesta.mensaje = "Credenciales correctas!!";
                    respuesta.respuesta = dataLogin;
                }

               return respuesta;
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex);
                return respuesta;
           }
        
        }
        #endregion

        #region Materiales
        public async Task<ResponseModel> getMateriales()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay materiales para mostrar";
                response.respuesta = "[]";

                List<Materiale> lista = await _ctx.Materiales.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los materiales!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postMaterial(MaterialRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo material";
                response.respuesta = "[]";

                Materiale newMaterial = new Materiale();
                
                newMaterial.Nombre = request.Nombre;
                newMaterial.Descripcion = request.Descripcion;
                newMaterial.Precio = request.Precio;
                newMaterial.TipoMedicion = request.TipoMedicion;
                newMaterial.Status = request.Status;
                newMaterial.Stock = request.Stock;

                _ctx.Materiales.Add(newMaterial);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el material correctamente!!";
                response.respuesta = newMaterial;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putMaterial(MaterialRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar el material";
                response.respuesta = "[]";

                Materiale existeMaterial = _ctx.Materiales.FirstOrDefault(x => x.IdMaterial == request.IdMaterial);
                if (existeMaterial != null)
                {
                    existeMaterial.Nombre = request.Nombre;
                    existeMaterial.Descripcion = request.Descripcion;
                    existeMaterial.Precio = request.Precio;
                    existeMaterial.TipoMedicion = request.TipoMedicion;
                    existeMaterial.Status = request.Status;
                    existeMaterial.Stock = request.Stock;

                    _ctx.Materiales.Update(existeMaterial);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el material correctamente!!";
                    response.respuesta = existeMaterial;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteMaterial(MaterialRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el material";
                response.respuesta = "[]";

                Materiale existeMaterial = _ctx.Materiales.FirstOrDefault(x => x.IdMaterial == request.IdMaterial);

                if (existeMaterial != null)
                {
                    existeMaterial.Visible = false;
                    _ctx.Materiales.Update(existeMaterial);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el material correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Proveedores
        public async Task<ResponseModel> getProveedores()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay proveedores para mostrar";
                response.respuesta = "[]";

                List<CatProveedore> lista = await _ctx.CatProveedores.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los proveedores!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postProveedor(ProveedorRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo proveedor";
                response.respuesta = "[]";

                CatProveedore newProveedor = new CatProveedore();
                newProveedor.Nombre = request.Nombre;
                newProveedor.ApellidoPaterno = request.ApellidoPaterno;
                newProveedor.ApellidoMaterno = request.ApellidoMaterno;
                newProveedor.Telefono1 = request.Telefono1;
                newProveedor.Telefono2 = request.Telefono2;
                newProveedor.Correo = request.Correo;
                newProveedor.Direccion = request.Direccion;
                newProveedor.EncargadoNombre = request.EncargadoNombre;

                _ctx.CatProveedores.Add(newProveedor);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el proveedor correctamente!!";
                response.respuesta = newProveedor;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putProveedor(ProveedorRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar el proveedor";
                response.respuesta = "[]";

                CatProveedore existeProveedor = _ctx.CatProveedores.FirstOrDefault(x => x.IdProveedor == request.IdProveedor);

                if (existeProveedor != null)
                {
                    existeProveedor.Nombre = request.Nombre;
                    existeProveedor.ApellidoPaterno = request.ApellidoPaterno;
                    existeProveedor.ApellidoMaterno = request.ApellidoMaterno;
                    existeProveedor.Telefono1 = request.Telefono1;
                    existeProveedor.Telefono2 = request.Telefono2;
                    existeProveedor.Correo = request.Correo;
                    existeProveedor.Direccion = request.Direccion;
                    existeProveedor.EncargadoNombre = request.EncargadoNombre;

                    _ctx.CatProveedores.Update(existeProveedor);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el proveedor correctamente!!";
                    response.respuesta = existeProveedor;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteProveedor(ProveedorRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el proveedor";
                response.respuesta = "[]";

                CatProveedore existeProveedor = _ctx.CatProveedores.FirstOrDefault(x => x.IdProveedor == request.IdProveedor);

                if (existeProveedor != null)
                {
                    existeProveedor.Visible = false;
                    _ctx.CatProveedores.Update(existeProveedor);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el proveedor correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Productos
        public async Task<ResponseModel> getProductos()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay arituculos para mostrar";
                response.respuesta = "[]";

                List<ProductoRequest> lista = _ctx.Articulos.Include(a => a.IdTallaNavigation)
                                                .Include(b=>b.IdCategoriaNavigation)
                                                .Include(c=>c.IdUbicacionNavigation)
                                                .Where(x => x.IdArticulo!=null).ToList()
                                                   .ConvertAll(u => new ProductoRequest()
                                                   {
                                                       IdArticulo=u.IdArticulo,
                                                       Unidad=u.Unidad,
                                                       Existencia=u.Existencia,
                                                       Descripcion=u.Descripcion,
                                                       FechaIngreso=u.FechaIngreso,
                                                       idTalla=(int)u.IdTalla,
                                                       idCategoria=(int)u.IdCategoria,
                                                       idUbicacion=(int)u.IdUbicacion,
                                                       imagen=u.imagen,
                                                       talla = u.IdTallaNavigation.Nombre,
                                                       ubicacion= u.IdUbicacionNavigation.Direccion,
                                                       categoria = u.IdCategoriaNavigation.Descripcion
                                   
                                                       
                                                                                                       
                                                   });
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los proveedores!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postProductos(ProductoRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo proveedor";
                response.respuesta = "[]";

                Articulo newArticulo = new Articulo();
                CatProveedore newProveedor = new CatProveedore();
            
                newArticulo.Unidad = request.Unidad;
                newArticulo.Descripcion = request.Descripcion;
                newArticulo.FechaIngreso = request.FechaIngreso;
                newArticulo.Existencia = request.Existencia;
                newArticulo.IdUbicacion = request.idUbicacion;
                newArticulo.IdCategoria = request.idCategoria;
                newArticulo.IdTalla = request.idTalla;
                newArticulo.imagen = request.imagen;

                _ctx.Articulos.Add(newArticulo);
                await _ctx.SaveChangesAsync();
                
                response.exito = true;
                response.mensaje = "Se agrego el articulo correctamente!!";
                response.respuesta = newArticulo;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putProductos(ProductoRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar el articulo";
                response.respuesta = "[]";
                Articulo existeArticulo = _ctx.Articulos.FirstOrDefault(x => x.IdArticulo == request.IdArticulo);
                if (existeArticulo != null)
                {



                    existeArticulo.Unidad = request.Unidad;
                    existeArticulo.Descripcion = request.Descripcion;
                    existeArticulo.FechaIngreso = request.FechaIngreso;
                    existeArticulo.Existencia = request.Existencia;
                    existeArticulo.IdUbicacion =request.idUbicacion;
                    existeArticulo.IdCategoria = request.idCategoria;
                    existeArticulo.IdTalla = request.idTalla;
                   
                    _ctx.Articulos.Update(existeArticulo);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el articulo correctamente!!";
                    response.respuesta = existeArticulo;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteProductos(ProductoRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el proveedor";
                response.respuesta = "[]";

               //CatProveedore existeProveedor = _ctx.CatProveedores.FirstOrDefault(x => x.IdProveedor == request.IdProveedor);
                Articulo existeArticulo = _ctx.Articulos.FirstOrDefault(x => x.IdArticulo == request.IdArticulo);
               
                if (existeArticulo != null)
                {
                    //existeArticulo.Visible = false;
                    //_ctx.CatProveedores.Update(existeProveedor);
                    _ctx.Articulos.Remove(existeArticulo);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el articulo correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region ProveedoresMateriales
        public async Task<ResponseModel> getProveedoresMateriales()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay proveedores de materiales para mostrar";
                response.respuesta = "[]";

                List<ProveedoresMateriale> lista = await _ctx.ProveedoresMateriales.Include(x => x.IdProveedorMaterialNavigation).Include(y => y.IdProveedorNavigation)
                                                                .Where(z => z.IdProveedorMaterialNavigation.Visible == true && z.IdProveedorNavigation.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los proveedores y materiales!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postProveedorMaterial(ProveedoresMaterialesRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo proveedor material";
                response.respuesta = "[]";

                ProveedoresMateriale newProvMaterial = new ProveedoresMateriale();
                
                newProvMaterial.IdMaterial = request.IdMaterial;
                newProvMaterial.IdProveedor = request.IdProveedor;

                _ctx.ProveedoresMateriales.Add(newProvMaterial);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el proveedor material correctamente!!";
                response.respuesta = newProvMaterial;
                

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteProveedorMaterial(ProveedoresMaterialesRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el proveedor material";
                response.respuesta = "[]";

                ProveedoresMateriale existeProvMaterial = _ctx.ProveedoresMateriales.FirstOrDefault(x => x.IdProveedorMaterial == request.IdProveedorMaterial);
                if (existeProvMaterial != null)
                {
                    _ctx.ProveedoresMateriales.Remove(existeProvMaterial);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el proveedor material correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Roles
        public async Task<ResponseModel> getRoles()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay roles para mostrar";
                response.respuesta = "[]";
                List<Rol> lista = await _ctx.Rols.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los roles!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        public async Task<ResponseModel> postRol(RolRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo rol";
                response.respuesta = "[]";

                Rol newRol = new Rol();
                newRol.Descripcion = request.Descripcion;
                
                _ctx.Rols.Add(newRol);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el rol correctamente!!";
                response.respuesta = newRol;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        public async Task<ResponseModel> putRol(RolRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar el rol";
                response.respuesta = "[]";

                Rol existeRol = _ctx.Rols.FirstOrDefault(x => x.IdRol == request.IdRol);

                if (existeRol != null)
                {
                    existeRol.Descripcion = request.Descripcion;
                    _ctx.Rols.Update(existeRol);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el rol correctamente!!";
                    response.respuesta = existeRol;
                }

                return response;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        public async Task<ResponseModel> deleteRol(RolRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el rol";
                response.respuesta = "[]";

                Rol existeRol = _ctx.Rols.FirstOrDefault(x => x.IdRol == request.IdRol);

                if (existeRol != null)
                {
                    existeRol.Visible = false;
                    _ctx.Rols.Update(existeRol);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el rol correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Tallas
        public async Task<ResponseModel> getTallas()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "";
                response.respuesta = "[]";

                List<CatTalla> lista = await _ctx.CatTallas.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las tallas!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postTalla(TallaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar la nueva talla";
                response.respuesta = "[]";

                CatTalla newTalla = new CatTalla();
                newTalla.Nombre = request.Nombre;
                newTalla.Descripcion = request.Descripcion;

                _ctx.CatTallas.Add(newTalla);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó la talla correctamente!!";
                response.respuesta = newTalla;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putTalla(TallaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar la talla";
                response.respuesta = "[]";

                CatTalla existeTalla = _ctx.CatTallas.FirstOrDefault(x => x.IdTalla == request.IdTalla);

                if (existeTalla != null)
                {
                    existeTalla.Nombre = request.Nombre;
                    existeTalla.Descripcion = request.Descripcion;

                    _ctx.CatTallas.Update(existeTalla);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó la talla correctamente!!";
                    response.respuesta = existeTalla;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteTalla(TallaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar la talla";
                response.respuesta = "[]";

                CatTalla existeTalla = _ctx.CatTallas.FirstOrDefault(x => x.IdTalla == request.IdTalla);

                if (existeTalla != null)
                {
                    existeTalla.Visible = false;
                    _ctx.CatTallas.Update(existeTalla);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó la talla correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Ubicaciones
        public async Task<ResponseModel> getUbicaciones()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay Ubicaciones para mostrar";
                response.respuesta = "[]";
                List<CatUbicacione> lista =  _ctx.CatUbicaciones.ToList();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las ubicaciones!!";
                    response.respuesta = lista;
                }

                return response;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postUbicacion(UbicacionRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo Crear la Ubicacion";
                response.respuesta = "[]";

                CatUbicacione newCatUbicacione = new CatUbicacione();
                newCatUbicacione.Direccion = request.Direccion;
                newCatUbicacione.NombreEncargado = request.NombreEncargado;
                newCatUbicacione.ApellidoPEncargado = request.ApellidoPEncargado;
                newCatUbicacione.ApellidoMEncargado = request.ApellidoMEncargado;
                newCatUbicacione.Telefono1 = request.Telefono1;
                newCatUbicacione.Telefono2 = request.Telefono2;
                newCatUbicacione.Correo = request.Correo;


                _ctx.CatUbicaciones.Add(newCatUbicacione);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se Creo Correctamente la Ubicacion!!";
                response.respuesta = newCatUbicacione;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putUbicacion(UbicacionRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar la Ubicacion";
                response.respuesta = "[]";

                CatUbicacione existeUbicacion = _ctx.CatUbicaciones.FirstOrDefault(x => x.IdUbicacion == request.IdUbicacion);

                if (existeUbicacion != null)
                {
                   
                    existeUbicacion.Direccion = request.Direccion;
                    existeUbicacion.NombreEncargado = request.NombreEncargado;
                    existeUbicacion.ApellidoPEncargado = request.ApellidoPEncargado;
                    existeUbicacion.ApellidoMEncargado = request.ApellidoMEncargado;
                    existeUbicacion.Telefono1 = request.Telefono1;
                    existeUbicacion.Telefono2 = request.Telefono2;
                    existeUbicacion.Correo = request.Correo;

                    _ctx.Update(existeUbicacion);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó correctamente la Ubicacion!!";
                    response.respuesta = existeUbicacion;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteUbicacion(UbicacionRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar la Ubicacion";
                response.respuesta = "[]";

                CatUbicacione existeUbicacion = _ctx.CatUbicaciones.FirstOrDefault(x => x.IdUbicacion == request.IdUbicacion);

                if (existeUbicacion != null)
                {

                    _ctx.Remove(existeUbicacion);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó correctamente la vista correspondiente al rol!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Categorias
        public async Task<ResponseModel> getCategorias()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay Categorias para mostrar";
                response.respuesta = "[]";
                List<CatCategoria> lista = _ctx.CatCategorias.ToList();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las Categorias!!";
                    response.respuesta = lista;
                }

                return response;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postCategoria(CategoriaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo Crear la Categoria";
                response.respuesta = "[]";

                CatCategoria newCatCategoria = new CatCategoria();
                newCatCategoria.IdCategoria = request.IdCategoria;
                newCatCategoria.Nombre = request.Nombre;
                newCatCategoria.Descripcion = request.Descripcion;
             


                _ctx.CatCategorias.Add(newCatCategoria);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se Creo Correctamente la Categoria!!";
                response.respuesta = newCatCategoria;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putCategoria(CategoriaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar la Categoria";
                response.respuesta = "[]";

                CatCategoria existeCategoria = _ctx.CatCategorias.FirstOrDefault(x => x.IdCategoria == request.IdCategoria);

                if (existeCategoria != null)
                {
                    existeCategoria.IdCategoria = request.IdCategoria;
                    existeCategoria.Nombre = request.Nombre;
                    existeCategoria.Descripcion = request.Descripcion;
                 
                   

                    _ctx.Update(existeCategoria);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó correctamente la Categoria!!";
                    response.respuesta = existeCategoria;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteCategoria(CategoriaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar la Categoria";
                response.respuesta = "[]";

                CatCategoria existeCategoria= _ctx.CatCategorias.FirstOrDefault(x => x.IdCategoria == request.IdCategoria);

                if (existeCategoria != null)
                {

                    _ctx.Remove(existeCategoria);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó correctamente la Categoria!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Usuarios
        public async Task<ResponseModel> getUsuarios()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay usuarios para mostrar";
                response.respuesta = "[]";

                List<UsuarioRequest> lista = _ctx.Users.Include(a => a.IdRolNavigation).Where(x => x.Visible == true).ToList()
                                                 .ConvertAll(u => new UsuarioRequest()
                                                 {
                                                     IdUser = u.IdUser,
                                                     Usuario = u.Usuario,
                                                     Password = Encrypt.GetSHA1(u.Password),
                                                     IdRol = (int)u.IdRol,
                                                     Rol = u.IdRolNavigation.Descripcion,
                                                     Nombre = u.Nombre,
                                                     ApellidoPaterno = u.ApellidoPaterno,
                                                     ApellidoMaterno = u.ApellidoMaterno,
                                                     Telefono = u.Telefono,
                                                     Correo = u.Correo
                                                 });

                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los usuarios!!";
                    response.respuesta = lista;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> postUsuario(UsuarioRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el usuario";
                response.respuesta = "[]";

                User newUser = new User();
                newUser.Usuario = request.Usuario;
                newUser.Password = request.Password;
                newUser.Nombre = request.Nombre;
                newUser.ApellidoPaterno = request.ApellidoPaterno;
                newUser.ApellidoMaterno = request.ApellidoMaterno; 
                newUser.Telefono = request.Telefono;
                newUser.Correo = request.Correo;
                newUser.IdRol = request.IdRol;

                _ctx.Users.Add(newUser);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el usuario correctamente!!";
                response.respuesta = newUser;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> putUsuario(UsuarioRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar el usuario";
                response.respuesta = "[]";

                User existeUser = _ctx.Users.FirstOrDefault(x => x.IdUser == request.IdUser);

                if (existeUser != null)
                {
                    existeUser.Usuario = request.Usuario;
                    existeUser.Nombre = request.Nombre;
                    existeUser.ApellidoPaterno = request.ApellidoPaterno;
                    existeUser.ApellidoMaterno = request.ApellidoMaterno;
                    existeUser.Telefono = request.Telefono;
                    existeUser.Correo = request.Correo;
                    existeUser.IdRol = request.IdRol;
                    _ctx.Users.Update(existeUser);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el usuario correctamente!!";
                    response.respuesta = existeUser;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }

        public async Task<ResponseModel> deleteUsuario(UsuarioRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el usuario";
                response.respuesta = "[]";

                User existeUser = _ctx.Users.FirstOrDefault(x => x.IdUser == request.IdUser);

                if (existeUser != null)
                {
                    existeUser.Visible = false;
                    _ctx.Users.Update(existeUser);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el usuario correctamente!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Vistas
        public async Task<ResponseModel> getVistas()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay vistas para mostrar";
                response.respuesta = "[]";
                List<Vista> lista = await _ctx.Vistas.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las vistas!!";
                    response.respuesta = lista;
                }

                return response;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region VistasRoles
        public async Task<ResponseModel> getVistasRol(int idRol)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay vistas por rol para mostrar";
                response.respuesta = "[]";
                List<VistasRol> vistasRol = await _ctx.VistasRols.Where(x => x.IdRol == idRol).ToListAsync();
                if (vistasRol != null)
                {
                    var vistaRol = _ctx.VistasRols.Include(v => v.IdVistaNavigation).Include(x => x.IdRolNavigation)
                        .Where(y => y.IdRol == idRol).OrderBy(or => or.IdVistaNavigation.Posicion).ToList()
                        .ConvertAll<VistasRolResponse>(r => new VistasRolResponse
                        {
                            idVistaRol = r.IdVistaRol,
                            idRol = r.IdRol,
                            rol = r.IdRolNavigation.Descripcion,
                            idVista = r.IdVista,
                            vista = r.IdVistaNavigation.Nombre
                        });
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las vistas por rol!!";
                    response.respuesta = vistaRol;
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        public async Task<ResponseModel> postVistaRol(VistaRolRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo asignar la vista al rol";
                response.respuesta = "[]";

                VistasRol newVistaRol = new VistasRol();
                newVistaRol.IdVista = request.IdVista;
                newVistaRol.IdRol = request.IdRol;

                _ctx.VistasRols.Add(newVistaRol);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se asignó correctamente la vista correspondiente al rol!!";
                response.respuesta = newVistaRol;

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        public async Task<ResponseModel> putVistaRol(VistaRolRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar la vista del rol";
                response.respuesta = "[]";

                VistasRol existeVistaRol = _ctx.VistasRols.FirstOrDefault(x => x.IdVistaRol == request.IdVistaRol);

                if (existeVistaRol != null)
                {
                    existeVistaRol.IdVista = request.IdVista;
                    existeVistaRol.IdRol = request.IdRol;

                    _ctx.Update(existeVistaRol);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó correctamente la vista correspondiente al rol!!";
                    response.respuesta = existeVistaRol;
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        public async Task<ResponseModel> deleteVistaRol(VistaRolRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar la vista del rol";
                response.respuesta = "[]";

                VistasRol existeVistaRol = _ctx.VistasRols.FirstOrDefault(x => x.IdVistaRol == request.IdVistaRol);

                if (existeVistaRol != null)
                {
                    
                    _ctx.Remove(existeVistaRol);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó correctamente la vista correspondiente al rol!!";
                    response.respuesta = "[]";
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                return response;
            }
        }
        #endregion

        #region Herramientas
        private UserTokens generarToken(User user)
        {
            try
            {

                var Token = new UserTokens();
                Token = JwtHelpers.GenTokenkey(new UserTokens()
                {
                    EmailId = user.Correo,
                    GuidId = Guid.NewGuid(),
                    UserName = user.Nombre + " " +user.ApellidoPaterno + " " +user.ApellidoMaterno,
                    Id = user.IdUser,
                }, _jwtSettings);
                return Token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public class Encrypt
        {
            public static string GetSHA1(string str)
            {
                SHA1 sha1 = SHA1Managed.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha1.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }
        #endregion


    }
}
