namespace task_management_system.Models.ViewModels.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatorName { get; set; } = string.Empty;
        public int TaskCount { get; set; }
    }
}