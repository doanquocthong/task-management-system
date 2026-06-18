using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using task_management_system.Data;
using task_management_system.Models.Entities;
using task_management_system.Models.Enum;
using task_management_system.Models.ViewModels.ProjectTask;
using task_management_system.Services.Interfaces;

namespace task_management_system.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả task (Dành cho Manager xem tổng quan)
        public async Task<IEnumerable<ProjectTaskViewModel>> GetAllTasksAsync()
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Select(t => new ProjectTaskViewModel
                {
                    Id = t.Id,
                    Title = t.title,
                    Description = t.description,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate,
                    Status = t.status,
                    ProjectName = t.Project != null ? t.Project.Name : "N/A",
                    AssigneeName = t.Assignee != null ? t.Assignee.FullName : "Chưa phân công",
                    CreatorName = t.CreatedBy != null ? t.CreatedBy.FullName : "N/A"
                })
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        // Lấy task được giao cho 1 User cụ thể (Dành cho trang "My Tasks" của Employee)
        public async Task<IEnumerable<ProjectTaskViewModel>> GetTasksByAssigneeAsync(int userId)
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.CreatedBy)
                .Where(t => t.AssigneeId == userId)
                .Select(t => new ProjectTaskViewModel
                {
                    Id = t.Id,
                    Title = t.title,
                    Description = t.description,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate,
                    Status = t.status,
                    ProjectName = t.Project != null ? t.Project.Name : "N/A",
                    CreatorName = t.CreatedBy != null ? t.CreatedBy.FullName : "N/A"
                })
                .ToListAsync();
        }

        // Chuẩn bị dữ liệu Dropdown Projects và Users từ DB ra ngoài Form
        public async Task<ProjectTaskCreateViewModel> GetProjectTaskCreateViewModelAsync()
        {
            var projects = await _context.Projects.ToListAsync(); // Get toàn bộ project trong DB
            var users = await _context.Users.ToListAsync(); // Get toàn bộ user trong DB

            return new ProjectTaskCreateViewModel
            {
                ProjectList = new SelectList(projects, "Id", "Name"),
                UserList = new SelectList(users, "Id", "FullName") 
            };
        }

        // Thực hiện lưu Task mới vào DB
        public async Task CreateTaskAsync(ProjectTaskCreateViewModel model, int creatorId)
        {
            var task = new ProjectTask
            {
                title = model.Title,
                description = model.Description ?? string.Empty,
                DueDate = model.DueDate,
                status = model.Status,
                ProjectId = model.ProjectId,
                AssigneeId = model.AssigneeId,
                CreatedById = creatorId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ProjectTaskCreateViewModel?> GetTaskForEditAsync(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return null;

            var projects = await _context.Projects.ToListAsync();
            var users = await _context.Users.ToListAsync();

            // Khởi tạo ViewModel và nạp dữ liệu cũ của Task vào
            return new ProjectTaskCreateViewModel
            {
                Title = task.title,
                Description = task.description,
                DueDate = task.DueDate,
                Status = task.status,
                ProjectId = task.ProjectId,
                AssigneeId = task.AssigneeId,

                // EF Core sẽ tự động so khớp ProjectId và AssigneeId để "Chọn sẵn" (Select) trên giao diện
                ProjectList = new SelectList(projects, "Id", "Name", task.ProjectId),
                UserList = new SelectList(users, "Id", "FullName", task.AssigneeId)
            };
        }
        public async Task UpdateTaskAsync(ProjectTaskCreateViewModel model)
        {
            // 1. Tìm đối tượng Task gốc đang nằm dưới Database dựa vào Id
            var task = await _context.ProjectTasks.FindAsync(model.Id);

            // 2. Nếu tìm thấy thì tiến hành cập nhật dữ liệu mới từ Form (Model) sang
            if (task != null)
            {
                task.title = model.Title;
                task.description = model.Description ?? string.Empty;
                task.DueDate = model.DueDate;
                task.status = model.Status;
                task.ProjectId = model.ProjectId;
                task.AssigneeId = model.AssigneeId;

                // 3. Đánh dấu thực thể thay đổi và lưu vào SQL Server
                _context.ProjectTasks.Update(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTaskWithIdAsync(int id, ProjectTaskCreateViewModel model)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task != null)
            {
                task.title = model.Title;
                task.description = model.Description ?? string.Empty;
                task.DueDate = model.DueDate;
                task.status = model.Status;
                task.ProjectId = model.ProjectId;
                task.AssigneeId = model.AssigneeId;

                _context.ProjectTasks.Update(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateStatusAsync(int taskId, int userId, ProjectTaskStatus newStatus)
        {
            // Tìm đúng Task và kiểm tra nghiêm ngặt xem Task này có phải được giao cho User này không
            var task = await _context.ProjectTasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.AssigneeId == userId);

            if (task == null) return false; // Không tìm thấy hoặc nhân viên khác cố tình can thiệp

            // Cập nhật trạng thái mới
            task.status = newStatus;

            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}