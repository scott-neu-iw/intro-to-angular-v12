using System;
using IW.TodoService.Api.DataModels;
using IW.TodoService.Api.Repositories;
using IW.TodoService.Api.Services;

namespace IW.TodoService.Api.Controllers
{
   public class BaseAuthenticatedController : BaseController
   {
      private readonly IUserRepository _userRepository;
      protected User AuthenticatedUser => GetAuthenticatedUser();

      public BaseAuthenticatedController(IUserRepository userRepository)
      {
         _userRepository = userRepository;
      }

      private User GetAuthenticatedUser()
      {
         try
         {
            var userId = JwtTokenService.ToUser(HttpContext.User)?.Id;
            return userId == null ? null : _userRepository.Get(userId.Value);
         }
         catch (Exception)
         {
            return null;
         }
      }
   }
}
