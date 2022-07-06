using CoreIndustriaHuitzil.Models;
using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UbicacionesController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public UbicacionesController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("ConsultaAll")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUbicaciones()
        {
            return Ok(await _service.getUbicaciones());
        }

        //[HttpGet("ConsultaAllVR")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetVistasRoles(int idRol)
        //{
        //    return Ok(await _service.getVistasRol(idRol));
        //}
        #endregion

        #region POST
        [HttpPost("AgregaUbicacion")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregarUbicacion([FromBody] UbicacionRequest request)
        {
            return Ok(await _service.postUbicacion(request));
        }
        #endregion

        #region PUT
        [HttpPut("ActualizaUbicacion")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaUbicacion([FromBody] UbicacionRequest request)
        {
            return Ok(await _service.putUbicacion(request));
        }
        #endregion

        #region DELETE
        [HttpDelete("EliminaUbicacion")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EliminaUbicacion([FromBody] UbicacionRequest request)
        {
            return Ok(await _service.deleteUbicacion(request));
        }
        #endregion
    }
}
