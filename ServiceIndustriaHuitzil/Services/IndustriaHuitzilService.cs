using AccessControl.JwtHelpers;
using AccessControl.Models;
using CoreIndustriaHuitzil.Models;
using CoreIndustriaHuitzil.ModelsRequest;
using CoreIndustriaHuitzil.ModelsResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace ServiceIndustriaHuitzil.Services
{
    public class IndustriaHuitzilService : IIndustriaHuitzilService
    {
        private readonly IndustriaHuitzilDbContext _ctx;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public IndustriaHuitzilService(
            IndustriaHuitzilDbContext ctx,
            IConfiguration configuration,
            JwtSettings jwtSettings
            )
        {
            _ctx = ctx;
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
                    existeUsuario.ExpiredTime = DateTime.Now.AddDays(1);
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
                    dataLogin.expiredTime = (DateTime)existeUsuario.ExpiredTime;
                    dataLogin.pc = existeUsuario.pc;

                    respuesta.exito = true;
                    respuesta.mensaje = "Credenciales correctas!!";
                    respuesta.respuesta = dataLogin;
                }

               return respuesta;
           }
           catch (Exception ex)
           {
                respuesta.mensaje = ex.Message;
                Console.WriteLine(ex);
                return respuesta;
           }
        
        }
        #endregion

        #region Apartados
        public async Task<ResponseModel> getApartados()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay clientes para mostrar";
                response.respuesta = "[]";
                List<ApartadosRequest> apartados = new List<ApartadosRequest>();
                apartados = _ctx.Apartados.Include(a => a.IdTallaNavigation).Include(b => b.IdArticuloNavigation).Include(c=>c.IdClienteNavigation).OrderByDescending(apartado => apartado.Status).ToList()
                     .ConvertAll(u => new ApartadosRequest()
                     {
                         IdApartado = u.IdApartado,
                         IdEmpleado = u.IdEmpleado,
                         idArticulo = u.idArticulo,
                         IdTalla = u.IdTalla,
                         Telefono = u.Telefono,
                         Direccion = u.Direccion,
                         Fecha = (DateTime)u.Fecha,
                         FechaEntrega = (DateTime?)u.FechaEntrega,
                         Status = u.Status,
                         talla = u.IdTallaNavigation.Descripcion,
                         articulo = u.IdArticuloNavigation.Descripcion,
                         precio = u.IdArticuloNavigation.Precio,
                         cliente = u.IdClienteNavigation.Nombre + " "+ u.IdClienteNavigation.ApellidoPaterno
                         //cliente = u.


                     });
                if (apartados != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los apartados!!";
                    response.respuesta = apartados;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getApartadosByUser(int IdUsuario,string type)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay Apartados para mostrar";
                response.respuesta = null;
                List<ApartadosRequest> apartados = new List<ApartadosRequest>();
                apartados =  _ctx.Apartados.Include(a => a.IdTallaNavigation).Include(b=>b.IdArticuloNavigation).Where(x => x.IdEmpleado == IdUsuario  && x.Type == type).OrderByDescending(apartado => apartado.Status).ToList()
                    .ConvertAll(u => new ApartadosRequest()
                {
                    IdApartado = u.IdApartado,
                    IdEmpleado = u.IdEmpleado,
                    idArticulo = u.idArticulo,
                    IdTalla = u.IdTalla,
                    Telefono = u.Telefono,
                    Direccion = u.Direccion,
                    Fecha = (DateTime)u.Fecha,
                    FechaEntrega = (DateTime?)u.FechaEntrega,
                    Status = u.Status,
                    talla = u.IdTallaNavigation.Descripcion,
                    articulo = u.IdArticuloNavigation.Descripcion,
                    precio = u.IdArticuloNavigation.Precio
                    
                
                });
                if (apartados != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los apartados!!";
                    response.respuesta = apartados;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = null;
            }
            return response;
        }

        public async Task<ResponseModel> postApartados(ApartadosRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo hacer el apartado";
                response.respuesta = "[]";

                Apartados newApartado = new Apartados();
                newApartado.idArticulo = request.idArticulo;
                newApartado.IdEmpleado = request.IdEmpleado;
                newApartado.Telefono = request.Telefono;
                newApartado.IdTalla = request.IdTalla;
                newApartado.Fecha = (DateTime)request.Fecha;
                newApartado.Direccion = request.Direccion;
                newApartado.Status = "Espera";
                newApartado.Type = request.type;
                _ctx.Apartados.Add(newApartado);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el cliente correctamente!!";
                response.respuesta = newApartado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> putApartados(ApartadosRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo entregar el producto";
                response.respuesta = "[]";

                Apartados existeApartado = _ctx.Apartados.FirstOrDefault(x => x.IdApartado == request.IdApartado);
                //CatCliente existeCliente = _ctx.CatClientes.FirstOrDefault(x => x.IdCliente == request.IdApartado);

                if (existeApartado != null)
                {
                    existeApartado.idArticulo = request.idArticulo;
                    existeApartado.IdEmpleado = request.IdEmpleado;
                    existeApartado.Telefono = request.Telefono;
                    existeApartado.IdTalla = request.IdTalla;
                    existeApartado.Fecha = (DateTime)request.Fecha;
                    existeApartado.FechaEntrega = (DateTime)request.FechaEntrega;
                    existeApartado.Direccion = request.Direccion;
                    existeApartado.Status = request.Status;
                    /* existeCliente.Nombre = request.Nombre;
                     existeCliente.ApellidoPaterno = request.ApellidoPaterno;
                     existeCliente.ApellidoMaterno = request.ApellidoMaterno;
                     existeCliente.Telefono1 = request.Telefono1;
                     existeCliente.Telefono2 = request.Telefono2;
                     existeCliente.Direccion = request.Direccion;*/
                    _ctx.Apartados.Update(existeApartado);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se entrego el producto correctamente";
                    response.respuesta = existeApartado;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> deleteApartados(ApartadosRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el proveedor";
                response.respuesta = "[]";

                //CatCliente existeCliente = _ctx.CatClientes.FirstOrDefault(x => x.IdCliente == request.IdCliente);

                /*if (existeCliente != null)
                {
                    existeCliente.Visible = false;
                    _ctx.CatClientes.Update(existeCliente);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el cliente correctamente!!";
                    response.respuesta = "[]";
                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        #endregion

        #region Caja
        public async Task<ResponseModel> getCaja(int idUser)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay una caja para mostrar";
                response.respuesta = "[]";

                Caja existeCaja = await _ctx.Cajas.Where(a => a.IdEmpleado == idUser).OrderByDescending(x => x.IdCaja).FirstOrDefaultAsync();
                if (existeCaja != null)
                {
                    CajaRequest caja = new CajaRequest();
                    response.exito = true;
                    response.mensaje = "Se ha consultado la caja exitosamente!";
                    caja.IdCaja = existeCaja.IdCaja;
                    caja.IdEmpleado = existeCaja.IdEmpleado;
                    caja.Fecha = existeCaja.Fecha.ToString();
                    caja.Monto = existeCaja.Monto;
                    caja.FechaCierre = existeCaja.FechaCierre != null ? existeCaja.FechaCierre.ToString() : null;
                    caja.MontoCierre = existeCaja.MontoCierre;
                    response.respuesta = caja;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> openCaja(CajaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se puede abrir la caja, hay una abierta";
                response.respuesta = "[]";

                Caja existCaja = _ctx.Cajas.Where(a => a.IdEmpleado == request.IdEmpleado).OrderByDescending(x => x.Fecha).FirstOrDefault();

                var caja = existCaja;

                if (caja == null)
                {
                    Caja newCaja = new Caja();
                    response.exito = true;
                    response.mensaje = "Caja abierta exitosamente!";

                    DateTime fechaOpen = setFormatDate(request.Fecha);

                    newCaja.Fecha = fechaOpen;
                    newCaja.Monto = request.Monto;
                    newCaja.IdEmpleado = request.IdEmpleado;
                    _ctx.Cajas.Add(newCaja);
                    await _ctx.SaveChangesAsync();

                    response.respuesta = newCaja;
                }
                else if (caja.FechaCierre != null)
                {
                    Caja newCaja = new Caja();
                    response.exito = true;
                    response.mensaje = "Caja abierta exitosamente!";
                    DateTime fechaOpen = setFormatDate(request.Fecha);

                    newCaja.Fecha = fechaOpen;
                    newCaja.Monto = request.Monto;
                    newCaja.IdEmpleado = request.IdEmpleado;
                    _ctx.Cajas.Add(newCaja);
                    await _ctx.SaveChangesAsync();

                    response.respuesta = newCaja;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> closeCaja(CajaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay una caja abierta para cerrar";
                response.respuesta = "[]";

                Caja existeCaja = _ctx.Cajas.Where(a => a.IdCaja == request.IdCaja).FirstOrDefault();
                if (existeCaja != null && existeCaja.FechaCierre == null)
                {
                    response.exito = true;
                    response.mensaje = "Caja cerrada exitosamente!";

                    DateTime fechaCierre = setFormatDate(request.FechaCierre);

                    existeCaja.FechaCierre = fechaCierre;
                    existeCaja.MontoCierre = request.MontoCierre;
                    _ctx.Cajas.Update(existeCaja);
                    await _ctx.SaveChangesAsync();

                    response.respuesta = existeCaja;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }
        #endregion

        #region Cambios y Devoluciones
        public async Task<ResponseModel> searchVentaByNoTicket(string noTicket)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "Venta no encontrada";
                response.respuesta = "[]";
                List<VentaRequest> resultadoVenta = new List<VentaRequest>();
                VentaRequest venta = new VentaRequest();
                Venta result = await _ctx.Ventas.Where(x => x.NoTicket == noTicket).FirstOrDefaultAsync();

                if (result != null)
                {
                    List<VentaArticulo> articulosVenta = await _ctx.VentaArticulos.Include(w => w.IdArticuloNavigation)
                                                                              .Include(wa => wa.IdArticuloNavigation.IdUbicacionNavigation)
                                                                              .Include(wb => wb.IdArticuloNavigation.IdCategoriaNavigation)
                                                                              .Include(wc => wc.IdArticuloNavigation.IdTallaNavigation)
                                                                              .Where(x => x.IdVenta == result.IdVenta).ToListAsync();
                    venta.IdVenta = result.IdVenta;
                    venta.IdCaja = result.IdCaja;
                    venta.Fecha = result.Fecha.ToString();
                    venta.NoTicket = result.NoTicket;
                    venta.TipoPago = result.TipoPago;
                    venta.TipoVenta = result.TipoVenta;
                    venta.NoArticulos = result.NoArticulos;
                    venta.Subtotal = result.Subtotal;
                    venta.Total = result.Total;
                    venta.ventaArticulo = articulosVenta.ConvertAll(x => new VentaArticuloRequest()
                    {
                        IdVentaArticulo = x.IdVentaArticulo,
                        IdVenta = x.IdVenta,
                        IdArticulo = x.IdArticulo,
                        Cantidad = x.Cantidad,
                        PrecioUnitario = x.PrecioUnitario,
                        Subtotal = x.Subtotal,
                        Articulo = new ProductoRequest()
                        {
                            IdArticulo = x.IdArticulo,
                            Unidad = x.IdArticuloNavigation.Unidad,
                            Existencia = x.IdArticuloNavigation.Existencia,
                            Descripcion = x.IdArticuloNavigation.Descripcion,
                            FechaIngreso = (DateTime)x.IdArticuloNavigation.FechaIngreso,
                            idUbicacion = (int)x.IdArticuloNavigation.IdUbicacion,
                            idCategoria = (int)x.IdArticuloNavigation.IdCategoria,
                            idTalla = (int)x.IdArticuloNavigation.IdTalla,
                            talla = x.IdArticuloNavigation.IdTallaNavigation.Descripcion,
                            categoria = x.IdArticuloNavigation.IdCategoriaNavigation.Descripcion,
                            ubicacion = x.IdArticuloNavigation.IdUbicacionNavigation.Direccion,
                            sku = x.IdArticuloNavigation.Sku,
                            precio = (int)x.IdArticuloNavigation.Precio,
                            imagen = x.IdArticuloNavigation.Imagen
                        }
                    });
                    resultadoVenta.Add(venta);

                    response.exito = true;
                    response.mensaje = "Se consultaron los datos de la venta exitosamente!";
                    response.respuesta = resultadoVenta;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.exito = false;
                response.mensaje = e.Message;
                response.respuesta = "[]";
            }
            return response;
        }
        public async Task<ResponseModel> getCambiosyDevoluciones()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay cambios y devoluciones para mostrar";
                response.respuesta = "[]";
                List<CambiosDevolucionesRequest> listaResultados;

                var existeCambios = await _ctx.CambiosDevoluciones.FirstOrDefaultAsync();
                if (existeCambios != null)
                {
                    listaResultados = new List<CambiosDevolucionesRequest>();
                    response.exito = true;
                    response.mensaje = "Cambios y devoluciones consultados exitosamente!";

                    var result = await _ctx.CambiosDevoluciones.Include(a => a.IdVentaNavigation).ToListAsync();
                    listaResultados = result.ConvertAll(x => new CambiosDevolucionesRequest()
                    {
                        IdCambioDevolucion = x.IdCambioDevolucion,
                        IdVenta = x.IdVenta,
                        Fecha = x.Fecha.ToString(),
                        NoArticulos = x.NoArticulos,
                        Subtotal = x.Subtotal,
                        Total = x.Total,
                        Venta = new VentaRequest()
                        {
                            IdVenta = x.IdVentaNavigation.IdVenta,
                            IdCaja = x.IdVentaNavigation.IdCaja,
                            Fecha = x.IdVentaNavigation.Fecha.ToString(),
                            NoTicket = x.IdVentaNavigation.NoTicket,
                            TipoPago = x.IdVentaNavigation.TipoPago,
                            TipoVenta = x.IdVentaNavigation.TipoVenta,
                            NoArticulos = x.IdVentaNavigation.NoArticulos,
                            Subtotal = x.IdVentaNavigation.Subtotal,
                            Total = x.IdVentaNavigation.Total
                        },
                        CambiosDevolucionesArticulos = _ctx.CambiosDevolucionesArticulos.Include(a => a.IdArticuloNavigation).ThenInclude(aa => aa.IdTallaNavigation)
                                                                                        .Include(b => b.IdVentaArticuloNavigation).ThenInclude(ba => ba.IdArticuloNavigation)
                                                                                        .ThenInclude(baa => baa.IdTallaNavigation).Where(y => y.IdCambioDevolucion == x.IdCambioDevolucion)
                                                                                        .ToList().ConvertAll(x => new CambiosDevolucionesArticuloRequest()
                        {
                            IdCambioArticulo = x.IdCambioArticulo,
                            IdCambioDevolucion = x.IdCambioDevolucion,
                            IdVentaArticulo = x.IdVentaArticulo,
                            IdArticulo = x.IdArticulo,
                            Cantidad = x.Cantidad,
                            Estado = x.Estado,
                            MotivoCambio = x.MotivoCambio,
                            PrecioAnterior = x.PrecioAnterior,
                            PrecioActual = x.PrecioActual,
                            Deducible = x.Deducible,
                            Articulo = new ProductoRequest()
                            {
                                IdArticulo = x.IdArticuloNavigation.IdArticulo,
                                Unidad = x.IdArticuloNavigation.Unidad,
                                Existencia = x.IdArticuloNavigation.Existencia,
                                Descripcion = x.IdArticuloNavigation.Descripcion,
                                FechaIngreso = (DateTime)x.IdArticuloNavigation.FechaIngreso,
                                idUbicacion = (int)x.IdArticuloNavigation.IdUbicacion,
                                idCategoria = (int)x.IdArticuloNavigation.IdCategoria,
                                idTalla = (int)x.IdArticuloNavigation.IdTalla,
                                talla = x.IdArticuloNavigation.IdTallaNavigation.Nombre,
                                categoria = null,
                                ubicacion = null,
                                sku = x.IdArticuloNavigation.Sku,
                                precio = (int)x.IdArticuloNavigation.Precio,
                                imagen = x.IdArticuloNavigation.Imagen
                            },
                            VentaArticulo = new VentaArticuloRequest()
                            {
                                IdVentaArticulo = x.IdVentaArticuloNavigation.IdVentaArticulo,
                                IdVenta = x.IdVentaArticuloNavigation.IdVenta,
                                IdArticulo = x.IdVentaArticuloNavigation.IdArticulo,
                                Cantidad = x.IdVentaArticuloNavigation.Cantidad,
                                PrecioUnitario = x.IdVentaArticuloNavigation.PrecioUnitario,
                                Subtotal = x.IdVentaArticuloNavigation.Subtotal,
                                Articulo = new ProductoRequest()
                                {
                                    IdArticulo = x.IdVentaArticuloNavigation.IdArticuloNavigation.IdArticulo,
                                    Unidad = x.IdVentaArticuloNavigation.IdArticuloNavigation.Unidad,
                                    Existencia = x.IdVentaArticuloNavigation.IdArticuloNavigation.Existencia,
                                    Descripcion = x.IdVentaArticuloNavigation.IdArticuloNavigation.Descripcion,
                                    FechaIngreso = (DateTime)x.IdVentaArticuloNavigation.IdArticuloNavigation.FechaIngreso,
                                    idUbicacion = (int)x.IdVentaArticuloNavigation.IdArticuloNavigation.IdUbicacion,
                                    idCategoria = (int)x.IdVentaArticuloNavigation.IdArticuloNavigation.IdCategoria,
                                    idTalla = (int)x.IdVentaArticuloNavigation.IdArticuloNavigation.IdTalla,
                                    talla = x.IdVentaArticuloNavigation.IdArticuloNavigation.IdTallaNavigation.Nombre,
                                    categoria = null,
                                    ubicacion = null,
                                    sku = x.IdVentaArticuloNavigation.IdArticuloNavigation.Sku,
                                    precio = (int)x.IdVentaArticuloNavigation.IdArticuloNavigation.Precio,
                                    imagen = x.IdVentaArticuloNavigation.IdArticuloNavigation.Imagen
                                }
                            }
                        })
                    });

                    response.respuesta = listaResultados;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> postCambiosyDevoluciones(CambiosDevolucionesRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo registrar el cambio y/o devolución";
                response.respuesta = "[]";

                CambiosDevolucione existeCambio = await _ctx.CambiosDevoluciones.FirstOrDefaultAsync(x => x.IdVenta == request.IdVenta);
                if (existeCambio == null)
                {
                    CambiosDevolucione newCambioDevolucion = new CambiosDevolucione();

                    using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            
                            DateTime fechaCambio = setFormatDate(request.Fecha);
                            newCambioDevolucion.IdVenta = request.IdVenta;
                            newCambioDevolucion.Fecha = fechaCambio;
                            newCambioDevolucion.NoArticulos = request.NoArticulos;
                            newCambioDevolucion.Subtotal = request.Subtotal;
                            newCambioDevolucion.Total = request.Total;

                            _ctx.Add(newCambioDevolucion);
                            await _ctx.SaveChangesAsync();

                            //Inserta los articulos cambiados o devueltos si es que tiene
                            if (request.CambiosDevolucionesArticulos?.Count() > 0)
                            {
                                List<CambiosDevolucionesArticulo> lstCambiosDevolucionesArticulos = new List<CambiosDevolucionesArticulo>();
                                request.CambiosDevolucionesArticulos.ForEach(dataArticulo =>
                                {
                                    VentaArticulo ventaArticulo = _ctx.VentaArticulos.FirstOrDefault(x => x.IdVentaArticulo == dataArticulo.IdVentaArticulo);
                                    lstCambiosDevolucionesArticulos.Add(new CambiosDevolucionesArticulo()
                                    {
                                        IdCambioArticulo = dataArticulo.IdCambioArticulo,
                                        IdCambioDevolucion = newCambioDevolucion.IdCambioDevolucion,
                                        IdVentaArticulo = dataArticulo.IdVentaArticulo,
                                        IdArticulo = dataArticulo.IdArticulo,
                                        Cantidad = dataArticulo.Cantidad,
                                        Estado = dataArticulo.Estado,
                                        MotivoCambio = dataArticulo.MotivoCambio,
                                        PrecioAnterior = dataArticulo.PrecioAnterior,
                                        PrecioActual = dataArticulo.PrecioActual,
                                        Deducible = dataArticulo.Deducible
                                    });

                                    //Actualiza el stock
                                    Articulo articuloVenta = _ctx.Articulos.FirstOrDefault(x => x.IdArticulo == ventaArticulo.IdArticulo);
                                    Articulo articuloCambio = _ctx.Articulos.FirstOrDefault(x => x.IdArticulo == dataArticulo.IdArticulo);

                                    articuloVenta.Existencia = (Int32.Parse(articuloVenta.Existencia) + dataArticulo.Cantidad).ToString();
                                    if ( (Int32.Parse(articuloCambio.Existencia) - dataArticulo.Cantidad) >= 0 )
                                    {
                                        articuloCambio.Existencia = (Int32.Parse(articuloCambio.Existencia) - dataArticulo.Cantidad).ToString();
                                    }
                                    else
                                    {
                                        response.exito = false;
                                        response.mensaje = "Ya no hay stock del articulo para cambio!";
                                        response.respuesta = "[]";
                                        dbContextTransaction.Rollback();
                                    }

                                    _ctx.Articulos.Update(articuloVenta);
                                    _ctx.Articulos.Update(articuloCambio);
                                    
                                });
                                
                                _ctx.CambiosDevolucionesArticulos.AddRange(lstCambiosDevolucionesArticulos);
                                await _ctx.SaveChangesAsync();

                            }


                            //Hacemos commit de todos los datos
                            dbContextTransaction.Commit();
                            response.exito = true;
                            response.mensaje = "Se ha registrado el cambio y/o devolucion!";
                            response.respuesta = "[]";

                        }
                        catch (Exception ex)
                        {
                            response.exito = false;
                            response.mensaje = ex.Message;
                            response.respuesta = "[]";
                            dbContextTransaction.Rollback();
                            return response;
                        }
                    }

                }
                else
                {
                    response.mensaje = "La venta ya tiene algún cambio realizado";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }
        #endregion

        #region Clientes
        public async Task<ResponseModel> getClientes()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay clientes para mostrar";
                response.respuesta = "[]";

                List<CatCliente> lista = await _ctx.CatClientes.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los clientes!!";
                    response.respuesta = lista;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> postCliente(ClienteRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo cliente";
                response.respuesta = "[]";

                CatCliente newCliente = new CatCliente();
                newCliente.Nombre = request.Nombre;
                newCliente.ApellidoPaterno = request.ApellidoPaterno;
                newCliente.ApellidoMaterno = request.ApellidoMaterno;
                newCliente.Telefono1 = request.Telefono1;
                newCliente.Telefono2 = request.Telefono2;
                newCliente.Direccion = request.Direccion;
                newCliente.Visible = true;
                _ctx.CatClientes.Add(newCliente);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el cliente correctamente!!";
                response.respuesta = newCliente;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> putCliente(ClienteRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar el cliente";
                response.respuesta = "[]";

                CatCliente existeCliente = _ctx.CatClientes.FirstOrDefault(x => x.IdCliente == request.IdCliente);

                if (existeCliente != null)
                {
                    existeCliente.Nombre = request.Nombre;
                    existeCliente.ApellidoPaterno = request.ApellidoPaterno;
                    existeCliente.ApellidoMaterno = request.ApellidoMaterno;
                    existeCliente.Telefono1 = request.Telefono1;
                    existeCliente.Telefono2 = request.Telefono2;
                    existeCliente.Direccion = request.Direccion;
                    _ctx.CatClientes.Update(existeCliente);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el cliente correctamente!!";
                    response.respuesta = existeCliente;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> deleteCliente(ClienteRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar el proveedor";
                response.respuesta = "[]";

                CatCliente existeCliente = _ctx.CatClientes.FirstOrDefault(x => x.IdCliente == request.IdCliente);

                if (existeCliente != null)
                {
                    existeCliente.Visible = false;
                    _ctx.CatClientes.Update(existeCliente);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó el cliente correctamente!!";
                    response.respuesta = "[]";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        #endregion

        #region Dashboard
        public async Task<ResponseModel> getCards(string year, int idSucursal)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<CardResponse> cardsResponse = new List<CardResponse>();
                decimal? ganancias = 0;
                decimal? cambios = 0;
                double? gastosTotales = 0;


                var fechaInicio = DateTime.ParseExact("01/01/"+year, "dd/MM/yyyy", null);
                var fechaFin = DateTime.ParseExact("31/12/"+year, "dd/MM/yyyy", null);

                if (idSucursal == 0)
                {
                    ganancias = await _ctx.Ventas.Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin).SumAsync(x => x.Total);
                    cambios = await _ctx.CambiosDevoluciones.Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin).SumAsync(x => x.Total);
                    gastosTotales = await _ctx.SolicitudesMateriales.Where(x => x.Fecha >= fechaInicio && x.Fecha <= fechaFin && x.Status.Trim().ToLower().Equals("COMPLETADO")).SumAsync(x => x.CostoTotal);
                }
                else
                {
                    ganancias = await _ctx.VentaArticulos.Include(w => w.IdVentaNavigation).Include(ww => ww.IdArticuloNavigation)
                                                         .Where(x => x.IdVentaNavigation.Fecha >= fechaInicio && x.IdVentaNavigation.Fecha <= fechaFin && x.IdArticuloNavigation.IdUbicacion == idSucursal).SumAsync(x => x.IdVentaNavigation.Total);
                    cambios = await _ctx.CambiosDevolucionesArticulos.Include(w => w.IdCambioDevolucionNavigation).Include(ww => ww.IdArticuloNavigation)
                                                        .Where(x => x.IdCambioDevolucionNavigation.Fecha >= fechaInicio && x.IdCambioDevolucionNavigation.Fecha <= fechaFin).SumAsync(x => x.IdCambioDevolucionNavigation.Total);
                    var query = from sm in _ctx.SolicitudesMateriales
                                join pm in _ctx.ProveedoresMateriales on sm.IdProveedorMaterial equals pm.IdProveedorMaterial
                                join m in _ctx.Materiales on pm.IdMaterial equals m.IdMaterial
                                join mu in _ctx.MaterialesUbicaciones on m.IdMaterial equals mu.IdMaterial
                                where sm.Fecha >= fechaInicio && sm.Fecha <= fechaFin && mu.IdUbicacion == idSucursal
                                select sm.CostoTotal;

                    gastosTotales = query.ToList().Sum(x => x.Value);
                }

                
                float gananciasTotales = float.Parse(ganancias.ToString()) + (float.Parse(cambios.ToString()));
                
                var ingresosTotales = float.Parse(gananciasTotales.ToString()) - float.Parse(gastosTotales.ToString());

                cardsResponse.Add(new CardResponse() { title = "Ganancias Totales", value = gananciasTotales.ToString(), aditionalValue = "" });
                cardsResponse.Add(new CardResponse() { title = "Gastos Totales", value = gastosTotales.ToString(), aditionalValue = "" });
                cardsResponse.Add(new CardResponse() { title = "Ingresos Totales", value = ingresosTotales.ToString(), aditionalValue = "" });

                response.exito = true;
                response.mensaje = "Datos Analitycs Ventas";
                response.respuesta = cardsResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.exito = false;
                response.mensaje = e.Message;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getVentasPorMes(string year, int idSucursal)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ChartBarResponse chartBarResponse = new ChartBarResponse();
                List<Series> series = new List<Series>();
                int[] seriesEfectivo = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] seriesTarjeta  = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] seriesMultiple = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                using (var con = _ctx.Database.GetDbConnection())
                {
                    con.Open();
                    DbCommand cmd = con.CreateCommand();

                    string query = "";
                    if (idSucursal == 0)
                    {
                        query = "SELECT DATEPART(MONTH, fecha) AS 'Mes', tipo_pago, Count(id_venta) AS 'No Ventas' FROM Ventas "+
                                   "WHERE DATEPART(YEAR, fecha) = "+ year + " " +
                                   "GROUP BY DATEPART(MONTH, fecha), tipo_pago "+
                                   "ORDER BY Mes";
                    }
                    else
                    {
                        query = "SELECT DATEPART(MONTH, v.fecha) AS 'Mes', v.tipo_pago, Count(v.id_venta) AS 'No Ventas' FROM Ventas AS V " +
                                       "INNER JOIN VentaArticulos AS VA ON V.id_venta = VA.id_venta " +
                                       "INNER JOIN Articulos as A ON VA.id_articulo = A.id_articulo " +
                                       "WHERE DATEPART(YEAR, v.fecha) = " + year + " AND A.id_ubicacion = " + idSucursal + " " +
                                       "GROUP BY DATEPART(MONTH, v.fecha), tipo_pago " +
                                       "ORDER BY Mes";
                    }

                    cmd.CommandText = query;
                    
                    using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
                    {
                        while (rdr.Read())
                        {
                            int mes = rdr.GetInt32(0);
                            string tipoPago = rdr.GetString(1);
                            if (tipoPago == "EFECTIVO")
                            {
                                seriesEfectivo[mes-1] = rdr.GetInt32(2);
                            }else if (tipoPago == "TARJETA")
                            {
                                seriesTarjeta[mes-1] = rdr.GetInt32(2);
                            }
                            else if (tipoPago == "MULTIPLE")
                            {
                                seriesMultiple[mes-1] = rdr.GetInt32(2);
                            }
                        }
                    }
                }

                series.Add(new Series() { name = "EFECTIVO", data = seriesEfectivo.ToList() });
                series.Add(new Series() { name = "TARJETA", data = seriesTarjeta.ToList() });
                series.Add(new Series() { name = "MULTIPLE", data = seriesMultiple.ToList() });

                chartBarResponse.title = "Ventas por Mes año " + year;
                chartBarResponse.series = series;

                response.exito = true;
                response.mensaje = "Datos Gráfica Ventas por Mes";
                response.respuesta = chartBarResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.exito = false;
                response.mensaje = e.Message;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getRankingArticulos(string year, int idSucursal)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                RankingResponse rankingArticulosResponse = new RankingResponse();

                using (var con = _ctx.Database.GetDbConnection())
                {
                    con.Open();
                    DbCommand cmd = con.CreateCommand();
                    string query = "";
                    if (idSucursal == 0)
                    {
                        query = "SELECT TOP 3 * FROM "+
                                       "(SELECT a.descripcion AS 'Articulo', SUM(va.cantidad) AS 'No Ventas' FROM VentaArticulos AS va "+
                                       "INNER JOIN Articulos AS a "+
                                       "ON va.id_articulo = a.id_articulo "+
                                       "INNER JOIN Ventas AS v "+
                                       "ON va.id_venta = v.id_venta "+
                                       "WHERE DATEPART(YEAR, v.fecha) = "+ year + " " +
                                       "GROUP BY va.id_articulo, a.descripcion) AS ranking_articles "+
                                       "ORDER BY [No Ventas] DESC";
                    }
                    else
                    {
                        query = "SELECT TOP 3 * FROM " +
                                       "(SELECT a.descripcion AS 'Articulo', SUM(va.cantidad) AS 'No Ventas' FROM VentaArticulos AS va " +
                                       "INNER JOIN Articulos AS a " +
                                       "ON va.id_articulo = a.id_articulo " +
                                       "INNER JOIN Ventas AS v " +
                                       "ON va.id_venta = v.id_venta " +
                                       "WHERE DATEPART(YEAR, v.fecha) = " + year + " AND a.id_ubicacion = " + idSucursal + " "+
                                       "GROUP BY va.id_articulo, a.descripcion) AS ranking_articles " +
                                       "ORDER BY [No Ventas] DESC";
                    }

                    cmd.CommandText = query;

                    using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
                    {
                        while (rdr.Read())
                        {
                            rankingArticulosResponse.ranking.Add(new DataRanking() { descripcion = rdr.GetString(0), noVentas = rdr.GetInt32(1).ToString() });
                        }
                    }
                }

                rankingArticulosResponse.title = "Articulos más vendidos año " + year;

                response.exito = true;
                response.mensaje = "Datos Ranking Articulos";
                response.respuesta = rankingArticulosResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.exito = false;
                response.mensaje = e.Message;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getRankingEmpleados(string year, int idSucursal)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                RankingResponse rankingEmpleadosResponse = new RankingResponse();

                using (var con = _ctx.Database.GetDbConnection())
                {
                    con.Open();
                    DbCommand cmd = con.CreateCommand();
                    string query = "";
                    if (idSucursal == 0)
                    {
                        query = "SELECT TOP 3 * FROM " +
                                       "(SELECT u.usuario AS 'Empleado', SUM(v.no_articulos) AS 'No Ventas' FROM Ventas AS v " +
                                       "INNER JOIN Caja AS c " +
                                       "ON v.id_caja = c.id_caja " +
                                       "INNER JOIN Users AS u " +
                                       "ON c.id_empleado = u.id_user " +
                                       "WHERE DATEPART(YEAR, v.fecha) = " + year + " " +
                                       "GROUP BY u.id_user, u.usuario) AS ranking_empleados " +
                                       "ORDER BY [No Ventas] DESC";
                    }
                    else
                    {
                        query = "SELECT TOP 3 * FROM " +
                                       "(SELECT u.usuario AS 'Empleado', SUM(v.no_articulos) AS 'No Ventas' FROM Ventas AS v " +
                                       "INNER JOIN VentaArticulos AS va " +
                                       "ON v.id_venta = va.id_venta " +
                                       "INNER JOIN Articulos AS a " +
                                       "ON va.id_articulo = a.id_articulo " +
                                       "INNER JOIN Caja AS c " +
                                       "ON v.id_caja = c.id_caja " +
                                       "INNER JOIN Users AS u " +
                                       "ON c.id_empleado = u.id_user " +
                                       "WHERE DATEPART(YEAR, v.fecha) = " + year + " AND a.id_ubicacion = " + idSucursal + " " +
                                       "GROUP BY u.id_user, u.usuario) AS ranking_empleados " +
                                       "ORDER BY [No Ventas] DESC";
                    }

                    cmd.CommandText = query;

                    using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
                    {
                        while (rdr.Read())
                        {
                            rankingEmpleadosResponse.ranking.Add(new DataRanking() { descripcion = rdr.GetString(0), noVentas = rdr.GetInt32(1).ToString() });
                        }
                    }
                }

                rankingEmpleadosResponse.title = "Empleados con más ventas año " + year;

                response.exito = true;
                response.mensaje = "Datos Ranking Empleados";
                response.respuesta = rankingEmpleadosResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.exito = false;
                response.mensaje = e.Message;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getRankingSucursales(string year)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                RankingResponse rankingSucursalesResponse = new RankingResponse();

                using (var con = _ctx.Database.GetDbConnection())
                {
                    con.Open();
                    DbCommand cmd = con.CreateCommand();
                    string query = "SELECT TOP 3 * FROM "+
                                   "(SELECT u.direccion AS 'Sucursal', SUM(va.cantidad) AS 'No Ventas' FROM VentaArticulos AS va "+
                                   "INNER JOIN Ventas AS v "+
                                   "ON v.id_venta = va.id_venta "+
                                   "INNER JOIN Articulos AS a "+
                                   "ON va.id_articulo = a.id_articulo "+
                                   "INNER JOIN CatUbicaciones AS u "+
                                   "ON a.id_ubicacion = u.id_ubicacion "+
                                   "WHERE DATEPART(YEAR, v.fecha) = "+ year + " " +
                                   "GROUP BY u.id_ubicacion, u.direccion) AS ranking_sucursales "+
                                   "ORDER BY [No Ventas] DESC";

                    cmd.CommandText = query;

                    using (DbDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
                    {
                        while (rdr.Read())
                        {
                            rankingSucursalesResponse.ranking.Add(new DataRanking() { descripcion = rdr.GetString(0), noVentas = rdr.GetInt32(1).ToString() });
                        }
                    }
                }

                rankingSucursalesResponse.title = "Sucursales con más ventas año " + year;

                response.exito = true;
                response.mensaje = "Datos Ranking Sucursales";
                response.respuesta = rankingSucursalesResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.exito = false;
                response.mensaje = e.Message;
                response.respuesta = "[]";
            }
            return response;
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

                List<MaterialRequest> listaR = new List<MaterialRequest>();
                List<Materiale> lista = await _ctx.Materiales.Where(x => x.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los materiales!!"; 
                    listaR = lista.ConvertAll(x => new MaterialRequest()
                    {
                        IdMaterial = x.IdMaterial,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        Precio = (int)x.Precio,
                        TipoMedicion = x.TipoMedicion,
                        Status = x.Status,
                        Stock = (double)x.Stock,
                        Visible = (bool)x.Visible,
                        proveedores = _ctx.ProveedoresMateriales.Include(y => y.IdProveedorNavigation)
                                          .Where(z => z.IdMaterial == x.IdMaterial)
                                          .ToList().ConvertAll(w => new ProveedorRequest()
                                          {
                                             IdProveedor = w.IdProveedor,
                                             Nombre = w.IdProveedorNavigation.Nombre,
                                             ApellidoPaterno = w.IdProveedorNavigation.ApellidoPaterno,
                                             ApellidoMaterno = w.IdProveedorNavigation.ApellidoMaterno,
                                             Telefono1 = w.IdProveedorNavigation.Telefono1,
                                             Telefono2 = w.IdProveedorNavigation.Telefono2,
                                             Correo = w.IdProveedorNavigation.Correo,
                                             Direccion = w.IdProveedorNavigation.Direccion,
                                             EncargadoNombre = w.IdProveedorNavigation.EncargadoNombre
                                          }),
                        ubicaciones = _ctx.MaterialesUbicaciones.Include(w => w.IdUbicacionNavigation)
                                          .Where(z => z.IdMaterial == x.IdMaterial)
                                          .ToList().ConvertAll(w => new UbicacionRequest()
                                          {
                                              IdUbicacion = w.IdUbicacionNavigation.IdUbicacion,
                                              Direccion = w.IdUbicacionNavigation.Direccion,
                                              NombreEncargado = w.IdUbicacionNavigation.NombreEncargado,
                                              ApellidoPEncargado = w.IdUbicacionNavigation.ApellidoPEncargado,
                                              ApellidoMEncargado = w.IdUbicacionNavigation.ApellidoMEncargado,
                                              Telefono1 = w.IdUbicacionNavigation.Telefono2,
                                              Telefono2 = w.IdUbicacionNavigation.Correo,
                                              Correo = w.IdUbicacionNavigation.Direccion
                                          })
                    });
                    response.respuesta = listaR;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> postMaterial(MaterialRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo material";
                response.respuesta = "[]";

                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {

                        Materiale newMaterial = new Materiale();

                        newMaterial.Nombre = request.Nombre;
                        newMaterial.Descripcion = request.Descripcion;
                        newMaterial.Precio = request.Precio;
                        newMaterial.TipoMedicion = request.TipoMedicion;
                        newMaterial.Status = request.Status;
                        newMaterial.Stock = request.Stock;

                        _ctx.Materiales.Add(newMaterial);
                        await _ctx.SaveChangesAsync();

                        //Inserta los proveedores del material si es que tiene
                        var insertaProveedores = await postProveedorMaterial(request.proveedores, newMaterial.IdMaterial);

                        if (insertaProveedores)
                        {
                            //Inserta las ubicaciones del material si es que tiene
                            var insertaUbicaciones = await postMaterialUbicacion(request.ubicaciones, newMaterial.IdMaterial);

                            if (!insertaUbicaciones)
                            {
                                //Hacer rollback... algo falló
                                dbContextTransaction.Rollback();
                                return response;
                            }
                        }
                        else
                        {
                            //Hacer rollback... algo falló
                            dbContextTransaction.Rollback();
                            return response;
                        }

                        //Hacemos commit de todos los datos
                        dbContextTransaction.Commit();
                        response.exito = true;
                        response.mensaje = "Se insertó el material correctamente!!";
                        response.respuesta = newMaterial;

                    }
                    catch (Exception ex)
                    {
                        response.exito = false;
                        response.mensaje = ex.Message;
                        response.respuesta = "[]";
                        dbContextTransaction.Rollback();
                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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

                    using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                    {
                        try
                        {

                            existeMaterial.Nombre = request.Nombre;
                            existeMaterial.Descripcion = request.Descripcion;
                            existeMaterial.Precio = request.Precio;
                            existeMaterial.TipoMedicion = request.TipoMedicion;
                            existeMaterial.Status = request.Status;
                            existeMaterial.Stock = request.Stock;

                            _ctx.Materiales.Update(existeMaterial);
                            await _ctx.SaveChangesAsync();

                            //Actualiza los proveedores del material si es que tiene
                            var updateProveedores = await putProveedorMaterial(request.proveedores, existeMaterial.IdMaterial);
                            if (updateProveedores)
                            {
                                //Actualiza las ubicaciones del material si es que tiene
                                var updateUbicaciones = await putMaterialUbicacion(request.ubicaciones, existeMaterial.IdMaterial);
                                if (!updateUbicaciones)
                                {
                                    //Hacer rollback... algo falló
                                    dbContextTransaction.Rollback();
                                    return response;
                                }
                            }
                            else
                            {
                                //hacer rollback... algo falló
                                dbContextTransaction.Rollback();
                                return response;
                            }


                            //Hacemos commit de todos los datos
                            dbContextTransaction.Commit();
                            response.exito = true;
                            response.mensaje = "Se actualizó el material correctamente!!";
                            response.respuesta = existeMaterial;

                        }
                        catch (Exception ex)
                        {
                            response.exito = false;
                            response.mensaje = ex.Message;
                            response.respuesta = "[]";
                            dbContextTransaction.Rollback();
                            return response;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }
        #endregion

        #region MaterialesUbicaciones
        public async Task<ResponseModel> getMaterialesUbicaciones()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay ubicaciones de materiales para mostrar";
                response.respuesta = "[]";

                List<MaterialesUbicacionesRequest> listaR = new List<MaterialesUbicacionesRequest>();
                List<MaterialesUbicacione> lista = await _ctx.MaterialesUbicaciones.Include(x => x.IdMaterialNavigation).Include(y => y.IdUbicacionNavigation)
                                                                .Where(z => z.IdMaterialNavigation.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las ubicaciones de los materiales!!";
                    listaR = lista.ConvertAll(x => new MaterialesUbicacionesRequest()
                    {
                        IdMaterialUbicacion = x.IdMaterialUbicacion,
                        IdMaterial = x.IdMaterial,
                        IdUbicacion = x.IdUbicacion,
                        material = new MaterialRequest()
                        {
                            IdMaterial = x.IdMaterialNavigation.IdMaterial,
                            Nombre = x.IdMaterialNavigation.Nombre,
                            Descripcion = x.IdMaterialNavigation.Descripcion,
                            Precio = (double)x.IdMaterialNavigation.Precio,
                            TipoMedicion = x.IdMaterialNavigation.TipoMedicion,
                            Status = x.IdMaterialNavigation.Status,
                            Stock = (double)x.IdMaterialNavigation.Stock,
                            Visible = (bool)x.IdMaterialNavigation.Visible,
                            proveedores = null,
                            ubicaciones = null
                        },
                        ubicacion = new UbicacionRequest()
                        {
                            IdUbicacion = x.IdUbicacionNavigation.IdUbicacion,
                            Direccion = x.IdUbicacionNavigation.Direccion,
                            NombreEncargado = x.IdUbicacionNavigation.NombreEncargado,
                            ApellidoPEncargado = x.IdUbicacionNavigation.ApellidoPEncargado,
                            ApellidoMEncargado = x.IdUbicacionNavigation.ApellidoMEncargado,
                            Telefono1 = x.IdUbicacionNavigation.Telefono1,
                            Telefono2 = x.IdUbicacionNavigation.Telefono2,
                            Correo = x.IdUbicacionNavigation.Correo
                        }
                    });
                    response.respuesta = listaR;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<bool> postMaterialUbicacion(List<UbicacionRequest> ubicaciones, int idMaterial)
        {
            bool response = false;
            try
            {
                if (ubicaciones.Count() > 0)
                {
                    List<MaterialesUbicacione> listMaterialesUbi = new List<MaterialesUbicacione>();
                    ubicaciones.ForEach(dataUbicacion =>
                    {
                        listMaterialesUbi.Add(new MaterialesUbicacione()
                        {
                            IdMaterial = idMaterial,
                            IdUbicacion = dataUbicacion.IdUbicacion
                        });
                    });
                    _ctx.MaterialesUbicaciones.AddRange(listMaterialesUbi);
                    await _ctx.SaveChangesAsync();
                    response = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response = false;
            }
            return response;
        }

        public async Task<bool> putMaterialUbicacion(List<UbicacionRequest> ubicaciones, int idMaterial)
        {
            bool response = false;
            try
            {
                List<MaterialesUbicacione> listUbiExistentes = await _ctx.MaterialesUbicaciones.Where(x => x.IdMaterial == idMaterial).ToListAsync();
                if (listUbiExistentes.Count() > 0)
                {
                    List<MaterialesUbicacione> ubiAdd = new List<MaterialesUbicacione>();
                    List<MaterialesUbicacione> ubiDelete = new List<MaterialesUbicacione>();
                    //Si no existe en base y viene del request lo agrega
                    ubicaciones.ForEach(data =>
                    {
                        if (listUbiExistentes.Find(x => x.IdUbicacion == data.IdUbicacion) == null)
                        {
                            ubiAdd.Add(new MaterialesUbicacione()
                            {
                                IdMaterial = idMaterial,
                                IdUbicacion = data.IdUbicacion
                            });
                        }
                    });
                    if (ubiAdd.Count() > 0)
                    {
                        _ctx.MaterialesUbicaciones.AddRange(ubiAdd);
                        await _ctx.SaveChangesAsync();
                    }
                    //Si existe en base y no viene del request lo elimina
                    listUbiExistentes.ForEach(data =>
                    {
                        if (ubicaciones.Find(x => x.IdUbicacion == data.IdUbicacion) == null)
                        {
                            ubiDelete.Add(data);
                        }
                    });
                    if (ubiDelete.Count() > 0)
                    {
                        _ctx.MaterialesUbicaciones.RemoveRange(ubiDelete);
                        await _ctx.SaveChangesAsync();
                    }
                    response = true;
                }
                else
                {
                    //Inserta las ubicaciones del material si es que tiene
                    var insertaUbicaciones = await postMaterialUbicacion(ubicaciones, idMaterial);
                    response = insertaUbicaciones;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response = false;
            }
            return response;
        }
        #endregion

        #region PagosApartados
        public async Task<ResponseModel> getPagos()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay pagos para mostrar";
                response.respuesta = "[]";

                List<PagoApartado> lista = await _ctx.PagoApartados.ToListAsync();
                //List<Apartados> lista = await _ctx.Apartados.ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los pagos!!";
                    response.respuesta = lista;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getPagosByApartado(int IdApartado)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay pagos pendientes";
                response.respuesta = null;
                List<PagoApartadoRequest> pagos = new List<PagoApartadoRequest>();
                
                pagos = _ctx.PagoApartados.Where(x => x.IdApartado == IdApartado).ToList()

                .ConvertAll(u => new PagoApartadoRequest()
                    {
                        IdPagoApartado = u.IdPagoApartado,
                        IdApartado = u.IdApartado,
                        Fecha = (DateTime?)u.Fecha,
                        Cantidad = u.Cantidad,
                        Status = u.Status,
                        IdArticulo = u.IdArticulo,
                        


                    });
                if (pagos != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los apartados!!";
                    response.respuesta = pagos;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = null;
            }
            return response;
        }

        public async Task<ResponseModel> postPagoApartado(PagoApartadoRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo hacer el pago";
                response.respuesta = "[]";

                PagoApartado newPago = new PagoApartado();
                newPago.IdApartado = request.IdApartado;
                newPago.Fecha = request.Fecha;
                newPago.Cantidad = request.Cantidad;

                /*newApartado.idArticulo = request.idArticulo;
                newApartado.IdEmpleado = request.IdEmpleado;
                newApartado.Telefono = request.Telefono;
                newApartado.IdTalla = request.IdTalla;
                newApartado.Fecha = (DateTime)request.Fecha;
                newApartado.Direccion = request.Direccion;
                newApartado.Status = "Espera";*/
                _ctx.PagoApartados.Add(newPago);
                //_ctx.Apartados.Add(newApartado);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se insertó el cliente correctamente!!";
                response.respuesta = newPago;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }
        #endregion

        #region Productos
        public async Task<ResponseModel> getProductos(string request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay articulos para mostrar";
                response.respuesta = "[]";
                List<ProductoRequest> lista = new List<ProductoRequest>();
                if (request == "all")
                {
                    lista = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b=>b.IdCategoriaNavigation).Include(c=>c.IdUbicacionNavigation)
                                                   .Where(x => x.IdArticulo!=null).ToList()
                                                   .ConvertAll(u => new ProductoRequest()
                                                   {
                                                       IdArticulo=u.IdArticulo,
                                                       Unidad=u.Unidad,
                                                       Existencia=u.Existencia,
                                                       Descripcion=u.Descripcion,
                                                       FechaIngreso=(DateTime)u.FechaIngreso,
                                                       idTalla=(int)u.IdTalla,
                                                       idCategoria=(int)u.IdCategoria,
                                                       idUbicacion=(int)u.IdUbicacion,
                                                       imagen=u.Imagen,
                                                       talla = u.IdTallaNavigation.Nombre,
                                                       ubicacion= u.IdUbicacionNavigation.Direccion,
                                                       categoria = u.IdCategoriaNavigation.Descripcion,
                                                       precio = (int)u.Precio,
                                                       sku = u.Sku
                                                   });
                }
                else
                {
                    lista = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                   .Where(x => x.IdArticulo != null && x.IdUbicacionNavigation.Direccion == request).ToList()
                                                   .ConvertAll(u => new ProductoRequest()
                                                   {
                                                       IdArticulo = u.IdArticulo,
                                                       Unidad = u.Unidad,
                                                       Existencia = u.Existencia,
                                                       Descripcion = u.Descripcion,
                                                       FechaIngreso = (DateTime)u.FechaIngreso,
                                                       idTalla = (int)u.IdTalla,
                                                       idCategoria = (int)u.IdCategoria,
                                                       idUbicacion = (int)u.IdUbicacion,
                                                       imagen = u.Imagen,
                                                       talla = u.IdTallaNavigation.Nombre,
                                                       ubicacion = u.IdUbicacionNavigation.Direccion,
                                                       categoria = u.IdCategoriaNavigation.Descripcion,
                                                       precio = (int)u.Precio,
                                                       sku = u.Sku
                                                   });
                }
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los proveedores!!";
                    response.respuesta = lista;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }


        public async Task<ResponseModel> getInexistencias(string request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay articulos para mostrar";
                response.respuesta = "[]";
                List<ProductoRequest> lista = new List<ProductoRequest>();
                if (request == "all")
                {
                    lista = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                   .Where(x => x.IdArticulo != null && x.Existencia == "0").ToList()
                                                   .ConvertAll(u => new ProductoRequest()
                                                   {
                                                       IdArticulo = u.IdArticulo,
                                                       Unidad = u.Unidad,
                                                       Existencia = u.Existencia,
                                                       Descripcion = u.Descripcion,
                                                       FechaIngreso = (DateTime)u.FechaIngreso,
                                                       idTalla = (int)u.IdTalla,
                                                       idCategoria = (int)u.IdCategoria,
                                                       idUbicacion = (int)u.IdUbicacion,
                                                       imagen = u.Imagen,
                                                       talla = u.IdTallaNavigation.Nombre,
                                                       ubicacion = u.IdUbicacionNavigation.Direccion,
                                                       categoria = u.IdCategoriaNavigation.Descripcion,
                                                       precio = (int)u.Precio,
                                                       sku = u.Sku
                                                   });
                }
                else
                {
                    lista = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                   .Where(x => x.IdArticulo != null && x.IdUbicacionNavigation.Direccion == request).ToList()
                                                   .ConvertAll(u => new ProductoRequest()
                                                   {
                                                       IdArticulo = u.IdArticulo,
                                                       Unidad = u.Unidad,
                                                       Existencia = u.Existencia,
                                                       Descripcion = u.Descripcion,
                                                       FechaIngreso = (DateTime)u.FechaIngreso,
                                                       idTalla = (int)u.IdTalla,
                                                       idCategoria = (int)u.IdCategoria,
                                                       idUbicacion = (int)u.IdUbicacion,
                                                       imagen = u.Imagen,
                                                       talla = u.IdTallaNavigation.Nombre,
                                                       ubicacion = u.IdUbicacionNavigation.Direccion,
                                                       categoria = u.IdCategoriaNavigation.Descripcion,
                                                       precio = (int)u.Precio,
                                                       sku = u.Sku
                                                   });
                }
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los proveedores!!";
                    response.respuesta = lista;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }


        public async Task<ResponseModel> postProductos(ProductoRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el nuevo articulo";
                response.respuesta = "[]";


                Articulo existeArticulo = _ctx.Articulos.FirstOrDefault(x => x.IdArticulo == request.IdArticulo);
                if (existeArticulo != null)
                {
                    existeArticulo.Unidad = request.Unidad;
                    existeArticulo.Descripcion = request.Descripcion;
                    existeArticulo.FechaIngreso = request.FechaIngreso;
                    existeArticulo.Existencia = request.Existencia;
                    existeArticulo.IdUbicacion = request.idUbicacion;
                    existeArticulo.IdCategoria = request.idCategoria;
                    existeArticulo.IdTalla = request.idTalla;
                    existeArticulo.Sku = request.sku;
                    existeArticulo.Precio = request.precio;
                    existeArticulo.Imagen = request.imagen;


                    _ctx.Articulos.Update(existeArticulo);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el articulo correctamente!!";
                    response.respuesta = existeArticulo;
                }
                else {
                    Articulo newArticulo = new Articulo();
                    newArticulo.Unidad = request.Unidad;
                    newArticulo.Descripcion = request.Descripcion;
                    newArticulo.FechaIngreso = request.FechaIngreso;
                    newArticulo.Existencia = request.Existencia;
                    newArticulo.IdUbicacion = request.idUbicacion;
                    newArticulo.IdCategoria = request.idCategoria;
                    newArticulo.IdTalla = request.idTalla;
                    newArticulo.Imagen = request.imagen;
                    newArticulo.Precio = request.precio;
                    newArticulo.Sku = request.sku;

                    _ctx.Articulos.Add(newArticulo);
                    await _ctx.SaveChangesAsync();
                    response.exito = true;
                    response.mensaje = "Se agrego el articulo correctamente!!";
                    response.respuesta = newArticulo;

                }
               
                
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
                    existeArticulo.Sku = request.sku;
                    existeArticulo.Precio = request.precio;
                    existeArticulo.Imagen = request.imagen;


                    _ctx.Articulos.Update(existeArticulo);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó el articulo correctamente!!";
                    response.respuesta = existeArticulo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> searchProduct(string queryString, string sucursal)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay productos relacionados con la busqueda";
                response.respuesta = "[]";

                List<ProductoRequest> allResults = new List<ProductoRequest>();
                List<Articulo> resultsByName = new List<Articulo>();
                List<Articulo> resultsBySku = new List<Articulo>();

                if (sucursal == "all")
                {
                    resultsByName = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                  .Where(x => x.Descripcion.ToLower().Contains(queryString.ToLower())).ToList();

                    resultsBySku = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                 .Where(x => x.Sku.ToLower().Contains(queryString.ToLower())).ToList();
                }
                else
                {
                    resultsByName = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                .Where(x => x.Descripcion.ToLower().Contains(queryString.ToLower()) && x.IdUbicacionNavigation.Direccion == sucursal).ToList();

                    resultsBySku = _ctx.Articulos.Include(a => a.IdTallaNavigation).Include(b => b.IdCategoriaNavigation).Include(c => c.IdUbicacionNavigation)
                                                .Where(x => x.Sku.ToLower().Contains(queryString.ToLower()) && x.IdUbicacionNavigation.Direccion == sucursal).ToList();
                }

                if (resultsByName.Count() > 0 || resultsBySku.Count() > 0)
                {
                    resultsBySku.ForEach(product =>
                    {
                        allResults.Add(new ProductoRequest()
                        {
                            IdArticulo = product.IdArticulo,
                            Unidad = product.Unidad,
                            Existencia = product.Existencia,
                            Descripcion = product.Descripcion,
                            FechaIngreso = (DateTime)product.FechaIngreso,
                            idTalla = (int)product.IdTalla,
                            idCategoria = (int)product.IdCategoria,
                            idUbicacion = (int)product.IdUbicacion,
                            imagen = product.Imagen,
                            talla = product.IdTallaNavigation.Nombre,
                            ubicacion = product.IdUbicacionNavigation.Direccion,
                            categoria = product.IdCategoriaNavigation.Descripcion,
                            precio = (int)product.Precio,
                            sku = product.Sku
                        });
                    });
                    resultsByName.ForEach(product =>
                    {
                        if (allResults.Find(x => x.IdArticulo == product.IdArticulo) == null)
                        {
                            allResults.Add(new ProductoRequest()
                            {
                                IdArticulo = product.IdArticulo,
                                Unidad = product.Unidad,
                                Existencia = product.Existencia,
                                Descripcion = product.Descripcion,
                                FechaIngreso = (DateTime)product.FechaIngreso,
                                idTalla = (int)product.IdTalla,
                                idCategoria = (int)product.IdCategoria,
                                idUbicacion = (int)product.IdUbicacion,
                                imagen = product.Imagen,
                                talla = product.IdTallaNavigation.Nombre,
                                ubicacion = product.IdUbicacionNavigation.Direccion,
                                categoria = product.IdCategoriaNavigation.Descripcion,
                                precio = (int)product.Precio,
                                sku = product.Sku
                            });
                        }
                    });
                    response.exito = true;
                    response.mensaje = "Se obtuvieron las coincidencias relacionadas con el filtro!!";
                    response.respuesta = allResults;
                }
            }
            catch (Exception e)
            {
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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

                List<ProveedoresMaterialesRequest> listaR = new List<ProveedoresMaterialesRequest>();
                List<ProveedoresMateriale> lista = await _ctx.ProveedoresMateriales.Include(x => x.IdMaterialNavigation).Include(y => y.IdProveedorNavigation)
                                                                .Where(z => z.IdMaterialNavigation.Visible == true && z.IdProveedorNavigation.Visible == true).ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente los proveedores y materiales!!";
                    listaR = lista.ConvertAll(x => new ProveedoresMaterialesRequest()
                    {
                        IdProveedorMaterial = x.IdProveedorMaterial,
                        IdMaterial = x.IdMaterial,
                        IdProveedor = x.IdProveedor,
                        material = new MaterialRequest()
                        {
                            IdMaterial = x.IdMaterialNavigation.IdMaterial,
                            Nombre = x.IdMaterialNavigation.Nombre,
                            Descripcion = x.IdMaterialNavigation.Descripcion,
                            Precio = (double)x.IdMaterialNavigation.Precio,
                            TipoMedicion = x.IdMaterialNavigation.TipoMedicion,
                            Status = x.IdMaterialNavigation.Status,
                            Stock = (double)x.IdMaterialNavigation.Stock,
                            Visible = (bool)x.IdMaterialNavigation.Visible,
                            proveedores = null,
                        },
                        proveedor = new ProveedorRequest()
                        {
                            IdProveedor = x.IdProveedorNavigation.IdProveedor,
                            Nombre = x.IdProveedorNavigation.Nombre,
                            ApellidoPaterno = x.IdProveedorNavigation.ApellidoPaterno,
                            ApellidoMaterno = x.IdProveedorNavigation.ApellidoMaterno,
                            Telefono1 = x.IdProveedorNavigation.Telefono1,
                            Telefono2 = x.IdProveedorNavigation.Telefono2,
                            Correo = x.IdProveedorNavigation.Correo,
                            Direccion = x.IdProveedorNavigation.Direccion,
                            EncargadoNombre = x.IdProveedorNavigation.EncargadoNombre,
                        }
                    });
                    response.respuesta = listaR;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<bool> postProveedorMaterial(List<ProveedorRequest> proveedores, int idMaterial)
        {
            bool response = false;
            try
            {
                if (proveedores.Count() > 0)
                {
                    List<ProveedoresMateriale> listProveedoresMateriales = new List<ProveedoresMateriale>();
                    proveedores.ForEach(dataProveedor =>
                    {
                        listProveedoresMateriales.Add(new ProveedoresMateriale()
                        {
                            IdMaterial = idMaterial,
                            IdProveedor = dataProveedor.IdProveedor
                        });
                    });
                    _ctx.ProveedoresMateriales.AddRange(listProveedoresMateriales);
                    await _ctx.SaveChangesAsync();
                    response = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response = false;
            }
            return response;
        }

        public async Task<bool> putProveedorMaterial(List<ProveedorRequest> proveedores, int idMaterial)
        {
            bool response = false;
            try
            {
                List<ProveedoresMateriale> listProvExistentes = await _ctx.ProveedoresMateriales.Where(x => x.IdMaterial == idMaterial).ToListAsync();
                if (listProvExistentes.Count() > 0)
                {
                    List<ProveedoresMateriale> provAdd = new List<ProveedoresMateriale>();
                    List<ProveedoresMateriale> provDelete = new List<ProveedoresMateriale>();
                    //Si no existe en base y viene del request lo agrega
                    proveedores.ForEach(data =>
                    {
                        if (listProvExistentes.Find(x => x.IdProveedor == data.IdProveedor) == null)
                        {
                            provAdd.Add(new ProveedoresMateriale()
                            {
                                IdMaterial = idMaterial,
                                IdProveedor = data.IdProveedor
                            });
                        }
                    });
                    if (provAdd.Count() > 0)
                    {
                        _ctx.ProveedoresMateriales.AddRange(provAdd);
                        await _ctx.SaveChangesAsync();
                    }
                    //Si existe en base y no viene del request lo elimina
                    listProvExistentes.ForEach(data =>
                    {
                        if (proveedores.Find(x => x.IdProveedor == data.IdProveedor) == null)
                        {
                            provDelete.Add(data);
                        }
                    });
                    if (provDelete.Count() > 0)
                    {
                        _ctx.ProveedoresMateriales.RemoveRange(provDelete);
                        await _ctx.SaveChangesAsync();
                    }
                    response = true;
                }
                else
                {
                    //Inserta los proveedores del material si es que tiene
                    var insertaProveedores = await postProveedorMaterial(proveedores, idMaterial);

                    response = insertaProveedores;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response = false;
            }
            return response;
        }

        public async Task<ResponseModel> searchCliente(string queryString)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay Clientes relacionados con la busqueda";
                response.respuesta = "[]";

                List<ProveedorRequest> allResults = new List<ProveedorRequest>();
                List<CatProveedore> resultsByName = _ctx.CatProveedores
                    .Where(x => x.Nombre.ToLower().Contains(queryString.ToLower())).ToList();
            

                if (resultsByName.Count() > 0)
                {
                
                    resultsByName.ForEach(product =>
                    {
                        if (allResults.Find(x => x.IdProveedor == product.IdProveedor) == null)
                        {
                            allResults.Add(new ProveedorRequest()
                            {
                                IdProveedor = product.IdProveedor,
                                Nombre = product.Nombre,
                                ApellidoPaterno = product.ApellidoPaterno,
                                ApellidoMaterno = product.ApellidoMaterno,
                                Telefono1 = product.Telefono1,
                                Telefono2 = product.Telefono2,
                                Correo = product.Correo,
                                Direccion = product.Direccion,
                                EncargadoNombre = product.EncargadoNombre
                            });
                        }
                    });
                    response.exito = true;
                    response.mensaje = "Se obtuvieron las coincidencias relacionadas con el filtro!!";
                    response.respuesta = allResults;
                }
            }
            catch (Exception e)
            {
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }
        #endregion

        #region SolicitudesMateriales
        public async Task<ResponseModel> getSolicitudesMateriales()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No hay solicitudes de materiales para mostrar";
                response.respuesta = "[]";
                List<SolicitudesMaterialesRequest> listaR = new List<SolicitudesMaterialesRequest>();
                List<SolicitudesMateriale> lista = await _ctx.SolicitudesMateriales.Include(x => x.IdProveedorMaterialNavigation)
                                                             .ThenInclude(a => a.IdMaterialNavigation)
                                                             .Include(b => b.IdProveedorMaterialNavigation)
                                                             .ThenInclude(c => c.IdProveedorNavigation)
                                                             .Include(y => y.IdUserNavigation).ThenInclude(z => z.IdRolNavigation)
                                                             .ToListAsync();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las solicitudes de materiales!!";
                    listaR = lista.ConvertAll(x => new SolicitudesMaterialesRequest()
                    {
                        IdSolicitud = x.IdSolicitud,
                        Fecha = x.Fecha,
                        Cantidad = x.Cantidad,
                        Comentarios = x.Comentarios,
                        IdProveedorMaterial = x.IdProveedorMaterial,
                        proveedorMaterial = new ProveedoresMaterialesRequest()
                        {
                            IdProveedorMaterial = x.IdProveedorMaterialNavigation.IdProveedorMaterial,
                            IdProveedor = x.IdProveedorMaterialNavigation.IdProveedor,
                            IdMaterial = x.IdProveedorMaterialNavigation.IdMaterial,
                            material = new MaterialRequest()
                            {
                                IdMaterial = x.IdProveedorMaterialNavigation.IdMaterialNavigation.IdMaterial,
                                Nombre = x.IdProveedorMaterialNavigation.IdMaterialNavigation.Nombre,
                                Descripcion = x.IdProveedorMaterialNavigation.IdMaterialNavigation.Descripcion,
                                Precio = (double)x.IdProveedorMaterialNavigation.IdMaterialNavigation.Precio,
                                TipoMedicion = x.IdProveedorMaterialNavigation.IdMaterialNavigation.TipoMedicion,
                                Status = x.IdProveedorMaterialNavigation.IdMaterialNavigation.Status,
                                Stock = (double)x.IdProveedorMaterialNavigation.IdMaterialNavigation.Stock,
                                Visible = (bool)x.IdProveedorMaterialNavigation.IdMaterialNavigation.Visible
                            },
                            proveedor = new ProveedorRequest()
                            {
                                IdProveedor = x.IdProveedorMaterialNavigation.IdProveedorNavigation.IdProveedor,
                                Nombre = x.IdProveedorMaterialNavigation.IdProveedorNavigation.Nombre,
                                ApellidoPaterno = x.IdProveedorMaterialNavigation.IdProveedorNavigation.ApellidoPaterno,
                                ApellidoMaterno = x.IdProveedorMaterialNavigation.IdProveedorNavigation.ApellidoMaterno,
                                Telefono1 = x.IdProveedorMaterialNavigation.IdProveedorNavigation.Telefono1,
                                Telefono2 = x.IdProveedorMaterialNavigation.IdProveedorNavigation.Telefono2,
                                Correo = x.IdProveedorMaterialNavigation.IdProveedorNavigation.Correo,
                                Direccion = x.IdProveedorMaterialNavigation.IdProveedorNavigation.Direccion,
                                EncargadoNombre = x.IdProveedorMaterialNavigation.IdProveedorNavigation.EncargadoNombre,
                            }
                        },
                        Status = (string) x.Status,
                        FechaUpdate = x.FechaUpdate,
                        CostoTotal = x.CostoTotal,
                        IdUser = x.IdUser,
                        usuario = new UsuarioRequest()
                        {
                            IdUser = x.IdUserNavigation.IdUser,
                            Usuario = x.IdUserNavigation.Usuario,
                            Password = x.IdUserNavigation.Password,
                            IdRol = (int)x.IdUserNavigation.IdRol,
                            Rol = (string)x.IdUserNavigation.IdRolNavigation.Descripcion,
                            Nombre = x.IdUserNavigation.Nombre,
                            ApellidoPaterno = x.IdUserNavigation.ApellidoPaterno,
                            ApellidoMaterno = x.IdUserNavigation.ApellidoMaterno,
                            Telefono = x.IdUserNavigation.Telefono,
                            Correo = x.IdUserNavigation.Correo,
                        }
                    });
                    response.respuesta = listaR;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> postSolicitudMaterial(SolicitudesMaterialesRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar la nueva solicitud de material";
                response.respuesta = "[]";

                SolicitudesMateriale newSolicitudMaterial = new SolicitudesMateriale();

                newSolicitudMaterial.Fecha = DateTime.Now;
                newSolicitudMaterial.Cantidad = request.Cantidad;
                newSolicitudMaterial.Comentarios = request.Comentarios;
                newSolicitudMaterial.IdProveedorMaterial = request.IdProveedorMaterial;
                newSolicitudMaterial.Status = request.Status;
                //newSolicitudMaterial.FechaUpdate = request.FechaUpdate;
                newSolicitudMaterial.CostoTotal = request.CostoTotal;
                newSolicitudMaterial.IdUser = request.IdUser;

                _ctx.SolicitudesMateriales.Add(newSolicitudMaterial);
                await _ctx.SaveChangesAsync();

                response.exito = true;
                response.mensaje = "Se realizó la solicitud de material correctamente!!";
                response.respuesta = newSolicitudMaterial;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> putSolicitudMaterial(SolicitudesMaterialesRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar la solicitud de material";
                response.respuesta = "[]";

                SolicitudesMateriale existeSolMaterial = _ctx.SolicitudesMateriales.FirstOrDefault(x => x.IdSolicitud == request.IdSolicitud);
                if (existeSolMaterial != null)
                {
                    existeSolMaterial.Cantidad = request.Cantidad;
                    existeSolMaterial.Comentarios = request.Comentarios;
                    existeSolMaterial.IdProveedorMaterial = request.IdProveedorMaterial;
                    existeSolMaterial.Status = request.Status;
                    existeSolMaterial.FechaUpdate = DateTime.Now;
                    existeSolMaterial.CostoTotal = request.CostoTotal;
                    existeSolMaterial.IdUser = request.IdUser;

                    _ctx.SolicitudesMateriales.Update(existeSolMaterial);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó la solicitud de material correctamente!!";
                    response.respuesta = existeSolMaterial;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> deleteSolicitudMaterial(SolicitudesMaterialesRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo eliminar la solicitud de material";
                response.respuesta = "[]";

                SolicitudesMateriale existeSolMaterial = _ctx.SolicitudesMateriales.FirstOrDefault(x => x.IdSolicitud == request.IdSolicitud);

                if (existeSolMaterial != null)
                {
                    
                    _ctx.SolicitudesMateriales.Remove(existeSolMaterial);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se eliminó la solicitud de material correctamente!!";
                    response.respuesta = "[]";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> postUsuario(UsuarioRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo insertar el usuario";
                response.respuesta = "[]";

                User existUser = await _ctx.Users.Where(x => x.Usuario == request.Usuario || x.Correo == request.Correo).FirstOrDefaultAsync();

                if (existUser == null)
                {
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
                }
                else
                {
                    response.mensaje = "Ya existe un usuario con el usuario y correo registrado";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> updatePassword(int idUser, string newPassword)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo actualizar la contraseña";
                response.respuesta = "[]";

                User existeUser = _ctx.Users.FirstOrDefault(x => x.IdUser == idUser);

                if (existeUser != null)
                {
                    existeUser.Password = newPassword;
                    _ctx.Users.Update(existeUser);
                    await _ctx.SaveChangesAsync();

                    response.exito = true;
                    response.mensaje = "Se actualizó la contraseña correctamente!!";
                    response.respuesta = "[]";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
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
        private DateTime setFormatDate(string fecha)
        {
            DateTime fechaR;
            var fechaComponents = fecha.Split(" ");
            var date = fechaComponents[0].Split("/");
            var hours = fechaComponents[1].Split(":");

            var hora = 0;
            //PM
            if (fecha.Contains("PM"))
            {
                if (Int32.Parse(hours[0]) != 12)
                {
                    hora = Int32.Parse(hours[0]) + 12;
                }
                else
                {
                    hora = Int32.Parse(hours[0]);
                }
            }
            //AM
            else
            {
                if (Int32.Parse(hours[0]) != 12)
                {
                    hora = Int32.Parse(hours[0]);
                }
                else
                {
                    hora = Int32.Parse(hours[0]) - 12;
                }
            }

            fechaR = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]), hora, Int32.Parse(hours[1]), Int32.Parse(hours[2]));

            return fechaR;
        }
        #endregion

        #region Ventas
        public async Task<ResponseModel> postAddVentas(VentaRequest request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No se pudo registrar la venta";
                response.respuesta = "[]";
                    Venta newVenta = new Venta();

                    using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            //newVenta.IdVenta = request.IdVenta;
                            newVenta.IdCaja = request.IdCaja;
                            newVenta.Fecha = DateTime.Parse(request.Fecha);
                            newVenta.NoTicket = request.NoTicket;
                            newVenta.TipoPago = request.TipoPago;
                            newVenta.TipoVenta = request.TipoVenta;
                            newVenta.NoArticulos = request.NoArticulos;
                            newVenta.Subtotal = request.Subtotal;
                            newVenta.Total = request.Total;

                            _ctx.Add(newVenta);
                         await _ctx.SaveChangesAsync();
                        int idVenta = newVenta.IdVenta; // recuperar

                        //Actualiza  el stock de los productos del inventario
                        if (request.ventaArticulo?.Count() > 0)
                            {
                                List<VentaArticulo> listventasArticulos = new List<VentaArticulo>();

                                request.ventaArticulo.ForEach(dataArticulo =>
                                {
                                    

                                    listventasArticulos.Add(new VentaArticulo()
                                    {
                                       // IdVentaArticulo = 1,
                                        IdVenta = idVenta,
                                        IdArticulo = dataArticulo.IdArticulo,
                                        Cantidad = dataArticulo.Cantidad,
                                        PrecioUnitario = dataArticulo.PrecioUnitario,
                                        Subtotal = dataArticulo.Subtotal,
                                       // Articulo  = dataArticulo.Articulo
                                      

                                    });

                                  
                                    //Actualiza el stock
                                    Articulo articuloVenta = _ctx.Articulos.FirstOrDefault(x => x.IdArticulo == dataArticulo.IdArticulo);
                                   
                                 
                                    if ((Int32.Parse(articuloVenta.Existencia) - dataArticulo.Cantidad) >= 0)
                                    {
                                       articuloVenta.Existencia = (Int32.Parse(articuloVenta.Existencia) - dataArticulo.Cantidad).ToString();
                                    }
                                    else
                                    {
                                        response.exito = false;
                                        response.mensaje = "Ya no hay stock del articulo !";
                                        response.respuesta = "[]";
                                       dbContextTransaction.Rollback();
                                    }

                                    _ctx.Articulos.Update(articuloVenta);

                                 
                                });
                            if (listventasArticulos.Count() > 0) {
                                _ctx.VentaArticulos.AddRange(listventasArticulos);
                                await _ctx.SaveChangesAsync();
                            }

                                

                            }


                            //Hacemos commit de todos los datos
                            dbContextTransaction.Commit();
                            response.exito = true;
                            response.mensaje = "Se ha registrado la venta!";
                            response.respuesta = "[]";

                        }
                        catch (Exception ex)
                        {
                        Console.WriteLine(ex.InnerException.Message);
                        response.exito = false;
                            response.mensaje = ex.InnerException.Message;
                            response.respuesta = "[]";
                            dbContextTransaction.Rollback();
                            return response;
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getVentas()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No ventas para mostrar";
                response.respuesta = "[]";

                List<VentaRequest> listaVentas = new List<VentaRequest>();
                List<Venta> lista = _ctx.Ventas.ToList();
                List < VentaArticulo> articulosVendidos= new List<VentaArticulo>();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las Ventas!!";
                    listaVentas = lista.ConvertAll(x => new VentaRequest()
                    {
                        IdVenta = x.IdVenta,
                        IdCaja = x.IdCaja,
                        Fecha = x.Fecha.ToString(),
                        NoTicket = x.NoTicket,
                        TipoPago = x.TipoPago,
                        TipoVenta = x.TipoVenta,
                        NoArticulos = x.NoArticulos,
                        Subtotal = x.Subtotal,
                        Total = x.Total,
                          
                    }
                  
                    );
                    response.respuesta = listaVentas;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        public async Task<ResponseModel> getVentasByDates(DateTime dateI,DateTime dateF)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response.exito = false;
                response.mensaje = "No ventas para mostrar";
                response.respuesta = "[]";

                List<VentaRequest> listaVentas = new List<VentaRequest>();
                List<Venta> lista = _ctx.Ventas.Where(x => x.Fecha >= dateI && x.Fecha <= dateF).ToList();
                List<VentaArticulo> articulosVendidos = new List<VentaArticulo>();
                if (lista != null)
                {
                    response.exito = true;
                    response.mensaje = "Se han consultado exitosamente las Ventas!!";
                    listaVentas = lista.ConvertAll(x => new VentaRequest()
                    {
                        IdVenta = x.IdVenta,
                        IdCaja = x.IdCaja,
                        Fecha = x.Fecha.ToString(),
                        NoTicket = x.NoTicket,
                        TipoPago = x.TipoPago,
                        TipoVenta = x.TipoVenta,
                        NoArticulos = x.NoArticulos,
                        Subtotal = x.Subtotal,
                        Total = x.Total,

                    }

                    );
                    response.respuesta = listaVentas;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.mensaje = e.Message;
                response.exito = false;
                response.respuesta = "[]";
            }
            return response;
        }

        #endregion
    }
}
