using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.TaskCategory;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/task")]
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;
        private readonly TaskUseCaseFacade _taskUseCaseFacade;
        public TasksController(ILogger<TasksController> logger, TaskUseCaseFacade taskUseCaseFacade)
        {
            _logger = logger;
            _taskUseCaseFacade = taskUseCaseFacade;
        }

        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model)
        {
            var Response = await _taskUseCaseFacade.Create.ExecuteAsync(model);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
