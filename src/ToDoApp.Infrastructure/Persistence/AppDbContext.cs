using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Entities;

namespace ToDoApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ToDo>().HasKey(t => t.Id);

        modelBuilder.Entity<ToDo>().Property(t => t.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ToDo>().Property(t => t.Title)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<ToDo>().Property(t => t.Description)
            .HasMaxLength(2048)
            .IsRequired(false);

        modelBuilder.Entity<ToDo>().Property(t => t.Complete)
            .HasDefaultValue(0)
            .HasPrecision(5, 2)
            .IsRequired();

        modelBuilder.Entity<ToDo>().Property(t => t.Priority)
            .IsRequired();

        modelBuilder.Entity<ToDo>().Property(t => t.ExpirationDateTime)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
    }
}