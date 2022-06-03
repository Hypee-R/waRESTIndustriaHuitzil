using CoreIndustriaHuitzil.ModelsRequest;
using Microsoft.AspNetCore.Mvc;
using ServiceIndustriaHuitzil.Services;

namespace waRESTIndustriaHuitzil.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIndustriaHuitzilService _service;
        public AuthController(IIndustriaHuitzilService service) => _service = service;

        #region GET
        #endregion

        #region POST
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthUserRequest request)
        {
            return Ok(await _service.auth(request));
        }
        #endregion
    }
}
