using Microsoft.EntityFrameworkCore;
using task_management_system.Data;
using task_management_system.Models.Entities;
using task_management_system.Models.ViewModels.Auth;
using task_management_system.Services.Interfaces;

namespace task_management_system.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        var exists = await _context.Users
            .AnyAsync(x => x.Email == model.Email);

        if (exists) return false;

        var user = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password, // demo nên chưa hash
            Role = "Employee"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User?> LoginAsync(LoginViewModel model)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Email == model.Email &&
                x.Password == model.Password);

        return user;
    }
}