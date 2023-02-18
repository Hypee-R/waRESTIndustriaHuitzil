using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public ClientesController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("Consulta")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetClientes()
        {
            return Ok(await _service.getClientes());
        }
        #endregion

        #region POST
        [HttpPost("Agrega")]
        public async Task<IActionResult> AgregaCliente([FromBody] ClienteRequest request)
        {
            return Ok(await _service.postCliente(request));
        }
        #endregion

        #region PUT
        [HttpPut("Actualiza")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaCliente([FromBody] ClienteRequest request)
        {
            return Ok(await _service.putCliente(request));
        }
        #endregion

        #region DELETE
        [HttpDelete("Elimina")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EliminaCliente([FromBody] ClienteRequest request)
        {
            return Ok(await _service.deleteCliente(request));
        }
        #endregion
    }
}
