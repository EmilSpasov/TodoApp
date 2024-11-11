using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoContext _context;

    public TaskRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Core.Models.Task>> GetPendingTasksAsync(CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => !t.IsCompleted && (t.DueDate == null || t.DueDate >= DateTime.UtcNow))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Core.Models.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => !t.IsCompleted && t.DueDate < DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task<Core.Models.Task?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task AddTaskAsync(Core.Models.Task task, CancellationToken cancellationToken)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTaskAsync(Core.Models.Task task, CancellationToken cancellationToken)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}