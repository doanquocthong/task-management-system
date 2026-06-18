using task_management_system.Models.Enum;
using task_management_system.Models.ViewModels.ProjectTask;

namespace task_management_system.Services.Interfaces
{
    public interface IProjectTaskService
    {
        Task<IEnumerable<ProjectTaskViewModel>> GetAllTasksAsync();
        Task<IEnumerable<ProjectTaskViewModel>> GetTasksByAssigneeAsync(int userId);
        Task<ProjectTaskCreateViewModel> GetProjectTaskCreateViewModelAsync();
        Task CreateTaskAsync(ProjectTaskCreateViewModel model, int creatorId);
        Task DeleteTaskAsync(int id);
        Task<ProjectTaskCreateViewModel?> GetTaskForEditAsync(int id);
        Task UpdateTaskAsync(ProjectTaskCreateViewModel model);
        Task<bool> UpdateStatusAsync(int taskId, int userId, ProjectTaskStatus newStatus);
    }
}