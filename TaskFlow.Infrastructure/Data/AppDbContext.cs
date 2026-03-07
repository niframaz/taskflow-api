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
        public DbSet<OrganizationMembership> OrganizationMemberships { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<TaskReaction> TaskReactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Organizations)
                .WithMany(p => p.Users);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Projects)
                .WithMany(p => p.Users);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.OrganizationUserRoles)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Organization>()
                .HasMany(u => u.Projects)
                .WithOne(o => o.Organization)
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(t => t.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(t => t.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskReaction>()
                .HasOne(t => t.TaskItem)
                .WithMany(u => u.TaskReactions)
                .HasForeignKey(t => t.TaskItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TaskItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CommentReaction>()
                .HasOne(c => c.Comment)
                .WithMany(t => t.CommentReactions)
                .HasForeignKey(c => c.CommentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommentReaction>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrganizationMembership>()
                .HasOne(or => or.Organization)
                .WithMany()
                .HasForeignKey(o => o.OrganizationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrganizationRole>()
                .HasOne(or => or.OrganizationUserRole)
                .WithMany(or => or.OrganizationRoles)
                .HasForeignKey(o => o.OrganizationMembershipId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskReaction>()
                .HasIndex(tr => new { tr.TaskItemId, tr.UserId })
                .IsUnique();

            modelBuilder.Entity<CommentReaction>()
                .HasIndex(cr => new { cr.CommentId, cr.UserId })
                .IsUnique();

            modelBuilder.Entity<OrganizationMembership>()
                .HasIndex(cr => new { cr.OrganizationId, cr.UserId })
                .IsUnique();

            modelBuilder.Entity<OrganizationRole>()
                .HasIndex(cr => new { cr.OrganizationMembershipId, cr.Role })
                .IsUnique();

            modelBuilder.Entity<TaskReaction>()
                .HasIndex(tr => new { tr.TaskItemId, tr.UserId })
                .IsUnique();

            modelBuilder.Entity<CommentReaction>()
                .HasIndex(cr => new { cr.CommentId, cr.UserId })
                .IsUnique();

            modelBuilder.Entity<OrganizationMembership>()
                .HasIndex(ou => new { ou.OrganizationId, ou.UserId })
                .IsUnique();

            modelBuilder.Entity<TaskItem>()
                .HasIndex(t => new { t.ProjectId, t.Title });

            //modelBuilder.Entity<Organization>().HasData(new Organization
            //{
            //    Id = 1,
            //    Name = "Acme Corp",
            //    Description = "Sample organization for seeding"
            //});

            //modelBuilder.Entity<Project>().HasData(new Project
            //{
            //    Id = 1,
            //    Name = "Project Alpha",
            //    Description = "Sample project",
            //    OrganizationId = 1,
            //    Organization = null!
            //});
        }
    }
}
