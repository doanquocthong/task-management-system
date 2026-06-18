using task_management_system.Models.Enum;

namespace task_management_system.Models.Entities
{
    public class ProjectTask
    {
        public int Id { get; set; }

        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public ProjectTaskStatus status {  get; set; } = ProjectTaskStatus.Pending; 

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        // --- Người thực hiện task ---
        public int? AssigneeId { get; set; } 
        public User? Assignee { get; set; } 

        // --- Manager giao task ---
        public int? CreatedById { get; set; } 
        public User? CreatedBy { get; set; }  
    }
}
