using System;
using System.Threading.Tasks;
using IW.TodoService.Api.DataModels;
using IW.TodoService.Api.Repositories;
using IW.TodoService.Api.Services;
using IW.TodoService.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable StringLiteralTypo
namespace IW.TodoService.Api.Controllers
{
    [ApiController, ApiVersion("2.0")]
    [Route("v{version:apiVersion}/todoitems")]
   public class ToDoItemsV20Controller : AuthenticationController
   {
      private readonly IToDoRepository _toDoRepository;

      public ToDoItemsV20Controller(IUserRepository userRepository, IAuthenticationService authenticationService, IJwtTokenService tokenService, 
         IToDoRepository toDoRepository) 
         : base(userRepository, authenticationService, tokenService)
      {
         _toDoRepository = toDoRepository;
      }

      public async Task<IActionResult> Get(
         [FromQuery(Name = "name")] string nameFilter,
         [FromQuery(Name = "late")] bool? lateFilter,
         [FromQuery(Name = "pastdue")] bool? pastDueFilter,
         [FromQuery(Name = "completed")] bool? completedFilter
         )
      {
         try
         {
            var filter = new ToDoItemFilterCriteria
            {
               Name = nameFilter,
               IsLate = lateFilter,
               IsPastDue = pastDueFilter,
               IsCompleted = completedFilter,
            };
            return Ok(await _toDoRepository.GetToDoItems(filter));
         }
         catch (Exception e)
         {
            ModelState.AddModelError("Exception", e.Message);
         }

         return BadRequest(ModelState);
      }

      [HttpGet("{id}")]
      public async Task<IActionResult> GetById(int id)
      {
         try
         {
            return Ok(await _toDoRepository.GetToDoItem(id));
         }
         catch (Exception e)
         {
            ModelState.AddModelError("Exception", e.Message);
         }

         return BadRequest(ModelState);
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody] ToDoItemCreateRequest req)
      {
         try
         {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = new ToDoItem
            {
               Name = req.Name.Trim(),
               Description = req.Description.Trim(),
               DueDate = req.DueDate,
               AssignedTo = req.AssignedTo.Trim(),
               CreateDate = DateTime.Now,
            };

            return Ok(await _toDoRepository.Save(item));

         }
         catch (Exception e)
         {
            ModelState.AddModelError("Exception", e.Message);
         }

         return BadRequest(ModelState);
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> Put(int id, [FromBody] ToDoItemCreateRequest req)
      {
         try
         {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = await _toDoRepository.GetToDoItem(id);
            if (item == null)
            {
               ModelState.AddModelError("Invalid", "Invalid todo item");
            }
            else
            {
               item.Name = req.Name.Trim();
               item.Description = req.Description.Trim();
               item.DueDate = req.DueDate;
               item.AssignedTo = req.AssignedTo.Trim();
            }

            return Ok(await _toDoRepository.Save(item));

         }
         catch (Exception e)
         {
            ModelState.AddModelError("Exception", e.Message);
         }

         return BadRequest(ModelState);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         try
         {
            await _toDoRepository.Delete(id);

            return Ok();
         }
         catch (Exception e)
         {
            ModelState.AddModelError("Exception", e.Message);
         }

         return BadRequest(ModelState);
      }

      [HttpPatch("{id}/complete")]
      public async Task<IActionResult> Complete(int id, [FromBody] ToDoItemActionRequest req)
      {
         try
         {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = await _toDoRepository.GetToDoItem(id);
            if (item == null)
            {
               ModelState.AddModelError("Invalid", "Invalid todo item");
            }
            else
            {
               item.CompletedDate = req.OnDate ?? DateTime.UtcNow;
               item.CompletedBy = AuthenticatedUser.FullName;
            }

            return Ok(await _toDoRepository.Save(item));
         }
         catch (Exception e)
         {
            ModelState.AddModelError("Exception", e.Message);
         }

         return BadRequest(ModelState);
      }
   }
}
