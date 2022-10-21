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

        [HttpPut("Returns/Actualiza")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaCambioDevolucion([FromBody] CambiosDevolucionesRequest request)
        {
            return Ok(await _service.putCambiosyDevoluciones(request));
        }
        #endregion

    }
}
