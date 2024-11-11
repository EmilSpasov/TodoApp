using FluentAssertions;
using Moq;
using TodoApp.Core.Dtos;
using TodoApp.Core.Interfaces;
using TodoApp.Infrastructure.Services;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace TodoApp.Tests.ServiceTests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<ITaskMapper> _taskMapperMock;
    private readonly TaskService _taskService;
    private readonly CancellationToken _cancellationToken;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskMapperMock = new Mock<ITaskMapper>();
        _taskService = new TaskService(_taskRepositoryMock.Object, _taskMapperMock.Object);
        _cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task GetPendingTasksAsync_ShouldReturnPendingTasks()
    {
        // Arrange
        var pendingTasks = new List<Core.Models.Task> { new Core.Models.Task { Id = 1, Title = "Pending Task 1" } };
        _taskRepositoryMock.Setup(repo => repo.GetPendingTasksAsync(_cancellationToken)).ReturnsAsync(pendingTasks);

        // Act
        var result = await _taskService.GetPendingTasksAsync(_cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(pendingTasks);
    }

    [Fact]
    public async Task GetOverdueTasksAsync_ShouldReturnOverdueTasks()
    {
        // Arrange
        var overdueTasks = new List<Core.Models.Task> { new Core.Models.Task { Id = 2, Title = "Overdue Task 1" } };
        _taskRepositoryMock.Setup(repo => repo.GetOverdueTasksAsync(_cancellationToken)).ReturnsAsync(overdueTasks);

        // Act
        var result = await _taskService.GetOverdueTasksAsync(_cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(overdueTasks);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new Core.Models.Task { Id = 1, Title = "Task 1" };
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1, _cancellationToken)).ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTaskByIdAsync(1, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(task);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1, _cancellationToken)).ReturnsAsync((Core.Models.Task?)null);

        // Act
        var result = await _taskService.GetTaskByIdAsync(1, _cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddTaskAsync_ShouldAddTask()
    {
        // Arrange
        var taskDto = new TaskDto { Title = "New Task" };
        var task = new Core.Models.Task { Id = 1, Title = "New Task" };
        _taskMapperMock.Setup(mapper => mapper.MapToEntity(taskDto)).Returns(task);
        _taskRepositoryMock.Setup(repo => repo.AddTaskAsync(task, _cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _taskService.AddTaskAsync(taskDto, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(task);
        _taskRepositoryMock.Verify(repo => repo.AddTaskAsync(task, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_WhenTaskExists()
    {
        // Arrange
        var taskDto = new TaskDto { Title = "Updated Task" };
        var existingTask = new Core.Models.Task { Id = 1, Title = "Existing Task" };
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1, _cancellationToken)).ReturnsAsync(existingTask);
        _taskMapperMock.Setup(mapper => mapper.MapToEntity(taskDto, existingTask));
        _taskRepositoryMock.Setup(repo => repo.UpdateTaskAsync(existingTask, _cancellationToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _taskService.UpdateTaskAsync(1, taskDto, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(existingTask);
        _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(existingTask, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskDto = new TaskDto { Title = "Updated Task" };
        _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1, _cancellationToken)).ReturnsAsync((Core.Models.Task?)null);

        // Act
        var result = await _taskService.UpdateTaskAsync(1, taskDto, _cancellationToken);

        // Assert
        result.Should().BeNull();
    }
}
