using System.Data;

namespace SchoolManagement_Api.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDTO
    {
        public string Email { get; set; }

        public string Role { get; set; }

        public List<string> Permissions { get; set; }
    }
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
    public class Permission
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
    public class RolePermission
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public Role Role { get; set; }

        public Permission Permission { get; set; }
    }
}
