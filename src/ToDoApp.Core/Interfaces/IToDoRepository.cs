using ToDoApp.Core.Entities;
using ToDoApp.Core.Enums;

namespace ToDoApp.Core.Interfaces;

public interface IToDoRepository
{
    Task<ICollection<ToDo>> GetAllAsync();
    Task<ToDo?> GetByIdAsync(Guid id);
    Task<ICollection<ToDo>> GetIncomingAsync();
    Task<ToDo> CreateAsync(string title, string description, Priority priority, DateTime expirationDateTime);
    Task UpdateAsync(Guid id, string? title, string? description, double? complete, Priority? priority, DateTime? expirationDateTime);
    Task SetPercentCompleteAsync(Guid id, double percent);
    Task DeleteAsync(Guid id);
    Task MarkAsDoneAsync(Guid id);
}