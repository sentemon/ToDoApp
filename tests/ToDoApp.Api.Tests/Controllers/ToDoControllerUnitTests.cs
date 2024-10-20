using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp.Api.Controllers;
using ToDoApp.Api.DTOs;
using ToDoApp.Core.Entities;
using ToDoApp.Core.Interfaces;
using Xunit;

namespace ToDoApp.Api.Tests.Controllers;

public class ToDoControllerTests
{
    private readonly ToDoController _controller;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IToDoRepository> _toDoRepositoryMock;

    public ToDoControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _toDoRepositoryMock = new Mock<IToDoRepository>();
        _controller = new ToDoController(_mapperMock.Object, _toDoRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithToDoItems()
    {
        // Arrange
        var toDoItems = new List<ToDo> { new() { Id = Guid.NewGuid(), Title = "Test ToDo" } };
        var toDoDtos = new List<ToDoDto> { new() { Id = Guid.NewGuid(), Title = "Test ToDo" } };
        _toDoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(toDoItems);
        _mapperMock.Setup(m => m.Map<ICollection<ToDoDto>>(toDoItems)).Returns(toDoDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(toDoDtos);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithToDoItem_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var toDoItem = new ToDo { Id = id, Title = "Test ToDo" };
        var toDoDto = new ToDoDto { Id = id, Title = "Test ToDo" };
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(toDoItem);
        _mapperMock.Setup(m => m.Map<ToDoDto>(toDoItem)).Returns(toDoDto);

        // Act
        var result = await _controller.Get(id);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(toDoDto);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenToDoItemDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ToDo)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithNewToDoItem()
    {
        // Arrange
        var createDto = new CreateToDoDto { Title = "New ToDo" };
        var newToDoItem = new ToDo { Id = Guid.NewGuid(), Title = "New ToDo" };
        var createdToDoDto = new ToDoDto { Id = newToDoItem.Id, Title = "New ToDo" };

        _mapperMock.Setup(m => m.Map<ToDo>(createDto)).Returns(newToDoItem);
        _toDoRepositoryMock.Setup(repo => repo.CreateAsync(newToDoItem)).ReturnsAsync(newToDoItem);
        _mapperMock.Setup(m => m.Map<ToDoDto>(newToDoItem)).Returns(createdToDoDto);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult?.StatusCode.Should().Be(201);
        createdResult?.Value.Should().BeEquivalentTo(createdToDoDto);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var createDto = new CreateToDoDto { Title = "" }; // Invalid DTO
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
    {
        // Arrange
        var toDoDto = new ToDoDto { Id = Guid.NewGuid(), Title = "Updated ToDo" };
        var updatedToDoItem = new ToDo { Id = toDoDto.Id, Title = toDoDto.Title };
        _mapperMock.Setup(m => m.Map<ToDo>(toDoDto)).Returns(updatedToDoItem);
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(toDoDto.Id)).ReturnsAsync(updatedToDoItem);

        // Act
        var result = await _controller.Update(toDoDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenToDoExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var toDoItem = new ToDo { Id = id, Title = "Test ToDo" };
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(toDoItem);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenToDoDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ToDo)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        var result = await _controller.Delete(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
        
    [Fact]
    public async Task SetPercentComplete_ReturnsOk_WhenToDoExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var toDoItem = new ToDo { Id = id, Title = "Test ToDo" };
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(toDoItem);

        // Act
        var result = await _controller.SetPercentComplete(id, 50.0);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task SetPercentComplete_ReturnsNotFound_WhenToDoDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ToDo)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        var result = await _controller.SetPercentComplete(id, 50.0);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task MarkAsDone_ReturnsNoContent_WhenToDoExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var toDoItem = new ToDo { Id = id, Title = "Test ToDo" };
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(toDoItem);

        // Act
        var result = await _controller.MarkAsDone(id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task MarkAsDone_ReturnsNotFound_WhenToDoDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((ToDo)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        var result = await _controller.MarkAsDone(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetIncoming_ReturnsOk_WithIncomingToDoItems()
    {
        // Arrange
        var incomingToDoItems = new List<ToDo> { new ToDo { Id = Guid.NewGuid(), Title = "Incoming ToDo" } };
        var incomingToDoDtos = new List<ToDoDto> { new ToDoDto { Id = Guid.NewGuid(), Title = "Incoming ToDo" } };
        _toDoRepositoryMock.Setup(repo => repo.GetIncomingAsync()).ReturnsAsync(incomingToDoItems);
        _mapperMock.Setup(m => m.Map<ICollection<ToDoDto>>(incomingToDoItems)).Returns(incomingToDoDtos);

        // Act
        var result = await _controller.GetIncoming();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(incomingToDoDtos);
    }
}