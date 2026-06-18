using System.ComponentModel.DataAnnotations;

namespace task_management_system.Models.ViewModels.Auth;

public class LoginViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}