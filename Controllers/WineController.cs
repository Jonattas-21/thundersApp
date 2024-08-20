using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using thundersApp.Dtos;

namespace thundersApp.Controllers
{
    public class WineController : Controller
    {
        private readonly IWineService _wineService;
        private readonly ILogger<WineController> _logger;
        private readonly IMapper _mapper;

        public WineController(IWineService service, IMapper mapper, ILogger<WineController> logger)
        {
            _wineService = service;
            _mapper = mapper;
            _logger = logger;
        }

        [OutputCache(Duration = 90, VaryByQueryKeys = new[] { "id" })]
        [HttpGet("FindWineById")]
        public ActionResult FindWineById(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            var wine = _wineService.GetWineById(id);
            if (wine == null)
            {
                response.Message = "Wine not found for ID: " + id.ToString();
                return NotFound(response);
            }

            var wineResponseDto = _mapper.Map<WineResponseDto>(wine);
            var grapeResponseDto = _mapper.Map<GrapeResponseDto>(wine.Grape);
            wineResponseDto.Grape = grapeResponseDto;

            response.Message = "Wine found successfully";
            response.Data = wineResponseDto;
            return Ok(response); ;
        }

        [HttpPost("CreateWine")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateWine(WineRequestDto item)
        {
            try
            {
                var wineData = _mapper.Map<Wine>(item);

                var grapeData = new Grape();
                grapeData.Name = item.GrapeName;
                grapeData.Description = item.GrapeDescription;
                grapeData.Origin = item.GrapeOrigin;

                wineData.Grape = grapeData;
                var (result, validations) = _wineService.CreateWine(wineData);

                DefaultResponse response = new DefaultResponse();
                if (validations.Count > 0)
                {
                    response.Message = "Error creating wine";
                    response.Data = validations;
                    return BadRequest(response);
                }

                var wineResponseDto = _mapper.Map<WineResponseDto>(result);
                var grapeResponseDto = _mapper.Map<GrapeResponseDto>(result.Grape);
                wineResponseDto.Grape = grapeResponseDto;

                response.Message = "Wine created successfully";
                response.Data = wineResponseDto;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating wine");
                return StatusCode(500, "Internal error happened while creating Analysis");
            }
        }

        [HttpPatch("UpdateWine")]
        public ActionResult UpdateWine(Guid id, Dictionary<string, string> fields)
        {
            try
            {
                var (result, validations) = _wineService.UpdateWine(id, fields);

                DefaultResponse response = new DefaultResponse();
                if (validations.Count > 0)
                {
                    response.Message = "Error while updating wine";
                    response.Data = validations;
                    return BadRequest(response);
                }

                var wineResponseDto = _mapper.Map<WineResponseDto>(result);
                var grapeResponseDto = _mapper.Map<GrapeResponseDto>(result.Grape);
                wineResponseDto.Grape = grapeResponseDto;

                response.Message = "Wine updated successfully";
                response.Data = wineResponseDto;
                return Ok(response);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating wine");
                return StatusCode(500, "Internal error happened while updating wine");
            }
        }

        [HttpDelete("DeleteWine")]
        public ActionResult DeleteWine(Guid id)
        {
            try
            {
                DefaultResponse response = new DefaultResponse();
                response.Message = _wineService.DeleteWine(id) ? "Wine deleted successfully" : "Wine not found for ID: " + id.ToString();
                response.Data = id;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting wine");
                return StatusCode(500, "Internal error happened while deleting wine");
            }
        }

        [HttpPatch("EnableDisableWine")]
        public ActionResult EnableDisableWine(Guid id)
        {
            try
            {
                DefaultResponse response = new DefaultResponse();
                response.Message = _wineService.EnableDisableWine(id) ? "Wine enabled or disabled successfully" : "Wine not found for ID: " + id.ToString();
                response.Data = id;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error enabling or disabling wine");
                return StatusCode(500, "Internal error happened while enabling or disabling wine");
            }
        }
    }
}
