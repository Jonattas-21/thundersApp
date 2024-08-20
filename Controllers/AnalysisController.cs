using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using thundersApp.Dtos;

namespace thundersApp.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly IAnalysisService _analysisService;
        private readonly ILogger<AnalysisController> _logger;
        private readonly IMapper _mapper;

        public AnalysisController(IAnalysisService service, IMapper mapper, ILogger<AnalysisController> logger)
        {
            _analysisService = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("FindAnalysisByWineId")]
        public ActionResult FindAnalysisByWineId(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            var analysis = _analysisService.GetAnalysisByWine(id);
            if (analysis == null)
            {
                response.Message = "analysis not found for ID: " + id.ToString();
                return NotFound(response);
            }

            var analysisResponseDto = _mapper.Map<WineResponseDto>(analysis);
            var wineResponseDto = _mapper.Map<WineResponseDto>(analysis.Wine);

            response.Message = "analysis found successfully";
            response.Data = analysisResponseDto;
            return Ok(response); ;
        }

        [HttpPost("CreateAnalysis")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateAnalysis(AnalysisRequestDto item)
        {
            try
            {
                var wineData = _mapper.Map<Analysis>(item);
                var (result, validations) = _analysisService.CreateAnalysis(wineData);

                DefaultResponse response = new DefaultResponse();
                if (validations.Count > 0)
                {
                    response.Message = "Error creating Analysis";
                    response.Data = validations;
                    return BadRequest(response);
                }

                var analysisResponseDto = _mapper.Map<AnalysisResponseDto>(result);
                var wineResponseDto = _mapper.Map<WineResponseDto>(result.Wine);

                analysisResponseDto.Wine = wineResponseDto;

                response.Message = "Analysis created successfully";
                response.Data = analysisResponseDto;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating Analysis");
                return StatusCode(500, "Internal error happened while creating Analysis");
            }
        }

        [HttpDelete("DeleteAnalysis")]
        public ActionResult DeleteAnalysis(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            var result = _analysisService.DeleteAnalysis(id);
            if (!result)
            {
                response.Message = "Error deleting analysis";
                return BadRequest(response);
            }

            response.Message = "Analysis deleted successfully";
            return Ok(response);
        }
    }
}
