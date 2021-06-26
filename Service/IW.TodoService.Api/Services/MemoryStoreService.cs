using System;
using Microsoft.Extensions.Caching.Memory;

namespace IW.TodoService.Api.Services
{
   public interface IMemoryStoreService
   {
      T TryGetValue<T>(string cacheKey, Func<T> executeDelegate) where T : class;
      T TryGetValue<T>(string cacheKey) where T : class;
      void TryReplace<T>(string cacheKey, T replaceWith) where T : class;
      void ClearCacheEntry(string cacheKey);
   }

   public class MemoryStoreService : IMemoryStoreService
   {
      private readonly IMemoryCache _cache;
      // private readonly TimeSpan _defaultTimeout;

      public MemoryStoreService(IMemoryCache cache)
      {
         _cache = cache;
      }

      public T TryGetValue<T>(string cacheKey, Func<T> executeDelegate) where T : class
      {
         _cache.TryGetValue(cacheKey, out T result);

         if (result == null)
         {
            // not in cache or a cast error, so get it
            try
            {
               result = executeDelegate();

               if (result != null)
               {
                  _cache.Set(cacheKey, result);
               }
            }
            catch (Exception)
            {
               return null;
            }
         }

         return result;
      }

      public T TryGetValue<T>(string cacheKey) where T : class
      {
         _cache.TryGetValue(cacheKey, out T result);
         return result;
      }

      public void TryReplace<T>(string cacheKey, T replaceWith) where T : class
      {
         try
         {
            ClearCacheEntry(cacheKey);
            _cache.Set(cacheKey, replaceWith);
         }
         catch (Exception)
         {
            // eat it
         }
      }

      public void ClearCacheEntry(string cacheKey)
      {
         if (_cache.TryGetValue(cacheKey, out _))
            _cache.Remove(cacheKey);
      }
   }
}
