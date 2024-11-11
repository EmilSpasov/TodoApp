using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Dtos;
using TodoApp.Core.Interfaces;

namespace TodoApp.Api.Controllers;

/// <summary>
/// Controller for managing tasks.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    /// <summary>
    /// Interface for managing tasks.
    /// </summary>
    /// <param name="taskService"></param>
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Gets all pending tasks.
    /// </summary>
    /// <returns>A list of pending tasks.</returns>
    /// <response code="200">Returns the list of pending tasks</response>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
    public async Task<IActionResult> GetPendingTasksAsync(CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetPendingTasksAsync(cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Gets all overdue tasks.
    /// </summary>
    /// <returns>A list of overdue tasks.</returns>
    /// <response code="200">Returns the list of overdue tasks</response>
    [HttpGet("overdue")]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), 200)]
    public async Task<IActionResult> GetOverdueTasksAsync(CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetOverdueTasksAsync(cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Gets a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The task with the specified ID.</returns>
    /// <response code="200">Returns the task with the specified ID</response>
    /// <response code="404">If the task is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetTaskByIdAsync(id, cancellationToken);
        if (task == null) return NotFound();

        return Ok(task);
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="taskDto">The task data transfer object.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created task.</returns>
    /// <response code="201">Returns the newly created task</response>
    /// <response code="400">If the task is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] TaskDto taskDto, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _taskService.AddTaskAsync(taskDto, cancellationToken);
            return CreatedAtAction("GetTaskById", new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="taskDto">The task data transfer object.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The updated task.</returns>
    /// <response code="200">Returns the updated task</response>
    /// <response code="400">If the task is invalid</response>
    /// <response code="404">If the task is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTaskAsync(int id, [FromBody] TaskDto taskDto, CancellationToken cancellationToken)
    {
        try
        {
            var updatedTask = await _taskService.UpdateTaskAsync(id, taskDto, cancellationToken);
            if (updatedTask == null) return NotFound();

            return Ok(updatedTask);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

