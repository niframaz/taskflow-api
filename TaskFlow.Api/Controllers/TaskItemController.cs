using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Abstractions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController(ITaskService taskService, IMapper mapper) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> Get()
        {
            var items = await _taskService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskItem>?> Get(int id)
        {
            var item = await _taskService.GetAsync(id);
            if(item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskItemRequest taskItemrequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var taskItem = _mapper.Map<TaskItem>(taskItemrequest);
            taskItem.UserId = userId;
            var result = await _taskService.AddAsync(taskItem);
            if (result)
                return CreatedAtAction(nameof(Get), new { id = taskItem.Id }, taskItem);
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskItemRequest taskItemrequest)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemrequest);
            var result = await _taskService.UpdateAsync(id, taskItem);
            if (result)
                return NoContent();
            return StatusCode(500);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.RemoveAsync(id);
            if (result)
                return NoContent();
            return StatusCode(500);
        }
    }
}
