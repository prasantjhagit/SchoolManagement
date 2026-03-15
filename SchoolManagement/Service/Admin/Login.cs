using System.Net.Http;
using System.Net.Http.Json;
using SchoolManagement_Ui.DTO;
using SchoolManagement_Ui.Models.Login;

namespace SchoolManagement_Ui.Service.Admin
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {
            var loginData = new
            {
                Email = model.Email,
                Password = model.Password
            };

            var response = await _httpClient.PostAsJsonAsync("apilogin/Login/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }

            return null;
        }
    }
}