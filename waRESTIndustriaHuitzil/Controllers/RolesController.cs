using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public RolesController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("ConsultaAll")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _service.getRoles());
        }
        #endregion

        #region POST
        [HttpPost("Agrega")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregaRol([FromBody] RolRequest request)
        {
            return Ok(await _service.postRol(request));
        }
        #endregion

        #region PUT
        [HttpPost("Actualiza")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaRol([FromBody] RolRequest request)
        {
            return Ok(await _service.putRol(request));
        }
        #endregion

        #region DELETE
        [HttpPost("Elimina")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EliminaRol([FromBody] RolRequest request)
        {
            return Ok(await _service.deleteRol(request));
        }
        #endregion
    }
}
