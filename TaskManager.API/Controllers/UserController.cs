using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Models;

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
            var Response = await _userUseCaseFacade.Login.Exe(model);
            if()

            return Ok("Login Efetuado com sucesso.");
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] CreateUserModel model)
        {
            return Created();
        }
    }
}
