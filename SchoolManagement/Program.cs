using Microsoft.AspNetCore.Authentication.Cookies;
using SchoolManagement_Ui.Service.Admin;
using SchoolManagement_Ui.Service.Admin.Student;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register your custom services
builder.Services.AddScoped<ILoginService, Login>();
builder.Services.AddScoped<IAdmissionService, AdmissionService>();

// Register HttpClient so AdmissionService can use it
builder.Services.AddHttpClient(); // ✅ important for HttpClient DI

// Add authentication & authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";           // Redirect for login
        options.AccessDeniedPath = "/Login/AccessDenied"; // Redirect for access denied
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANT: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();