using TodoApp.Core.Dtos;

namespace TodoApp.Core.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<Models.Task>> GetPendingTasksAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Models.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken);
    Task<Models.Task?> GetTaskByIdAsync(int id, CancellationToken cancellationToken);
    Task<Models.Task> AddTaskAsync(TaskDto task, CancellationToken cancellationToken);
    Task<Models.Task?> UpdateTaskAsync(int id, TaskDto task, CancellationToken cancellationToken);
}