using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Reposirty.Admin
{
    public interface ILoginServiceRepo
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO model);

    }
}
