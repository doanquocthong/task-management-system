using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_management_system.Models.Enum;
using task_management_system.Models.ViewModels.ProjectTask;
using task_management_system.Services.Interfaces;

namespace task_management_system.Controllers
{
    [Authorize]
    public class ProjectTaskController : Controller
    {
        private readonly IProjectTaskService _taskService;

        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        // Xem toàn bộ danh sách task (Thường cho Manager điều phối)
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return View(tasks);
        }

        // Giao diện xem Task cá nhân của Nhân viên (Employee)
        public async Task<IActionResult> MyTasks()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value; // Theo đúng file Auth của user
            if (int.TryParse(userIdClaim, out int currentUserId))
            {
                var myTasks = await _taskService.GetTasksByAssigneeAsync(currentUserId);
                return View(myTasks);
            }
            return RedirectToAction("Login", "Auth");
        }

        // Giao diện Tạo Task mới (Chỉ dành cho Manager)
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            var model = await _taskService.GetProjectTaskCreateViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTaskCreateViewModel model)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                ModelState.AddModelError("", "Không thể xác thực tài khoản Manager.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                await _taskService.CreateTaskAsync(model, currentUserId);
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi Validate, nạp lại danh sách dropdown để tránh lỗi sập giao diện
            var reloadModel = await _taskService.GetProjectTaskCreateViewModelAsync();
            model.ProjectList = reloadModel.ProjectList;
            model.UserList = reloadModel.UserList;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _taskService.GetTaskForEditAsync(id);
            if (model == null) return NotFound();

            // Lưu tạm Id vào ViewBag để form POST lấy lại đúng ID cần sửa
            ViewBag.TaskId = id;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTaskCreateViewModel model)
        {
            // Đảm bảo Id từ Route được gán vào Model trước khi truyền xuống Service
            model.Id = id;

            if (ModelState.IsValid)
            {
                await _taskService.UpdateTaskAsync(model); // Gọi hàm mượt mà không cần ép kiểu
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi nạp lại danh sách dropdown...
            var reloadModel = await _taskService.GetProjectTaskCreateViewModelAsync();
            model.ProjectList = reloadModel.ProjectList;
            model.UserList = reloadModel.UserList;
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Employee")] // Chỉ nhân viên mới kích hoạt được luồng này
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMyTaskStatus(int taskId, ProjectTaskStatus status)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out int currentUserId))
            {
                var result = await _taskService.UpdateStatusAsync(taskId, currentUserId, status);
                if (result)
                {
                    return RedirectToAction(nameof(MyTasks));
                }
            }
            return BadRequest("Không thể cập nhật trạng thái công việc.");
        }
    }
}