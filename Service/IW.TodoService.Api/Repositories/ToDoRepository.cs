using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IW.TodoService.Api.DataModels;
using IW.TodoService.Api.Services;

namespace IW.TodoService.Api.Repositories
{
   public interface IToDoRepository
   {
      Task<List<ToDoItem>> GetToDoItems(ToDoItemFilterCriteria filter);
      Task<ToDoItem> GetToDoItem(int id);
      Task<ToDoItem> Save(ToDoItem item);
      Task Delete(int id);
   }

   public class ToDoRepository : IToDoRepository
   {
      private readonly IMemoryStoreService _memoryStoreService;
      private readonly IKeyGenerationService _keyGenerationService;

      private const string CACHE_TODO_ITEMS = "ToDoItems";

      public ToDoRepository(IMemoryStoreService memoryStoreService, IKeyGenerationService keyGenerationService)
      {
         _memoryStoreService = memoryStoreService;
         _keyGenerationService = keyGenerationService;

         var maxId = GetDummyList().Max(i => i.Id);
         _keyGenerationService.SetMaxKey(maxId);
      }

      public async Task<List<ToDoItem>> GetToDoItems(ToDoItemFilterCriteria filter)
      {
         return await Task.Run(() =>
         {
            var items = _memoryStoreService.TryGetValue(CACHE_TODO_ITEMS, GetDummyList);

            return ApplyFilter(filter, items).ToList();
         });
      }

      public async Task<ToDoItem> GetToDoItem(int id)
      {
         var items = await GetToDoItems(null);
         return items.FirstOrDefault(i => i.Id == id);
      }

      public async Task<ToDoItem> Save(ToDoItem item)
      {
         var items = await GetToDoItems(null);
         if (item.Id == 0)
         {
            item.Id = _keyGenerationService.GetNext();
            items.Add(item);
         }
         else
         {
            var index = items.IndexOf(items.First(i => i.Id == item.Id));
            items[index] = item;
         }

         _memoryStoreService.TryReplace(CACHE_TODO_ITEMS, items);

         return item;
      }

      public async Task Delete(int id)
      {
         var items = await GetToDoItems(null);
         items.RemoveAll(i => i.Id == id);
         _memoryStoreService.TryReplace(CACHE_TODO_ITEMS, items);
      }

      private static List<ToDoItem> GetDummyList()
      {
         return new List<ToDoItem>
         {
            new ToDoItem
            {
               Id = 1,
               Name = "First To Do Item",
               Description = "I really need to get this done",
               CreateDate = new DateTime(2018,8,1),
               DueDate = DateTime.Now.Date.AddMonths(1),
               AssignedTo = "Scott Neu"
            },
            new ToDoItem
            {
               Id = 2,
               Name = "Another Really Important Item",
               Description = "Something super important",
               CreateDate = new DateTime(2018,8,3),
               DueDate = DateTime.Now.Date.AddDays(3),
               AssignedTo = "Ryan Jackson"
            },
            new ToDoItem
            {
               Id = 3,
               Name = "Its not that important",
               Description = "Something super important",
               CreateDate = new DateTime(2018,8,2)
            },
            new ToDoItem
            {
               Id = 4,
               Name = "A Past Due Item",
               Description = "Should have done this earlier",
               CreateDate = new DateTime(2018,8,4,8,23,12),
               DueDate = new DateTime(2018,8,6),
               AssignedTo = "Joe User"
            },
            new ToDoItem
            {
               Id = 5,
               Name = "Oops, its late",
               Description = "Better late than never",
               CreateDate = new DateTime(2018,7,12),
               DueDate = new DateTime(2018,8,1),
               AssignedTo = "Mr. Delinquent",
               CompletedDate = new DateTime(2018, 8, 20, 8, 30, 23),
               CompletedBy = "Mr. Delinquent"
            }
         };
      }

      private static List<ToDoItem> ApplyFilter(ToDoItemFilterCriteria filter, List<ToDoItem> items)
      {
         if (filter == null) return items;

         var tmpItems = items.AsQueryable();

         if (!string.IsNullOrWhiteSpace(filter.Name))
         {
            tmpItems = tmpItems.Where(i =>
               CultureInfo.CurrentCulture.CompareInfo.IndexOf
                  (i.Name, filter.Name.Trim(), CompareOptions.IgnoreCase) >= 0
            );
         }

         if (filter.IsLate.HasValue)
         {
            tmpItems = tmpItems.Where(i => i.IsLate == filter.IsLate.Value);
         }

         if (filter.IsCompleted.HasValue)
         {
            tmpItems = tmpItems.Where(i => i.Completed == filter.IsCompleted.Value);
         }

         if (filter.IsPastDue.HasValue)
         {
            tmpItems = tmpItems.Where(i => i.IsPastDue == filter.IsPastDue.Value);
         }

         return tmpItems.ToList();
      }

   }
}
