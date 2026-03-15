using SchoolManagement_Api.DTO;
using SchoolManagement_Api.Reposirty.Admin;

namespace SchoolManagement_Api.Service.Admin
{
    public class LoginService : ILoginService
    {
        private readonly ILoginServiceRepo _loginService;

        public LoginService(ILoginServiceRepo loginService)
        {
            _loginService = loginService;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO model)
        {
            return await _loginService.LoginAsync(model);
        }
    }
}