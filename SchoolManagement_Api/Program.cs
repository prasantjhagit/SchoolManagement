using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SchoolManagement_Api.Authentication;
using SchoolManagement_Api.Reposirty.Admin;
using SchoolManagement_Api.Service.Admin;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddScoped<IAdmissionRepo, AdmissionRepo>();
builder.Services.AddScoped<IAdmissionService, AdmissionService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Management API", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authentication header"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginServiceRepo, LoginServiceRepo>();
builder.Services.AddScoped<IAdmissionService, AdmissionService>();
builder.Services.AddScoped<IAdmissionRepo, AdmissionRepo>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowUI");
app.UseAuthorization();
app.MapControllers();
app.Run();
