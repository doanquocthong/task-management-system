using task_management_system.Models.Enum;

namespace task_management_system.Models.ViewModels.ProjectTask
{
    public class ProjectTaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string AssigneeName { get; set; } = "Chưa phân công";
        public string CreatorName { get; set; } = string.Empty;
    }
}