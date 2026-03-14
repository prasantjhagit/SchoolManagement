namespace SchoolManagement_Api.Reposirty.Admin
{
    public interface ILoginServiceRepo
    {
        Task<string> LoginAsync(string email, string password);

    }
}
