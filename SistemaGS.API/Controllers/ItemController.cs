using Microsoft.AspNetCore.Mvc;

namespace SistemaGS.API.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
