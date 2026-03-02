using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TaskItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Projects)
                .WithMany(p => p.Users);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Organization)
                .WithMany(o => o.Projects)
                .HasForeignKey(p => p.OrganizationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganizationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Organization>().HasData(new Organization
            {
                Id = 1,
                Name = "Acme Corp",
                Description = "Sample organization for seeding"
            });

            modelBuilder.Entity<Project>().HasData(new Project
            {
                Id = 1,
                Name = "Project Alpha",
                Description = "Sample project",
                OrganizationId = 1
            });

            //modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            //{
            //    Id = "user-1",
            //    UserName = "john.doe",
            //    NormalizedUserName = "JOHN.DOE",
            //    Email = "john.doe@example.com",
            //    NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
            //    EmailConfirmed = false,
            //    Name = "John Doe",
            //    OrganizationId = 1,
            //    SecurityStamp = "00000000-0000-0000-0000-000000000001"
            //});

            //modelBuilder.Entity<TaskItem>().HasData(new TaskItem
            //{
            //    Id = 1,
            //    Title = "Initial Task",
            //    Description = "Sample task assigned to user",
            //    UserId = "user-1",
            //    Liked = false
            //});

            //modelBuilder.Entity<Comment>().HasData(new Comment
            //{
            //    Id = 1,
            //    Message = "This is a sample comment",
            //    Reaction = TaskFlow.Domain.Enums.Reaction.Like,
            //    TaskItemId = 1
            //});
        }
    }
}
