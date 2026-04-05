using Microsoft.EntityFrameworkCore;
using TeacherControl.Enums;
using TeacherControl.Models;

namespace TeacherControl.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(u => u.Description).HasColumnName("description").IsRequired().HasMaxLength(250);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(u => u.Password).HasColumnName("password").IsRequired().HasMaxLength(250);
            entity.Property(u => u.Status).HasColumnName("status").IsRequired().HasConversion<int>()
                .HasDefaultValue(UserStatus.Active);
            entity.Property(u => u.RoleId).HasColumnName("role_id");
            
            //Garante que o usuário não possa ser excluído se houver uma relação com o role
            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("Lessons");
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Date).HasColumnName("date").IsRequired().HasColumnType("date");     
            entity.Property(u => u.Title).HasColumnName("title").IsRequired().HasMaxLength(100);
            entity.Property(u => u.Description).HasColumnName("description").IsRequired().HasMaxLength(250);
            entity.Property(u => u.Status).HasColumnName("status").IsRequired().HasConversion<int>()
                .HasDefaultValue(LessonStatus.Pending);
            entity.Property(u => u.UserId).HasColumnName("user_id");

            entity.HasOne(u => u.User)
                .WithMany(u => u.Lessons)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        
    }
}