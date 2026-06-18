using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using task_management_system.Models.Enum;

namespace task_management_system.Models.ViewModels.ProjectTask
{
    public class ProjectTaskCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Pending;

        [Required]
        public int ProjectId { get; set; }

        public int? AssigneeId { get; set; } // Có thể để trống nếu chưa muốn giao cho ai ngay

        // --- Các danh sách dùng để đổ vào thẻ <select> trên giao diện ---
        public SelectList? ProjectList { get; set; }
        public SelectList? UserList { get; set; }
    }
}