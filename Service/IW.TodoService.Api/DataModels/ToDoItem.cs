using System;
using System.ComponentModel.DataAnnotations;

namespace IW.TodoService.Api.DataModels
{
   public class ToDoItem
   {
      public int Id { get; set; }
      [Required]
      public string Name { get; set; }
      [Required]
      public string Description { get; set; }
      public DateTime CreateDate { get; set; }
      public DateTime? DueDate { get; set; }
      public string AssignedTo { get; set; }
      public DateTime? CompletedDate { get; set; }
      public string CompletedBy { get; set; }
      public bool Completed => CompletedDate.HasValue;
      public bool IsPastDue => !CompletedDate.HasValue && DueDate.HasValue && DueDate.Value < DateTime.Now;
      public bool IsLate => IsPastDue || (CompletedDate.HasValue && DueDate.HasValue && CompletedDate.Value > DueDate.Value);
   }
}