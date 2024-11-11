using System.ComponentModel.DataAnnotations;

namespace TodoApp.Core.Models;

public class Task
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Title field is required.")]
    [MaxLength(100, ErrorMessage = "The Title field must be a maximum length of 100 characters.")]
    public string Title { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; }
}