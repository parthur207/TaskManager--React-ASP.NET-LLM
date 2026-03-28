using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.ResposePattern;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [AllowAnonymous]
    public class UserController : Controller
    {

        private readonly UserUseCaseFacade _userUseCaseFacade;
        public UserController(UserUseCaseFacade userUseCaseFacade)
        {
            _userUseCaseFacade = userUseCaseFacade;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] LoginRequestModel model)
        {
            var Response = await _userUseCaseFacade.Login.ExecuteAsync(model);

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
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] CreateUserModel model)
        {
            var Response= await _userUseCaseFacade.Create.ExecuteAsync(model);

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
        }
    }
}
