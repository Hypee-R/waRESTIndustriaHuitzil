using AccessControl.JwtHelpers;
using AccessControl.Models;
using CoreIndustriaHuitzil.Models;
using CoreIndustriaHuitzil.ModelsRequest;
using CoreIndustriaHuitzil.ModelsResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
            _connectionString = "Server=localhost;Database=IndustriaHuitzil;Trusted_Connection=false;MultipleActiveResultSets=true;User ID=sa;Password=Ventana0512";
            _configuration = configuration;
            _jwtSettings = jwtSettings;
        }

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
                    dataLogin.apellidoMaterno = existeUsuario.ApellidoPaterno;
                    dataLogin.correo = existeUsuario.Correo;
                    dataLogin.telefono = existeUsuario.Telefono;
                    dataLogin.token = existeUsuario.Token;
                    dataLogin.ultimoAcceso = existeUsuario.UltimoAcceso.ToString();
                    dataLogin.idRol = existeUsuario.IdRolNavigation.IdRol;
                    dataLogin.rol = existeUsuario.IdRolNavigation.Descripcion;

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
    }
}
