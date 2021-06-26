using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IW.TodoService.Api.Models
{
   public enum RoleEnum
   {
      CanView = 0,
      CanComplete = 1,
      CanAdd = 2,
      CanEdit = 3,
      CanDelete = 4
   }
}
