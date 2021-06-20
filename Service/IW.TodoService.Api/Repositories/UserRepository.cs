using System.Collections.Generic;
using IW.TodoService.Api.DataModels;
using IW.TodoService.Api.Models;

namespace IW.TodoService.Api.Repositories
{
   public interface IUserRepository
   {
      User Get(string username);
      User Get(long id);
   }

   public class UserRepository : IUserRepository
   {
      public User Get(string username)
      {
         var user =  GetUser();
         user.Username = username;
         return user;
      }

      public User Get(long id)
      {
         return GetUser();
      }

      private static User GetUser()
      {
         return new User
         {
            Id = 100,
            FirstName = "Demo",
            LastName = "User",
            EmailAddress = "demo.user@email.com",
            Roles = new List<RoleEnum>
            {
               RoleEnum.CanView,
               RoleEnum.CanAdd,
               RoleEnum.CanEdit,
               RoleEnum.CanComplete,
               RoleEnum.CanDelete
            }
         };
      }
   }
}
