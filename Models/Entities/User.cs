namespace task_management_system.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
           
        public string Password { get; set; } = string.Empty;


        public string Role { get; set; } = string.Empty;


        // Danh sách dự án do User này tạo (Manager)
        public ICollection<Project>? CreatedProjects { get; set; }

        // Danh sách công việc được giao cho User này (Employee nhận việc)
        public ICollection<ProjectTask>? AssignedTasks { get; set; }

        // Danh sách công việc do User này tạo/phân công (Manager giao việc)
        public ICollection<ProjectTask>? CreatedTasks { get; set; }
    }
}
