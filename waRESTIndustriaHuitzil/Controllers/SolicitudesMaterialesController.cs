using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SolicitudesMaterialesController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public SolicitudesMaterialesController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("Consulta")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetSolicitudesMateriales()
        {
            return Ok(await _service.getSolicitudesMateriales());
        }

        [HttpGet("ConsultaProvMat")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProveedoresMateriales()
        {
            return Ok(await _service.getProveedoresMateriales());
        }
        #endregion

        #region POST
        [HttpPost("Agrega")]
        public async Task<IActionResult> AgregaSolicitud([FromBody] SolicitudesMaterialesRequest request)
        {
            return Ok(await _service.postSolicitudMaterial(request));
        }
        #endregion

        #region PUT
        [HttpPut("Actualiza")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaSolicitud([FromBody] SolicitudesMaterialesRequest request)
        {
            return Ok(await _service.putSolicitudMaterial(request));
        }
        #endregion

        #region DELETE
        [HttpDelete("Elimina")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EliminaSolicitud([FromBody] SolicitudesMaterialesRequest request)
        {
            return Ok(await _service.deleteSolicitudMaterial(request));
        }
        #endregion
    }
}
