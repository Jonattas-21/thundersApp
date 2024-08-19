using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace thundersApp.Controllers
{
    public class WineController : Controller
    {




        [HttpGet("FindWineById")]
        public ActionResult FindWineById(int id)
        {

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Hello, Microsoft.Extensions.Logging!");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateWine(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPatch]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWine(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult DeleteWine(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
