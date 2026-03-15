using Microsoft.EntityFrameworkCore;
using SchoolManagement_Api.DTO;

namespace SchoolManagement_Api.Reposirty.Admin
{
    public class LoginServiceRepo : ILoginServiceRepo
    {
        private readonly ApplicationDbContext _context;

        public LoginServiceRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO model)
        {
            try
            {
               
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(x =>
                        x.Email == model.Email &&
                        x.Password == model.Password);

                if (user == null)
                {
                    return null;
                }

               
                var permissions = await _context.RolePermissions
                    .Where(rp => rp.RoleId == user.RoleId)
                    .Include(rp => rp.Permission)
                    .Select(rp => rp.Permission.PermissionName)
                    .ToListAsync();

               
                var response = new LoginResponseDTO
                {
                    Email = user.Email,
                    Role = user.Role.RoleName,
                    Permissions = permissions
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}