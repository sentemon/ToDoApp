using ToDoApp.Core.Entities;

namespace ToDoApp.Core.Interfaces;

public interface IToDoRepository
{
    Task<ICollection<ToDo>> GetAllAsync();
    Task<ToDo?> GetByIdAsync(Guid id);
    Task<ICollection<ToDo>> GetIncomingAsync();
    Task<ToDo> CreateAsync(ToDo entity);
    Task UpdateAsync(ToDo entity);
    Task SetPercentCompleteAsync(Guid id, double percent);
    Task DeleteAsync(Guid id);
    Task MarkAsDoneAsync(Guid id);
}