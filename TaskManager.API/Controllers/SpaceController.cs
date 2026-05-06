using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/spaces")]
    public class SpaceController
    {
        private readonly SpaceUseCaseFacade _spaceUseCaseFacade;

        public SpaceController(SpaceUseCaseFacade spaceUseCaseFacade)
        {
            _spaceUseCaseFacade = spaceUseCaseFacade;
        }

        public async Task<IActionResult> GetSpaces()
        {
            var spaces = await _spaceUseCaseFacade.();
            return new OkObjectResult(spaces);
        }
}
