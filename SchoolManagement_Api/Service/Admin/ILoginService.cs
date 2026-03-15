using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Service.Admin
{
    public interface ILoginService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO model);
    }
}
