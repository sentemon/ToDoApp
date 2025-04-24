using ToDoApp.Core.Enums;

namespace ToDoApp.Core.Entities;

public class ToDo
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public double Complete { get; private set; }
    public Priority Priority { get; private set; }
    
    public DateTime ExpirationDateTime { get; private set; }

    private ToDo(string title, string description, Priority priority, DateTime expirationDateTime)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Priority = priority;
        ExpirationDateTime = expirationDateTime;
    }

    public static ToDo CreateInstance(string title, string description, Priority priority, DateTime expirationDateTime)
    {
        return new ToDo(title, description, priority, expirationDateTime);
    }

    public void SetPercentComplete(double percent)
    {
        Complete = percent;
    }
}