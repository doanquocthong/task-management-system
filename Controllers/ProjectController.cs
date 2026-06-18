using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_management_system.Models.ViewModels.Project;
using task_management_system.Services.Interfaces;

namespace task_management_system.Controllers
{
    [Authorize] // Phải đăng nhập
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // Ai cũng có thể xem danh sách dự án
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects);
        }

        // Chỉ Manager mới được Tạo / Sửa / Xóa
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Lấy ID của người dùng hiện tại từ Claims
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out int currentUserId))
            {
                await _projectService.CreateProjectAsync(model, currentUserId);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Không thể xác thực tài khoản.");
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _projectService.GetProjectForEditAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _projectService.UpdateProjectAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}