using TodoApp.Core.Dtos;
using TodoApp.Core.Interfaces;
using Task = TodoApp.Core.Models.Task;

namespace TodoApp.Core.Mappers;

public class TaskMapper : ITaskMapper
{
    public Task MapToEntity(TaskDto taskDto)
    {
        return new Task
        {
            Title = taskDto.Title,
            DueDate = taskDto.DueDate,
            IsCompleted = taskDto.IsCompleted
        };
    }

    public void MapToEntity(TaskDto taskDto, Task task)
    {
        task.Title = taskDto.Title;
        task.DueDate = taskDto.DueDate;
        task.IsCompleted = taskDto.IsCompleted;
    }
}