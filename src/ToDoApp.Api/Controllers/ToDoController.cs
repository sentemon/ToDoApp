using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.DTOs;
using ToDoApp.Core.Enums;
using ToDoApp.Core.Interfaces;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoRepository _toDoRepository;

    public ToDoController(IToDoRepository toDoRepository)
    {
        _toDoRepository = toDoRepository;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var toDos = await _toDoRepository.GetAllAsync();

        return Ok(toDos);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var toDo = await _toDoRepository.GetByIdAsync(id);
        if (toDo is null)
        {
            return NotFound();
        }

        return Ok(toDo);
    }

    [HttpGet("getincoming")]
    public async Task<IActionResult> GetIncoming()
    {
        var incomingToDos = await _toDoRepository.GetIncomingAsync();

        return Ok(incomingToDos);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateToDoDto createToDoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var toDo = await _toDoRepository.CreateAsync(createToDoDto.Title, createToDoDto.Description, createToDoDto.Priority, createToDoDto.ExpirationDateTime);

        return CreatedAtAction(nameof(Get), new { id = toDo.Id }, toDo);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(Guid id, string? title, string? description, double? complete, Priority? priority, DateTime? expirationDateTime)
    {
        var toDo = await _toDoRepository.GetByIdAsync(id);
        if (toDo == null)
        {
            return NotFound();
        }

        await _toDoRepository.UpdateAsync(id, title, description, complete, priority, expirationDateTime);

        return NoContent();
    }

    [HttpPut("setpercentcomplete/{id:guid}")]
    public async Task<IActionResult> SetPercentComplete(Guid id, double percentComplete)
    {
        var toDo = await _toDoRepository.GetByIdAsync(id);
        if (toDo == null)
        {
            return NotFound();
        }

        await _toDoRepository.SetPercentCompleteAsync(id, percentComplete);

        return Ok("Percent complete set successfully");
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var toDo = await _toDoRepository.GetByIdAsync(id);
        if (toDo is null)
        {
            return NotFound();
        }

        await _toDoRepository.DeleteAsync(id);

        return NoContent();
    }

    [HttpPut("markdone/{id:guid}")]
    public async Task<IActionResult> MarkAsDone(Guid id)
    {
        var toDo = await _toDoRepository.GetByIdAsync(id);
        if (toDo is null)
        {
            return NotFound();
        }

        await _toDoRepository.MarkAsDoneAsync(id);

        return NoContent();
    }
}