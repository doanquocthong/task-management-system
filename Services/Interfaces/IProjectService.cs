using task_management_system.Models.ViewModels.Project;

namespace task_management_system.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync();
        Task<ProjectCreateViewModel?> GetProjectForEditAsync(int id);
        Task CreateProjectAsync(ProjectCreateViewModel model, int currentUserId);
        Task UpdateProjectAsync(ProjectCreateViewModel model);
        Task DeleteProjectAsync(int id);
    }
}