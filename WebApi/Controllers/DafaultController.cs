using Microsoft.AspNetCore.Mvc;

namespace thundersApp.Controllers
{
    [Route("Default")]
    public class DafaultController : Controller
    {
        [HttpGet("Health")]
        public IActionResult Heath()
        {
            return Ok("I am alive!!!");
        }
    }
}
