namespace IW.TodoService.Api.DataModels
{
    public class ToDoItemFilterCriteria
    {
       public string Name { get; set; }
       public bool? IsLate { get; set; }
       public bool? IsPastDue { get; set; }
       public bool? IsCompleted { get; set; }
   }
}
