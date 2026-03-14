using Microsoft.EntityFrameworkCore;
using SchoolManagement_Api.DTO;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<StudentAddress> StudentAddresses { get; set; }
    public DbSet<StudentDocument> StudentDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Address)
            .WithOne(a => a.Student)
            .HasForeignKey<StudentAddress>(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Documents)
            .WithOne(d => d.Student)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}