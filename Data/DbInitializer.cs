using task_management_system.Models.Common;
using task_management_system.Models.Entities;

namespace task_management_system.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                FullName = "Manager",
                Email = "manager@gmail.com",
                Password = "123",
                Role = RoleConstants.Manager
            });

            context.Users.Add(new User
            {
                FullName = "Employee",
                Email = "employee@gmail.com",
                Password = "123",
                Role = RoleConstants.Employee
            });

            context.SaveChanges();
        }

    }
}