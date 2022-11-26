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
        public async Task<IActionResult> GetCards(string year, int idSucursal)
        {
            return Ok(await _service.getCards(year, idSucursal));
        }
        #endregion

        #region ChartBar
        [HttpGet("ChartBar")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDataChartBar(string year, int idSucursal)
        {
            return Ok(await _service.getVentasPorMes(year, idSucursal));
        }
        #endregion

        #region RankingArticulos
        [HttpGet("RankingArticles")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRankingArticles(string year, int idSucursal)
        {
            return Ok(await _service.getRankingArticulos(year, idSucursal));
        }
        #endregion

        #region RankingEmpleados
        [HttpGet("RankingEmpleados")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRankingEmpleados(string year, int idSucursal)
        {
            return Ok(await _service.getRankingEmpleados(year, idSucursal));
        }
        #endregion

        #region RankingSucursales
        [HttpGet("RankingSucursales")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRankingSucursales(string year)
        {
            return Ok(await _service.getRankingSucursales(year));
        }
        #endregion
    }
}
