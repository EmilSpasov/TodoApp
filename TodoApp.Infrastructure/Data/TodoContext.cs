using Microsoft.EntityFrameworkCore;
using Task = TodoApp.Core.Models.Task;

namespace TodoApp.Infrastructure.Data;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Task>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}