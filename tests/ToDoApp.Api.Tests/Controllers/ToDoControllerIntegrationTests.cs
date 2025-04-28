using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ToDoApp.Api.Controllers;
using ToDoApp.Api.DTOs;
using ToDoApp.Core.Entities;
using ToDoApp.Core.Enums;
using Xunit;

namespace ToDoApp.Api.Tests.Controllers;

public class ToDoControllerIntegrationTests(WebApplicationFactory<ToDoController> factory) : IClassFixture<WebApplicationFactory<ToDoController>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task GetAll_ReturnsOk_WithToDoItems()
    {
        // Act
        var response = await _client.GetAsync("/api/todo/getall");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateToDoDto("New ToDo", "Something to do", Priority.Low, DateTime.UtcNow.AddDays(5));
        var content = new StringContent(JsonSerializer.Serialize(createDto, _jsonOptions), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/todo/create", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenToDoItemExists()
    {
        // Arrange
        var createDto = new CreateToDoDto("New ToDo to delete", "Something to do", Priority.Low, DateTime.UtcNow.AddDays(5));
        var createContent = new StringContent(JsonSerializer.Serialize(createDto, _jsonOptions), Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/api/todo/create", createContent);

        var createdBody = await createResponse.Content.ReadAsStringAsync();
        var createdToDo = JsonSerializer.Deserialize<ToDo>(createdBody, _jsonOptions);
        

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/todo/delete/{createdToDo?.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Additional check, verify it was deleted
        var checkResponse = await _client.GetAsync($"/api/todo/get/{createdToDo?.Id}");
        checkResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenToDoItemDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"/api/todo/get/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
    {
        // Arrange
        var createDto = new CreateToDoDto("ToDo to update", "Initial description", Priority.Low, DateTime.UtcNow.AddDays(5));
        var createContent = new StringContent(JsonSerializer.Serialize(createDto, _jsonOptions), Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/api/todo/create", createContent);
        
        var createdBody = await createResponse.Content.ReadAsStringAsync();
        var createdToDo = JsonSerializer.Deserialize<ToDo>(createdBody, _jsonOptions);

        createdToDo.Should().NotBeNull();
        
        var updateUrl = $"/api/todo/update?id={createdToDo?.Id}&title=Updated Title&description=Updated Description&complete=50&priority=High&expirationDateTime={DateTime.UtcNow.AddDays(10):O}";

        // Act
        var updateResponse = await _client.PutAsync(updateUrl, null);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
