// using System.Net;
// using System.Text;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc.Testing;
// using ToDoApp.Api.DTOs;
// using ToDoApp.Api.Controllers;
// using ToDoApp.Core.Entities;
// using ToDoApp.Core.Enums;
// using Xunit;
//
// namespace ToDoApp.Api.Tests.Controllers;
//
// public class ToDoControllerIntegrationTests : IClassFixture<WebApplicationFactory<ToDoController>>
// {
//     private readonly HttpClient _client;
//
//     public ToDoControllerIntegrationTests(WebApplicationFactory<ToDoController> factory)
//     {
//         _client = factory.CreateClient();
//     }
//
//     [Fact]
//     public async Task GetAll_ReturnsOk_WithToDoItems()
//     {
//         // Act
//         var response = await _client.GetAsync("/api/todo/getall");
//
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.OK);
//     }
//
//     [Fact]
//     public async Task Create_ReturnsCreatedAtAction()
//     {
//         // Arrange
//         var createDto = new CreateToDoDto
//         {
//             Title = "New ToDo",
//             Description = "smth to do",
//             Priority = Priority.Low,
//             ExpirationDateTime = DateTime.UtcNow.AddDays(5)
//         };
//         var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
//
//         // Act
//         var response = await _client.PostAsync("/api/todo/create", content);
//
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.Created);
//     }
//
//     [Fact]
//     public async Task Delete_ReturnsNoContent_WhenToDoItemExists()
//     {
//         // Arrange
//         var createDto = new CreateToDoDto
//         {
//             Title = "ToDo to delete",
//             Description = "smth to do",
//             Priority = Priority.Low,
//             ExpirationDateTime = DateTime.UtcNow.AddDays(4)
//         };
//
//         var createContent = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
//         var createResponse = await _client.PostAsync("/api/todo/create", createContent);
//         var responseContent = await createResponse.Content.ReadAsStringAsync();
//             
//         var options = new JsonSerializerOptions
//         {
//             PropertyNameCaseInsensitive = true,
//             Converters = { new JsonStringEnumConverter() }
//         };
//
//         var createdToDo = JsonSerializer.Deserialize<ToDo>(responseContent, options);
//
//         // Act
//         var deleteResponse = await _client.DeleteAsync($"/api/todo/delete/{createdToDo?.Id}");
//
//         // Assert
//         deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
//
//         // Additional check to make sure that the item has really been deleted
//         var checkResponse = await _client.GetAsync($"/api/todo/get/{createdToDo?.Id}");
//         checkResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
//     }
//         
//     [Fact]
//     public async Task Get_ReturnsNotFound_WhenToDoItemDoesNotExist()
//     {
//         // Act
//         var response = await _client.GetAsync($"/api/todo/get/{Guid.NewGuid()}");
//
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//     }
//
//     [Fact]
//     public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
//     {
//         // Arrange
//         var createDto = new CreateToDoDto
//         {
//             Title = "ToDo to update",
//             Description = "smth to update",
//             Priority = Priority.Low,
//             ExpirationDateTime = DateTime.UtcNow.AddDays(5)
//         };
//
//         var createContent = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");
//         var createResponse = await _client.PostAsync("/api/todo/create", createContent);
//         var responseContent = await createResponse.Content.ReadAsStringAsync();
//
//         var createdToDo = JsonSerializer.Deserialize<ToDo>(responseContent);
//         
//         if (createdToDo != null)
//         {
//             var updatedDto = new ToDo
//             {
//                 Id = createdToDo.Id,
//                 Title = "Updated ToDo",
//                 Description = "Updated description",
//                 Priority = Priority.Medium,
//                 ExpirationDateTime = DateTime.UtcNow.AddDays(10)
//             };
//             var updatedContent = new StringContent(JsonSerializer.Serialize(updatedDto), Encoding.UTF8, "application/json");
//
//             // Act
//             var updateResponse = await _client.PutAsync("/api/todo/update", updatedContent);
//
//             // Assert
//             updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
//         }
//     }
// }