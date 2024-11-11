using TodoApp.Core.Dtos;
using Task = TodoApp.Core.Models.Task;

namespace TodoApp.Core.Interfaces;

public interface ITaskMapper
{
    Task MapToEntity(TaskDto taskDto);
    void MapToEntity(TaskDto taskDto, Task task);
}