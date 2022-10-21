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
        [HttpGet("GetCash")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCaja(int param)
        {
            return Ok(await _service.getCaja(param));
        }

        [HttpPost("OpenCash")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AbrirCaja([FromBody] CajaRequest request)
        {
            return Ok(await _service.openCaja(request));
        }

        [HttpPut("CloseCash")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CerrarCaja([FromBody] CajaRequest request)
        {
            return Ok(await _service.closeCaja(request));
        }
        #endregion
    }
}
