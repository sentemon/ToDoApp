using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Core.Entities;

namespace ToDoApp.Infrastructure.Persistence.Configurations;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(t => t.Complete)
            .HasDefaultValue(0)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired();

        builder.Property(t => t.ExpirationDateTime)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
    }
}