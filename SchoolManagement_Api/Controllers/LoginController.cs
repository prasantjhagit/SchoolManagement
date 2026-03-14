using Microsoft.AspNetCore.Mvc;
using SchoolManagement_Api.Service.Admin;

namespace SchoolManagement_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _loginService.LoginAsync(email, password);

            return Ok(result);
        }
    }
}