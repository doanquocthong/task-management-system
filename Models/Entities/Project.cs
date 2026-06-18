namespace task_management_system.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public ICollection<ProjectTask>? Tasks { get; set; }
    }
}
