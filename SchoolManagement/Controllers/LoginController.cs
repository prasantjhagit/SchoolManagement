using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement_Ui.Models.Login;
using SchoolManagement_Ui.Service.Admin;
using System.Security.Claims;
using System.Text;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }
    [HttpGet]
    public IActionResult Login()
    {



        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        //string apiKey = "cWy83ipk0iWWaieCAYYpd3lwhgjLqXV4q2jPD73rL5IRFc8CekQl7NjxKnwD"; // paste your key

        //using (HttpClient client = new HttpClient())
        //{
        //    client.DefaultRequestHeaders.Clear();
        //    client.DefaultRequestHeaders.Add("authorization", apiKey);

        //    var json = @"{
        //        ""route"": ""q"",
        //        ""message"": ""Your OTP is 2321"",
        //        ""language"": ""english"",
        //        ""numbers"": ""919234652921""
        //    }";

        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await client.PostAsync("https://www.fast2sms.com/dev/bulkV2", content);
        //    var result1 = await response.Content.ReadAsStringAsync();

        //    Console.WriteLine("Status: " + response.StatusCode);
        //    //Console.WriteLine("Response: " + result);
        //}
    




        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _loginService.LoginAsync(model);

        if (result != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Email),
            new Claim(ClaimTypes.Role, result.Role)
        };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(result.Role))
                return RedirectToAction("DashboardPartial", "Admin");
        }

        TempData["ErrorMessage"] = "Invalid Email or Password";
        return View(model);
    }
}