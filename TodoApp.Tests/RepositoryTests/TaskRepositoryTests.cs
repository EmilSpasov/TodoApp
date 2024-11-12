using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace TodoApp.Tests.RepositoryTests;

public class TaskRepositoryTests : IDisposable
{
    private readonly TodoContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "TodoAppTestDb")
            .Options;

        _context = new TodoContext(dbContextOptions);
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public async Task AddTaskAsync_ShouldAddTask()
    {
        // Arrange
        var task = new Core.Models.Task { Title = "Test Task", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = false };

        // Act
        await _repository.AddTaskAsync(task, CancellationToken.None);
        var tasks = await _context.Tasks.ToListAsync();

        // Assert
        tasks.Should().ContainSingle();
        tasks.First().Title.Should().Be("Test Task");
    }

    [Fact]
    public async Task GetPendingTasksAsync_ShouldReturnPendingTasks()
    {
        // Arrange
        var task1 = new Core.Models.Task { Title = "Pending Task", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = false };
        var task2 = new Core.Models.Task { Title = "Completed Task", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = true };
        await _context.Tasks.AddRangeAsync(task1, task2);
        await _context.SaveChangesAsync();

        // Act
        var pendingTasks = await _repository.GetPendingTasksAsync(CancellationToken.None);

        // Assert
        pendingTasks?.Should().NotBeNullOrEmpty();
        pendingTasks?.Should().ContainSingle();
        pendingTasks?.First().Title.Should().Be("Pending Task");
    }

    [Fact]
    public async Task GetOverdueTasksAsync_ShouldReturnOverdueTasks()
    {
        // Arrange
        var task1 = new Core.Models.Task { Title = "Overdue Task", DueDate = DateTime.UtcNow.AddDays(-1), IsCompleted = false };
        var task2 = new Core.Models.Task { Title = "Completed Task", DueDate = DateTime.UtcNow.AddDays(-1), IsCompleted = true };
        await _context.Tasks.AddRangeAsync(task1, task2);
        await _context.SaveChangesAsync();

        // Act
        var overdueTasks = await _repository.GetOverdueTasksAsync(CancellationToken.None);

        // Assert
        overdueTasks?.Should().NotBeNullOrEmpty();
        overdueTasks?.Should().ContainSingle();
        overdueTasks?.First().Title.Should().Be("Overdue Task");
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new Core.Models.Task { Title = "Test Task", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = false };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        var retrievedTask = await _repository.GetTaskByIdAsync(task.Id, CancellationToken.None);

        // Assert
        retrievedTask.Should().NotBeNull();
        retrievedTask?.Title.Should().Be("Test Task");
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Act
        var retrievedTask = await _repository.GetTaskByIdAsync(999, CancellationToken.None);

        // Assert
        retrievedTask.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask()
    {
        // Arrange
        var task = new Core.Models.Task { Title = "Test Task", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = false };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        task.Title = "Updated Task";
        await _repository.UpdateTaskAsync(task, CancellationToken.None);
        var updatedTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id);

        // Assert
        updatedTask.Should().NotBeNull();
        updatedTask?.Title.Should().Be("Updated Task");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}