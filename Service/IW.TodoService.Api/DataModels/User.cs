using System.Collections.Generic;
using IW.TodoService.Api.Models;

namespace IW.TodoService.Api.DataModels
{
   public class User
   {
      public long Id { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string FullName => $"{FirstName} {LastName}";
      public string EmailAddress { get; set; }
      public string Username { get; set; }
      public List<RoleEnum> Roles { get; set; }
   }
}
