using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.Api.Controllers;
using TodoApp.Core.Dtos;
using TodoApp.Core.Interfaces;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace TodoApp.Tests.ControllerTests;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _taskServiceMock;
    private readonly TasksController _controller;
    private readonly CancellationToken _cancellationToken;

    public TasksControllerTests()
    {
        _taskServiceMock = new Mock<ITaskService>();
        _controller = new TasksController(_taskServiceMock.Object);
        _cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldReturn201Created_WhenTaskIsCreated()
    {
        // Arrange
        var taskDto = new TaskDto { Title = "Test Task", DueDate = DateTime.Now, IsCompleted = false };
        var task = new Core.Models.Task { Id = 1, Title = "Test Task", DueDate = DateTime.Now, IsCompleted = false };

        _taskServiceMock.Setup(service => service.AddTaskAsync(It.IsAny<TaskDto>(), _cancellationToken)).ReturnsAsync(task);

        // Act
        var result = await _controller.CreateTaskAsync(taskDto, _cancellationToken);
        var createdResult = result as CreatedAtActionResult;

        // Assert
        createdResult.Should().NotBeNull();
        createdResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult?.Value.Should().BeEquivalentTo(task);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldReturn200OK_WhenTaskIsUpdated()
    {
        // Arrange
        var taskId = 1;
        var taskDto = new TaskDto { Title = "Updated Task", DueDate = DateTime.Now.AddDays(1), IsCompleted = true };
        var updatedTask = new Core.Models.Task { Id = taskId, Title = "Updated Task", DueDate = DateTime.Now.AddDays(1), IsCompleted = true };

        _taskServiceMock.Setup(service => service.UpdateTaskAsync(taskId, taskDto, _cancellationToken)).ReturnsAsync(updatedTask);

        // Act
        var result = await _controller.UpdateTaskAsync(taskId, taskDto, _cancellationToken);
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult?.Value.Should().BeEquivalentTo(updatedTask);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldReturn404NotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = 1;
        var taskDto = new TaskDto { Title = "Non-Existing Task", DueDate = DateTime.Now.AddDays(1), IsCompleted = true };

        _taskServiceMock.Setup(service => service.UpdateTaskAsync(taskId, taskDto, _cancellationToken)).ReturnsAsync(() => null);

        // Act
        var result = await _controller.UpdateTaskAsync(taskId, taskDto, _cancellationToken);
        var notFoundResult = result as NotFoundResult;

        // Assert
        notFoundResult.Should().NotBeNull();
        notFoundResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturn200OK_WhenTaskExists()
    {
        // Arrange
        var taskId = 1;
        var task = new Core.Models.Task { Id = taskId, Title = "Test Task", DueDate = DateTime.Now, IsCompleted = false };

        _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId, _cancellationToken)).ReturnsAsync(task);

        // Act
        var result = await _controller.GetTaskByIdAsync(taskId, _cancellationToken);
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult?.Value.Should().BeEquivalentTo(task);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturn404NotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = 1;

        _taskServiceMock.Setup(service => service.GetTaskByIdAsync(taskId, _cancellationToken)).ReturnsAsync(() => null);

        // Act
        var result = await _controller.GetTaskByIdAsync(taskId, _cancellationToken);
        var notFoundResult = result as NotFoundResult;

        // Assert
        notFoundResult.Should().NotBeNull();
        notFoundResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task GetPendingTasks_ShouldReturn200OK_WhenTasksArePending()
    {
        // Arrange
        var tasks = new List<Core.Models.Task>
        {
            new Core.Models.Task { Id = 1, Title = "Test Task 1", DueDate = DateTime.Now.AddDays(1), IsCompleted = false },
            new Core.Models.Task { Id = 2, Title = "Test Task 2", DueDate = DateTime.Now.AddDays(2), IsCompleted = false }
        };
        _taskServiceMock.Setup(service => service.GetPendingTasksAsync(_cancellationToken)).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetPendingTasksAsync(_cancellationToken);
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult?.Value.Should().BeEquivalentTo(tasks.Select(t => new { t.Title, t.DueDate, t.IsCompleted }));
    }

    [Fact]
    public async Task GetOverdueTasks_ShouldReturn200OK_WhenTasksAreOverdue()
    {
        // Arrange
        var tasks = new List<Core.Models.Task>
        {
            new Core.Models.Task { Id = 1, Title = "Overdue Task 1", DueDate = DateTime.Now.AddDays(-1), IsCompleted = false },
            new Core.Models.Task { Id = 2, Title = "Overdue Task 2", DueDate = DateTime.Now.AddDays(-2), IsCompleted = false }
        };
        _taskServiceMock.Setup(service => service.GetOverdueTasksAsync(_cancellationToken)).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetOverdueTasksAsync(_cancellationToken);
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult?.Value.Should().BeEquivalentTo(tasks.Select(t => new { t.Title, t.DueDate, t.IsCompleted }));
    }
}
