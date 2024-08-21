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

        [OutputCache(Duration = 90, VaryByQueryKeys = new[] { "id" })]
        [HttpGet("FindById")]
        public ActionResult FindById(Guid id)
        {
            DefaultResponse response = new DefaultResponse();

            var taskForce = _service.GetTaskById(id);
            if (taskForce == null)
            {
                response.Message = "TaskForce not found for ID: " + id.ToString();
                return NotFound(response);
            }

            var taskForceResponseDto = _mapper.Map<TaskForceResponseDto>(taskForce);
            response.Message = "taskForce found successfully";
            response.Data = taskForceResponseDto;
            return Ok(response); ;
        }

        [HttpPost("CreateTaskForce")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateTaskForce(TaskForceRequestDto item)
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

        [HttpPatch("UpdateTaskForceStatus")]
        public ActionResult UpdateTaskForceStatus(Guid id, int status)
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

        [HttpDelete("DeleteTaskForce")]
        public ActionResult DeleteTaskForce(Guid id)
        {
            try
            {
                DefaultResponse response = new DefaultResponse();
                response.Message = _service.Delete(id) ? "TaskForce deleted successfully" : "TaskForce was not found for ID: " + id.ToString();
                response.Data = id;

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
