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
    public class PagosApartadosController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public PagosApartadosController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        [HttpGet("ConsultaAll")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPagos()
        {
            return Ok(await _service.getPagos());
        }

        [HttpGet("Consulta/Apartado")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPagosByApartado(int idApartado)
        {
            return Ok(await _service.getPagosByApartado(idApartado));
        }

        #endregion

        #region POST
        [HttpPost("Agrega")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AgregarPagoApartado([FromBody] PagoApartadoRequest request)
        {
            return Ok(await _service.postPagoApartado(request));
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
