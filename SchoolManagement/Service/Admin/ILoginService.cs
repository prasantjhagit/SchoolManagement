using SchoolManagement_Ui.DTO;
using SchoolManagement_Ui.Models.Login;

namespace SchoolManagement_Ui.Service.Admin
{
    public interface ILoginService
    {
        Task<LoginResponse> LoginAsync(LoginModel model);
    }
}