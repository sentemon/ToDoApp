using ToDoApp.Core.Enums;

namespace ToDoApp.Api.DTOs;

public class CreateToDoDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}