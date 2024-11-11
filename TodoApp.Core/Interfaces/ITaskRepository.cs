namespace TodoApp.Core.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<Models.Task>> GetPendingTasksAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Models.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken);
    Task<Models.Task?> GetTaskByIdAsync(int id, CancellationToken cancellationToken);
    Task AddTaskAsync(Models.Task task, CancellationToken cancellationToken);
    Task UpdateTaskAsync(Models.Task task, CancellationToken cancellationToken);
}