using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManager.Core.Models;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/task")]
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;

        public TasksController(ILogger<TasksController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
