using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public VentasController(IIndustriaHuitzilService service) => _service = service;

        #region Caja
        [HttpGet("Cash/Cajas")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCaja()
        {
            return Ok(await _service.getCajaDate());
        }

        [HttpGet("Cash/CajasDate")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCaja(DateTime dateI, DateTime dateF)
        {
            return Ok(await _service.getCajaDate(dateI,dateF));
        }
       
    [HttpGet("Cash/Consulta")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCaja(int param)
        {
            return Ok(await _service.getCaja(param));
        }

        [HttpPost("Cash/Abrir")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AbrirCaja([FromBody] CajaRequest request)
        {
            return Ok(await _service.openCaja(request));
        }

        [HttpPut("Cash/Cerrar")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CerrarCaja([FromBody] CajaRequest request)
        {
            return Ok(await _service.closeCaja(request));
        }
        #endregion

        #region Cambios y Devoluciones
        [HttpGet("Returns/SearchSale")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SearchVenta(string noTicket)
        {
            return Ok(await _service.searchVentaByNoTicket(noTicket));
        }

        [HttpGet("Returns/Consulta")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCambioDevolucion()
        {
            return Ok(await _service.getCambiosyDevoluciones());
        }

        [HttpPost("Returns/Agrega")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregaCambioDevolucion([FromBody] CambiosDevolucionesRequest request)
        {
            return Ok(await _service.postCambiosyDevoluciones(request));
        }
        #endregion

        #region Ventas 
        [HttpPost("Sales/Agrega")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregaVentas([FromBody] VentaRequest request)
        {
            return Ok(await _service.postAddVentas(request));
        }

        [HttpGet("Sales")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetVentas()
        {
            return Ok(await _service.getVentas());
        }

        [HttpGet("SalesByDates")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetVentasByDates(DateTime dateI,DateTime dateF)
        {
            return Ok(await _service.getVentasByDates(dateI,dateF));
        }


        [HttpGet("SalesByCajas")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetVentasByCaja(int idCaja)
        {
            return Ok(await _service.getVentasByCaja(idCaja));
        }

        #endregion

    }
}
