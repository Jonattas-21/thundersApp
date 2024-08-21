using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using thundersApp.Dtos;

namespace thundersApp.Controllers
{
    public class OriginController : Controller
    {
        private readonly IOriginService _service;
        private readonly ILogger<OriginController> _logger;
        private readonly IMapper _mapper;

        public OriginController(IOriginService service, IMapper mapper, ILogger<OriginController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [OutputCache(Duration = 90, VaryByQueryKeys = new[] { "id" })]
        [HttpGet("FindById")]
        public ActionResult FindById(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            if (id == Guid.Empty)
            {
                var origin = _service.GetAll();
                var responseList = new List<OriginResponseDto>();

                foreach (var item in origin)
                {
                    var mapped = _mapper.Map<OriginResponseDto>(item);
                    responseList.Add(mapped);
                }

                response.Data = responseList;
            }
            else
            {
                var origin = _service.GetOriginById(id);
                if (origin == null)
                {
                    response.Message = "origin not found for ID: " + id.ToString();
                    return NotFound(response);
                }

                var responseDto = _mapper.Map<OriginResponseDto>(origin);
                response.Data = responseDto;
            }

            response.Message = "origin was found successfully";
            return Ok(response); ;
        }

        [HttpPost("CreateOrigin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateOrigin(string name)
        {
            try
            {
                Origin origin = new Origin();
                origin.Name = name;
                var (result, validations) = _service.CreateOrigin(origin);

                DefaultResponse response = new DefaultResponse();
                if (validations.Count > 0)
                {
                    response.Message = "Error creating origin";
                    response.Data = validations;
                    return BadRequest(response);
                }

                var responseDto = _mapper.Map<OriginResponseDto>(result);

                response.Message = "Origin created successfully";
                response.Data = responseDto;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating Origin");
                return StatusCode(500, "Internal error happened while creating Origin");
            }
        }

        [HttpDelete("DeleteOrigin")]
        public ActionResult DeleteOrigin(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            var result = _service.Delete(id);
            if (!result)
            {
                response.Message = "Error deleting Origin";
                return BadRequest(response);
            }

            response.Message = "Origin deleted successfully";
            return Ok(response);
        }
    }
}
