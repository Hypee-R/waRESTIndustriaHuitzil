using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public DashboardController(IIndustriaHuitzilService service) => _service = service;

        #region Cards
        [HttpGet("Cards")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCards(int idSucursal)
        {
            return Ok(await _service.getCards(idSucursal));
        }
        #endregion

        #region ChartBar
        [HttpGet("ChartBar")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDataChartBar(int idSucursal)
        {
            return Ok(await _service.getVentasPorMes(idSucursal));
        }
        #endregion

        #region RankingArticulos
        [HttpGet("RankingArticles")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRankingArticles()
        {
            return Ok(await _service.getRankingArticulos());
        }
        #endregion

        #region RankingEmpleados
        [HttpGet("RankingEmpleados")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRankingEmpleados()
        {
            return Ok(await _service.getRankingEmpleados());
        }
        #endregion

        #region RankingSucursales
        [HttpGet("RankingSucursales")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRankingSucursales()
        {
            return Ok(await _service.getRankingSucursales());
        }
        #endregion
    }
}
