using System.ComponentModel.DataAnnotations;

namespace task_management_system.Models.ViewModels.Project
{
    public class ProjectCreateViewModel
    {
        public int Id { get; set; } // Dùng khi Edit

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}