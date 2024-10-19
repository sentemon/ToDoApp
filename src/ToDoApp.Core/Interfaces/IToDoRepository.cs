using ToDoApp.Core.Entities;

namespace ToDoApp.Core.Interfaces;

public interface IToDoRepository
{
    Task<ICollection<ToDo>> GetAllAsync();
    Task<ToDo> GetAsync(Guid id);
    Task<ToDo> GetIncoming();
    Task<ToDo> CreateAsync(ToDo entity);
    Task<ToDo> UpdateAsync(Guid id);
    Task SetPercentComplete(double percent);
    Task Delete(Guid id);
    Task MarkDone(Guid id);
}