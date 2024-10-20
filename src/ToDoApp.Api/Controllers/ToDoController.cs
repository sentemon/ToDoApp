using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.DTOs;
using ToDoApp.Core.Entities;
using ToDoApp.Core.Interfaces;

namespace ToDoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IToDoRepository _toDoRepository;

    public ToDoController(IMapper mapper, IToDoRepository toDoRepository)
    {
        _mapper = mapper;
        _toDoRepository = toDoRepository;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var toDoItems = await _toDoRepository.GetAllAsync();

        return Ok(_mapper.Map<ICollection<ToDoDto>>(toDoItems));
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var toDoItem = await _toDoRepository.GetByIdAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ToDoDto>(toDoItem));
    }

    [HttpGet("getincoming")]
    public async Task<IActionResult> GetIncoming()
    {
        var incomingToDos = await _toDoRepository.GetIncomingAsync();

        return Ok(_mapper.Map<ICollection<ToDoDto>>(incomingToDos));
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateToDoDto createToDoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newToDoItem = _mapper.Map<ToDo>(createToDoDto);
        var createdToDo = await _toDoRepository.CreateAsync(newToDoItem);

        return CreatedAtAction(nameof(Get), new { id = createdToDo.Id }, _mapper.Map<ToDoDto>(createdToDo));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] ToDoDto toDoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedToDoItem = _mapper.Map<ToDo>(toDoDto);
        await _toDoRepository.UpdateAsync(updatedToDoItem);

        return NoContent();
    }

    [HttpPut("setpercentcomplete/{id:guid}")]
    public async Task<IActionResult> SetPercentComplete(Guid id, double percentComplete)
    {
        var toDoItem = await _toDoRepository.GetByIdAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        await _toDoRepository.SetPercentCompleteAsync(id, percentComplete);

        return Ok("Percent complete set successfully");
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var toDoItem = await _toDoRepository.GetByIdAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        await _toDoRepository.DeleteAsync(id);

        return NoContent();
    }

    [HttpPut("markdone/{id:guid}")]
    public async Task<IActionResult> MarkAsDone(Guid id)
    {
        var toDoItem = await _toDoRepository.GetByIdAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        await _toDoRepository.MarkAsDoneAsync(id);

        return NoContent();
    }
}