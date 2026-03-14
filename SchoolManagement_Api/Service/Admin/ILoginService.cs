namespace SchoolManagement_Api.Service.Admin
{
    public interface ILoginService
    {
        Task<string> LoginAsync(string email, string password);
    }
}
