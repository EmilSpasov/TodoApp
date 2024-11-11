using FluentAssertions;
using TodoApp.Core.Dtos;
using TodoApp.Core.Mappers;
using Xunit;

namespace TodoApp.Tests.MapperTests;

public class TaskMapperTests
{
    private readonly TaskMapper _taskMapper;

    public TaskMapperTests()
    {
        _taskMapper = new TaskMapper();
    }

    [Fact]
    public void MapToEntity_ShouldMapTaskDtoToTask()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = "Test Task",
            DueDate = DateTime.Now,
            IsCompleted = false
        };

        // Act
        var result = _taskMapper.MapToEntity(taskDto);

        // Assert
        result.Should().BeEquivalentTo(taskDto, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void MapToEntity_ShouldUpdateExistingTask()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = "Updated Task",
            DueDate = DateTime.Now.AddDays(1),
            IsCompleted = true
        };
        var task = new Core.Models.Task
        {
            Id = 1,
            Title = "Old Task",
            DueDate = DateTime.Now,
            IsCompleted = false
        };

        // Act
        _taskMapper.MapToEntity(taskDto, task);

        // Assert
        task.Should().BeEquivalentTo(taskDto, options => options.ExcludingMissingMembers());
    }
}