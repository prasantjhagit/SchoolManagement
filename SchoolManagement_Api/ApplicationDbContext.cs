using Microsoft.EntityFrameworkCore;
using SchoolManagement_Api.DTO;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<StudentAddress> StudentAddresses { get; set; }
    public DbSet<StudentDocument> StudentDocuments { get; set; }

    public DbSet<StudentAttendance> StudentAttendances { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<StudentTransfers> StudentTransfer { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Student -> Address
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Address)
            .WithOne(a => a.Student)
            .HasForeignKey<StudentAddress>(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Student -> Documents
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Documents)
            .WithOne(d => d.Student)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Role
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        // RolePermission composite key
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        modelBuilder.Entity<StudentAttendance>(entity =>
        {
            entity.ToTable("StudentAttendance");

            entity.HasKey(a => a.AttendanceId);

            entity.HasOne(a => a.Student)
                  .WithMany(s => s.StudentAttendances)
                  .HasForeignKey(a => a.StudentId);
        });


    }
}