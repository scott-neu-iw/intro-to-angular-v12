using System;
using System.ComponentModel.DataAnnotations;

namespace IW.TodoService.Api.ViewModels
{
    public class ToDoItemCreateRequest
    {
       [Required]
       public string Name { get; set; }
       [Required]
       public string Description { get; set; }
       public DateTime? DueDate { get; set; }
       public string AssignedTo { get; set; }
    }
}
