using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace IW.TodoService.Api.Controllers
{
   [ResponseCache(NoStore = true, Duration = 0)]
   public abstract class BaseController : Controller
    {
    }
}
