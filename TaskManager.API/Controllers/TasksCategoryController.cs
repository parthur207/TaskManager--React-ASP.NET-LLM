using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.TaskCategory;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasksCategory")]
    public class TasksCategoryController : ControllerBase
    {
        private readonly TaskCategoryUseCaseFacade _taskCategoryUseCaseFacade;
        public TasksCategoryController(TaskCategoryUseCaseFacade taskCategoryUseCaseFacade)
        {
            _taskCategoryUseCaseFacade = taskCategoryUseCaseFacade;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] Guid SpaceId)
        {
            var Response = await _taskCategoryUseCaseFacade.Get.ExecuteAsync(SpaceId);
            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);
                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);
                case ResponseStatusEnum.Success:
                    return Ok(Response);
                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);
                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateTaskCategoryModel model)
        {
            var Response = await _taskCategoryUseCaseFacade.Create.ExecuteAsync(model);
            
            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateTaskCategoryModel model)
        {
            var Response = await _taskCategoryUseCaseFacade.Update.ExecuteAsync(model);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategory([FromQuery] Guid TaskCategoryId)
        {
            var Response = await _taskCategoryUseCaseFacade.Delete.ExecuteAsync(TaskCategoryId);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }
    }
}
