using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Newtonsoft.Json.Linq;
using thundersApp.Dtos;

namespace thundersApp.Controllers
{
    [Route("TaskForce")]
    public class TaskForceController : Controller
    {
        private readonly ITaskForceService _service;
        private readonly ILogger<TaskForceController> _logger;
        private readonly IMapper _mapper;

        public TaskForceController(ITaskForceService service, IMapper mapper, ILogger<TaskForceController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [OutputCache(Duration = 15, VaryByQueryKeys = new[] { "id" })]
        [HttpGet("FindById")]
        public ActionResult FindById(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            if (id == Guid.Empty)
            {
                var tasks = _service.GetAll().ToList();
                var responseList = new List<TaskForceResponseDto>();

                foreach (var item in tasks)
                {
                    var mapped = _mapper.Map<TaskForceResponseDto>(item);
                    mapped.Origin = item.Origin.Name;
                    responseList.Add(mapped);
                }

                response.Data = responseList;
            }
            else
            {
                var taskForce = _service.GetTaskById(id);
                if (taskForce == null)
                {
                    response.Message = "TaskForce not found for ID: " + id.ToString();
                    return NotFound(response);
                }

                var taskForceResponseDto = _mapper.Map<TaskForceResponseDto>(taskForce);
                response.Data = taskForceResponseDto;
                
            }

            response.Message = "taskForce found successfully";
            return Ok(response);
        }

        [HttpPost("CreateTaskForce")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateTaskForce([FromBody] TaskForceRequestDto item)
        {
            try
            {
                var taskData = _mapper.Map<TaskForce>(item);
                var (result, validations) = _service.CreateTask(taskData, item.OriginId);

                DefaultResponse response = new DefaultResponse();
                if (validations.Count > 0)
                {
                    response.Message = "Error creating TaskForce";
                    response.Data = validations;
                    return BadRequest(response);
                }

                var taskForceResponseDto = _mapper.Map<TaskForceResponseDto>(result);

                response.Message = "taskforce created successfully";
                response.Data = taskForceResponseDto;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating taskforce");
                return StatusCode(500, "Internal error happened while creating taskforce");
            }
        }

        [HttpPatch("UpdateTaskForceStatus/{id}")]
        public ActionResult UpdateTaskForceStatus(Guid id, [FromQuery] int status)
        {
            try
            {
                DefaultResponse response = new DefaultResponse();

                if (Enum.IsDefined(typeof(StatusEnum), status))
                {
                    var result = _service.UpdateStatus(id, (StatusEnum)status);

                    if (!result)
                    {
                        response.Message = "TaskForce not found for ID: " + id.ToString();
                        return NotFound(response);
                    }
                    else
                    {
                        response.Message = "TaskForce status updated successfully";
                        response.Data = id;
                        return Ok(response);
                    }
                }
                else
                {
                    response.Message = "Invalid Status number";
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating wine");
                return StatusCode(500, "Internal error happened while updating wine");
            }
        }

        [HttpDelete("DeleteTaskForce/{id}")]
        public ActionResult DeleteTaskForce(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            try
            {
                var result = _service.Delete(id);
                if (!result)
                {
                    response.Message = "Id taskforce not found";
                    return BadRequest(response);
                }

                response.Message = "taskforce deleted successfully";
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting TaskForce");
                return StatusCode(500, "Internal error happened while deleting TaskForce");
            }
        }
    }
}
