using ToDoApp.Core.Enums;

namespace ToDoApp.Core.Entities;

public class ToDo
{
    public Guid Id { get; init; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Complete { get; set; }
    public Priority Priority { get; set; }
}