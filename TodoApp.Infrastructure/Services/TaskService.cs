using TodoApp.Core.Dtos;
using TodoApp.Core.Interfaces;

namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Service for managing tasks.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskMapper _taskMapper;

    public TaskService(ITaskRepository taskRepository, ITaskMapper taskMapper)
    {
        _taskRepository = taskRepository;
        _taskMapper = taskMapper;
    }

    /// <summary>
    /// Gets all pending tasks.
    /// </summary>
    /// <returns>A list of pending tasks.</returns>
    public async Task<IEnumerable<Core.Models.Task>> GetPendingTasksAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _taskRepository.GetPendingTasksAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving pending tasks.", ex);
        }
    }

    /// <summary>
    /// Gets all overdue tasks.
    /// </summary>
    /// <returns>A list of overdue tasks.</returns>
    public async Task<IEnumerable<Core.Models.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _taskRepository.GetOverdueTasksAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving overdue tasks.", ex);
        }
    }

    /// <summary>
    /// Gets a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>The task with the specified ID.</returns>
    public async Task<Core.Models.Task?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _taskRepository.GetTaskByIdAsync(id, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the task with ID {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new task.
    /// </summary>
    /// <param name="taskDto">The task data transfer object.</param>
    /// <returns>The created task.</returns>
    public async Task<Core.Models.Task> AddTaskAsync(TaskDto taskDto, CancellationToken cancellationToken)
    {
        try
        {
            var task = _taskMapper.MapToEntity(taskDto);
            await _taskRepository.AddTaskAsync(task, cancellationToken).ConfigureAwait(false);
            return task;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the task.", ex);
        }
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="taskDto">The task data transfer object.</param>
    /// <returns>The updated task.</returns>
    public async Task<Core.Models.Task?> UpdateTaskAsync(int id, TaskDto taskDto, CancellationToken cancellationToken)
    {
        try
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (existingTask == null) return null;

            _taskMapper.MapToEntity(taskDto, existingTask);
            await _taskRepository.UpdateTaskAsync(existingTask, cancellationToken).ConfigureAwait(false);

            return existingTask;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the task with ID {id}.", ex);
        }
    }
}
