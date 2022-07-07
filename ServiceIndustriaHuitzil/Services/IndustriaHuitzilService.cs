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
           // _connectionString = "Server=localhost;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=Ventana0512";
            _connectionString = "Server=DESKTOP-GHBL8TT\\SQLEXPRESS;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=Ventana0512";
            _configuration = configuration;
            _jwtSettings = jwtSettings;
        }

        #region Auth
        public async Task<object> auth(AuthUserRequest authRequest)
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

        #region Roles
        public async Task<object> getRoles()
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
        public async Task<object> postRol(RolRequest request)
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
        public async Task<object> putRol(RolRequest request)
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
        public async Task<object> deleteRol(RolRequest request)
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

        #region Ubicaciones
        public async Task<object> getUbicaciones()
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

        public async Task<object> postUbicacion(UbicacionRequest request)
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

        public async Task<object> putUbicacion(UbicacionRequest request)
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

        public async Task<object> deleteUbicacion(UbicacionRequest request)
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

        #region Usuarios
        public async Task<object> getUsuarios()
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

        public async Task<object> postUsuario(UsuarioRequest request)
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

        public async Task<object> putUsuario(UsuarioRequest request)
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

        public async Task<object> deleteUsuario(UsuarioRequest request)
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
        public async Task<object> getVistas()
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
        public async Task<object> getVistasRol(int idRol)
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
        public async Task<object> postVistaRol(VistaRolRequest request)
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
        public async Task<object> putVistaRol(VistaRolRequest request)
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
        public async Task<object> deleteVistaRol(VistaRolRequest request)
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
