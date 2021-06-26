using System.Threading;

namespace IW.TodoService.Api.Services
{
   public interface IKeyGenerationService
   {
      void SetMaxKey(int key);

      /// <summary>
      /// Gets the next in-memory key in a thread safe manner
      /// </summary>
      /// <returns></returns>
      int GetNext();
   }

   public class KeyGenerationService : IKeyGenerationService
   {
      private int _keyIdCount;

      public void SetMaxKey(int key)
      {
         if (_keyIdCount == 0) _keyIdCount = key;
      }

      /// <inheritdoc />
      public int GetNext()
      {
         return Interlocked.Increment(ref _keyIdCount);
      }
   }
}
