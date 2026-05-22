using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Space;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/spaces")]
    public class SpaceController : ControllerBase
    {
        private readonly SpaceUseCaseFacade _spaceUseCaseFacade;

        public SpaceController(SpaceUseCaseFacade spaceUseCaseFacade)
        {
            _spaceUseCaseFacade = spaceUseCaseFacade;
        }

       
        //sidebar
        [HttpGet("sidebar-data")]
        public async Task<IActionResult> GetSpacesMember()
        {
            var Response = await _spaceUseCaseFacade.();

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



        [HttpGet("space/{id}")]
        public async Task<IActionResult> GetSpaceById([FromRoute] Guid id)
        {
            var Response = await _spaceUseCaseFacade.getSpaceById.ExecuteAsync(id);

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
        public async Task<IActionResult> CreateSpace([FromBody] CreateSpaceModel space)
        {
            var createdSpace = await _spaceUseCaseFacade.create.ExecuteAsync(space);

            switch (createdSpace.Status)
            {
                case ResponseStatusEnum.Success:
                    return new OkObjectResult(createdSpace);
                case ResponseStatusEnum.Error:
                    return new BadRequestObjectResult(createdSpace);
                case ResponseStatusEnum.CriticalError:
                    return new StatusCodeResult(500);
                default:
                    return new ObjectResult(createdSpace) { StatusCode = 500 };
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSpace([FromRoute] Guid id)
        {
            var Response = await _spaceUseCaseFacade.delete.ExecuteAsync(id);

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

        [HttpPatch("leave/{id}")]
        public async Task<IActionResult> LeaveSpace([FromRoute] Guid id, [FromBody] UpdateSpaceModel space)
        {

            //

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

        [HttpPatch("remove-member")]
        public async Task<IActionResult> RemoveMemberFromSpace([FromBody] MemberSpaceModel model)
        {
            var Response = await _spaceUseCaseFacade.removeMember.ExecuteAsync(model);
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