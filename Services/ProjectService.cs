using Microsoft.EntityFrameworkCore;
using task_management_system.Data;
using task_management_system.Models.Entities;
using task_management_system.Models.ViewModels.Project;
using task_management_system.Services.Interfaces;

namespace task_management_system.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Tasks)
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    CreatorName = p.CreatedBy != null ? p.CreatedBy.FullName : "N/A",
                    TaskCount = p.Tasks != null ? p.Tasks.Count : 0
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<ProjectCreateViewModel?> GetProjectForEditAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return null;

            return new ProjectCreateViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description
            };
        }

        public async Task CreateProjectAsync(ProjectCreateViewModel model, int currentUserId)
        {
            var project = new Project
            {
                Name = model.Name,
                Description = model.Description ?? string.Empty,
                CreatedById = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectCreateViewModel model)
        {
            var project = await _context.Projects.FindAsync(model.Id);
            if (project != null)
            {
                project.Name = model.Name;
                project.Description = model.Description ?? string.Empty;

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}