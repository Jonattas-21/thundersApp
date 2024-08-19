using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thundersApp.Dtos;

namespace thundersApp.Controllers
{
    public class WineController : Controller
    {
        private readonly IWineService _wineService;
        private readonly IMapper _mapper;

        public WineController(IWineService service, IMapper mapper)
        {
            _wineService = service;
            _mapper = mapper;
        }


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
        public ActionResult CreateWine(WineDto item)
        {
            try
            {
                var wineData = _mapper.Map<Wine>(item);
                var (result, validations) = _wineService.AddWine(wineData);

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
