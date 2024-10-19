using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Entities;
using ToDoApp.Infrastructure.Persistence.Configurations;

namespace ToDoApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ToDoConfiguration());
    }
}