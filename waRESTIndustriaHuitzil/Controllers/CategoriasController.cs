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
    public class CategoriasController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public CategoriasController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("ConsultaAll")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCategorias()
        {
            return Ok(await _service.getCategorias());
        }

        //[HttpGet("ConsultaAllVR")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetVistasRoles(int idRol)
        //{
        //    return Ok(await _service.getVistasRol(idRol));
        //}
        #endregion

        #region POST
        [HttpPost("AgregaCategoria")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregarCategoria([FromBody] CategoriaRequest request)
        {
            return Ok(await _service.postCategoria(request));
        }
        #endregion

        #region PUT
        [HttpPut("ActualizaCategoria")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizaCategoria([FromBody] CategoriaRequest request)
        {
            return Ok(await _service.putCategoria(request));
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
