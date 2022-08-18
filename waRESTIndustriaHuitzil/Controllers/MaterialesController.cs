using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public MaterialesController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("Consulta")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMateriales()
        {
            return Ok(await _service.getMateriales());
        }
        #endregion

        #region POST
        [HttpPost("Agrega")]
        public async Task<IActionResult> AgregaMaterial([FromBody] MaterialRequest request)
        {
            return Ok(await _service.postMaterial(request));
        }
        #endregion

        #region PUT
        [HttpPut("Actualiza")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaMaterial([FromBody] MaterialRequest request)
        {
            return Ok(await _service.putMaterial(request));
        }
        #endregion

        #region DELETE
        [HttpDelete("Elimina")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EliminaMaterial([FromBody] MaterialRequest request)
        {
            return Ok(await _service.deleteMaterial(request));
        }
        #endregion
    }
}
