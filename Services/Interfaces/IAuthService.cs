using task_management_system.Models.Entities;
using task_management_system.Models.ViewModels.Auth;

namespace task_management_system.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterViewModel model);

    Task<User?> LoginAsync(LoginViewModel model);
}
