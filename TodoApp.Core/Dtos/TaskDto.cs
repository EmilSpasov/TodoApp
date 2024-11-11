namespace TodoApp.Core.Dtos;

public class TaskDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}