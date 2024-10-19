using ToDoApp.Core.Entities;
using ToDoApp.Core.Interfaces;

namespace ToDoApp.Infrastructure.Repositories;

public class ToDoRepository : IToDoRepository
{
    public async Task<ICollection<ToDo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ToDo> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ToDo> GetIncoming()
    {
        throw new NotImplementedException();
    }

    public async Task<ToDo> CreateAsync(ToDo entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ToDo> UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task SetPercentComplete(double percent)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task MarkDone(Guid id)
    {
        throw new NotImplementedException();
    }
}