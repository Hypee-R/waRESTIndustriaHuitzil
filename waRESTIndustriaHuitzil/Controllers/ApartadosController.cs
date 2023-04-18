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
    public class ApartadosController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public ApartadosController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("ConsultaAll")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetApartados()
        {
            return Ok(await _service.getApartados());
        }

        [HttpGet("Consulta/Usuario")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetApartadosByUsuario(int idUsuario,string type)
        {
            return Ok(await _service.getApartadosByUser(idUsuario,type));
        }

        #endregion

        #region POST
        [HttpPost("Agrega")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregarApartado([FromBody] ApartadosRequest request)
        {
            return Ok(await _service.postApartados(request));
        }
        #endregion

        #region PUT
        [HttpPut("Actualiza")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaApartado([FromBody] ApartadosRequest request)
        {
            return Ok(await _service.putApartados(request));
        }
        #endregion

        #region DELETE
        [HttpDelete("EliminaCategoria")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EliminaUbicacion([FromBody] CategoriaRequest request)
        {
            return Ok(await _service.deleteCategoria(request));
        }
        #endregion
    }
}
