using ToDoApp.Core.Entities;

namespace ToDoApp.Core.Interfaces;

public interface IToDoRepository
{
    Task<ICollection<ToDo>> GetAllAsync();
    Task<ToDo?> GetAsync(Guid id);
    Task<ICollection<ToDo>> GetIncomingAsync();
    Task<ToDo> CreateAsync(ToDo entity);
    Task<ToDo> UpdateAsync(ToDo entity);
    Task SetPercentCompleteAsync(Guid id, double percent);
    Task DeleteAsync(Guid id);
    Task MarkDoneAsync(Guid id);
}