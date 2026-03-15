using Microsoft.AspNetCore.Mvc;
using SchoolManagement_Api.DTO;
using SchoolManagement_Api.Service.Admin;

namespace SchoolManagement_Api.Controllers
{
    [Route("apilogin/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _loginService.LoginAsync(model);

            if (result == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            return Ok(result);
        }
    }
}