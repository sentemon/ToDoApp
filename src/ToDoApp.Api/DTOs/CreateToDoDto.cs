using ToDoApp.Core.Enums;

namespace ToDoApp.Api.DTOs;

public record CreateToDoDto(
    string Title,
    string Description,
    Priority Priority,
    DateTime ExpirationDateTime
);