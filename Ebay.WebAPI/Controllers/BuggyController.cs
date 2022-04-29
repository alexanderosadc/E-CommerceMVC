using Microsoft.AspNetCore.Mvc;

namespace Ebay.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : Controller
    {
        [HttpGet("servererror")]
        public async Task<IActionResult> ServerError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("badrequest")]
        public async Task<IActionResult> BadRequest()
        {
            var returnJson = new { Message = "Current Item Not Found" };
            return StatusCode(StatusCodes.Status400BadRequest, returnJson);
        }

        [HttpGet("notfound")]
        public async Task<IActionResult> NotFound()
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
    }
}