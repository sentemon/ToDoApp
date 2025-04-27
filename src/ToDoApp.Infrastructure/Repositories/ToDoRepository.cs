using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Entities;
using ToDoApp.Core.Enums;
using ToDoApp.Core.Interfaces;
using ToDoApp.Infrastructure.Persistence;

namespace ToDoApp.Infrastructure.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;

    public ToDoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<ToDo>> GetAllAsync()
    {
        return await _context.ToDos
            .OrderBy(t => t.Title)
            .ToListAsync();
    }

    public async Task<ToDo?> GetByIdAsync(Guid id)
    {
        return await _context.ToDos.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<ICollection<ToDo>> GetIncomingAsync()
    {
        return await _context.ToDos
            .Where(t => t.ExpirationDateTime >= DateTime.UtcNow && t.ExpirationDateTime <= DateTime.UtcNow.AddDays(7))
            .OrderBy(t => t.ExpirationDateTime)
            .ToListAsync();
    }

    public async Task<ToDo> CreateAsync(string title, string description, Priority priority,
        DateTime expirationDateTime)
    {
        var toDo = new ToDo(title, description, priority, expirationDateTime);
        _context.ToDos.Add(toDo);

        await _context.SaveChangesAsync();
        return toDo;
    }

    public async Task UpdateAsync(Guid id, string? title, string? description, double? complete, Priority? priority, DateTime? expirationDateTime)
    {
        var todo = await GetByIdAsync(id);
        if (todo is null)
        {
            return;
        }

        todo.Update(title, description, complete, priority, expirationDateTime);
        await _context.SaveChangesAsync();
    }

    public async Task SetPercentCompleteAsync(Guid id, double percent)
    {
        var todo = await GetByIdAsync(id);
        
        if (todo != null)
        {
            todo.SetPercentComplete(percent);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var todo = await GetByIdAsync(id);
        
        if (todo != null)
        {
            _context.ToDos.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAsDoneAsync(Guid id)
    {
        var todo = await GetByIdAsync(id);
        
        if (todo != null)
        {
            todo.SetPercentComplete(100);
            await _context.SaveChangesAsync();
        }
    }
}