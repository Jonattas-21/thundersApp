using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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

        [OutputCache(Duration = 90, VaryByQueryKeys = new[] { "id" })]
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

            var analysisResponseDto = _mapper.Map<AnalysisResponseDto>(analysis);
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

        [OutputCache(Duration = 90, VaryByQueryKeys = new[] { "Sweet", "Tannin", "Acidity", "Alcohol", "Body" })]
        [HttpGet("FindAnalysisByCategory")]
        public ActionResult FindAnalysisByCategory(int? Sweet, int? Tannin, int? Acidity, int? Alcohol, int? Body)
        {
            DefaultResponse response = new DefaultResponse();

            var analyses = _analysisService.GetAnalysesByCategories(Sweet, Tannin, Acidity, Alcohol, Body).ToList();
            if (analyses == null || analyses.Count() == 0)
            {
                response.Message = "analysis not found for given especifications";
                return NotFound(response);
            }

            var listResponseDto = new List<AnalysisResponseDto>();
            AnalysisResponseDto analysisResponseDto;
            WineResponseDto wineResponseDto;
            foreach (var item in analyses)
            {
                analysisResponseDto = _mapper.Map<AnalysisResponseDto>(item);
                wineResponseDto = _mapper.Map<WineResponseDto>(item.Wine);
                analysisResponseDto.Wine = wineResponseDto;

                listResponseDto.Add(analysisResponseDto);
            }


            response.Message = "analyses were found successfully";
            response.Data = listResponseDto;
            return Ok(response); ;
        }
    }
}
