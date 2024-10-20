using ToDoApp.Core.Enums;

namespace ToDoApp.Api.DTOs;

public class ToDoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public double PercentComplete { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}