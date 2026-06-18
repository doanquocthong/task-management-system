using Microsoft.EntityFrameworkCore;
using task_management_system.Models.Entities;

namespace task_management_system.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; } // Thay thế DbSet<Task> cũ

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình User tạo Project
            modelBuilder.Entity<Project>()
                .HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedProjects)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict); // Khi xóa user, không tự động xóa project

            // 2. Cấu hình Mối quan hệ Người nhận việc (Assignee) trong Task
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Cấu hình Mối quan hệ Manager giao việc (CreatedBy) trong Task
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Cấu hình Task thuộc về Project
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // Nếu xóa Project, tất cả Task của Project đó sẽ bị xóa theo
        }
    }
}